/* Copyright (c) 2006 Google Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Net;
using System.Security.Cryptography;

namespace EasyKeys.Google.GData.Client
{
    public interface ICreateHttpRequest
    {
        HttpWebRequest Create(Uri target);
    }

    public class HttpRequestFactory : ICreateHttpRequest
    {
        public HttpWebRequest Create(Uri target)
        {
            return WebRequest.Create(target) as HttpWebRequest;
        }
    }

    /// <summary>
    /// this is the static collection of all google service names
    /// </summary>
    public static class ServiceNames
    {
        public static string YouTube = "youtube";
        public static string Calendar = "cl";
        public static string Documents = "writely";
    }

    /// <summary>
    /// Base authentication class. Takes credentials and applicationname
    /// and is able to create a HttpWebRequest augmented with the right
    /// authentication
    /// </summary>
    /// <returns></returns>
    public abstract class Authenticator
    {
        private string _applicationName;
        private string _developerKey;
        private ICreateHttpRequest _requestFactory;

        /// <summary>
        /// an unauthenticated use case
        /// </summary>
        /// <param name="applicationName"></param>
        /// <returns></returns>
        public Authenticator(string applicationName)
        {
            _applicationName = applicationName;
            _requestFactory = new HttpRequestFactory();
        }

        public ICreateHttpRequest RequestFactory
        {
            get
            {
                return _requestFactory;
            }

            set
            {
                _requestFactory = value;
            }
        }

        /// <summary>
        /// Creates a HttpWebRequest object that can be used against a given service.
        /// for a RequestSetting object that is using client login, this might call
        /// to get an authentication token from the service, if it is not already set.
        ///
        /// if this uses client login, and you need to use a proxy, set the application wide
        /// proxy first using the GlobalProxySelection
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="httpMethod"></param>
        /// <param name="targetUri"></param>
        /// <returns></returns>
        public HttpWebRequest CreateHttpWebRequest(string httpMethod, Uri targetUri)
        {
            Uri uriResult = ApplyAuthenticationToUri(targetUri);

            if (_requestFactory != null)
            {
                HttpWebRequest request = _requestFactory.Create(uriResult);
                // turn off autoredirect
                request.AllowAutoRedirect = false;
                request.Method = httpMethod;
                ApplyAuthenticationToRequest(request);
                return request;
            }

            return null;
        }

        /// <summary>
        /// returns the application name
        /// </summary>
        /// <returns></returns>
        public string Application
        {
            get
            {
                return _applicationName;
            }
        }

        /// <summary>
        /// primarily for YouTube. allows you to set the developer key used
        /// </summary>
        public string DeveloperKey
        {
            get
            {
                return _developerKey;
            }

            set
            {
                _developerKey = value;
            }
        }

        /// <summary>
        /// Takes an existing httpwebrequest and modifies its headers according to
        /// the authentication system used.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual void ApplyAuthenticationToRequest(HttpWebRequest request)
        {
            /// adds the developer key if present
            if (DeveloperKey != null)
            {
                string strHeader = GoogleAuthentication.YouTubeDevKey + DeveloperKey;
                request.Headers.Add(strHeader);
            }
        }

        /// <summary>
        /// Takes an existing httpwebrequest and modifies its uri according to
        /// the authentication system used. Only overridden in 2-leggedoauth case
        /// </summary>
        /// <param name="source">the original uri</param>
        /// <returns></returns>
        public virtual Uri ApplyAuthenticationToUri(Uri source)
        {
            return source;
        }
    }

    public class ClientLoginAuthenticator : Authenticator
    {
        private GDataCredentials _credentials;
        private Uri _loginHandler;
        private string _serviceName;

        /// <summary>
        /// a constructor for client login use cases
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ClientLoginAuthenticator(string applicationName, string serviceName, string username, string password) : this(applicationName, serviceName, new GDataCredentials(username, password))
        {
        }

        /// <summary>
        /// a constructor for client login use cases
        /// </summary>
        /// <param name="applicationName">The name of the application</param>
        /// <param name="credentials">the user credentials</param>
        /// <returns></returns>
        public ClientLoginAuthenticator(
            string applicationName,
            string serviceName,
            GDataCredentials credentials)
            : this(applicationName, serviceName, credentials, null)
        {
        }

        /// <summary>
        /// a constructor for client login use cases
        /// </summary>
        /// <param name="applicationName">The name of the application</param>
        /// <param name="credentials">the user credentials</param>
        /// <returns></returns>
        public ClientLoginAuthenticator(
            string applicationName,
            string serviceName,
            GDataCredentials credentials,
            Uri clientLoginHandler)
            : base(applicationName)
        {
            _credentials = credentials;
            _serviceName = serviceName;
            _loginHandler = clientLoginHandler == null ?
                new Uri(GoogleAuthentication.UriHandler) : clientLoginHandler;
        }

        /// <summary>
        /// returns the Credentials in case of a client login scenario
        /// </summary>
        /// <returns></returns>
        public GDataCredentials Credentials
        {
            get
            {
                return _credentials;
            }
        }

        /// <summary>
        /// returns the service this authenticator is working against
        /// </summary>
        /// <returns></returns>
        public string Service
        {
            get
            {
                return _serviceName;
            }
        }

        /// <summary>
        /// returns the loginhandler that is used to acquire the token from
        /// </summary>
        /// <returns></returns>
        public Uri LoginHandler
        {
            get
            {
                return _loginHandler;
            }
        }

        /// <summary>
        /// Takes an existing httpwebrequest and modifies its headers according to
        /// the authentication system used.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override void ApplyAuthenticationToRequest(HttpWebRequest request)
        {
            base.ApplyAuthenticationToRequest(request);
            EnsureClientLoginCredentials(request);
            if (!String.IsNullOrEmpty(Credentials.ClientToken))
            {
                string strHeader = GoogleAuthentication.Header + Credentials.ClientToken;
                request.Headers.Add(strHeader);
            }
        }

        private void EnsureClientLoginCredentials(HttpWebRequest request)
        {
            if (String.IsNullOrEmpty(Credentials.ClientToken))
            {
                Credentials.ClientToken = Utilities.QueryClientLoginToken(
                    Credentials,
                    Service,
                    Application,
                    false,
                    LoginHandler);
            }
        }
    }

    public class AuthSubAuthenticator : Authenticator
    {
        private string _authSubToken;
        private AsymmetricAlgorithm _privateKey;

        /// <summary>
        /// a constructor for a web application authentication scenario
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="authSubToken"></param>
        /// <returns></returns>
        public AuthSubAuthenticator(string applicationName, string authSubToken)
            : this(applicationName, authSubToken, null)
        {
        }

        /// <summary>
        /// a constructor for a web application authentication scenario
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="authSubToken"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public AuthSubAuthenticator(
            string applicationName,
            string authSubToken,
            AsymmetricAlgorithm privateKey)
            : base(applicationName)
        {
            _privateKey = privateKey;
            _authSubToken = authSubToken;
        }

        /// <summary>
        /// returns the authsub token to use for a webapplication scenario
        /// </summary>
        /// <returns></returns>
        public string Token
        {
            get
            {
                return _authSubToken;
            }
        }

        /// <summary>
        /// returns the private key used for authsub authentication
        /// </summary>
        /// <returns></returns>
        public AsymmetricAlgorithm PrivateKey
        {
            get
            {
                return _privateKey;
            }
        }

        /// <summary>
        /// Takes an existing httpwebrequest and modifies its headers according to
        /// the authentication system used.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override void ApplyAuthenticationToRequest(HttpWebRequest request)
        {
            base.ApplyAuthenticationToRequest(request);

            string header = AuthSubUtil.formAuthorizationHeader(
                Token,
                PrivateKey,
                request.RequestUri,
                request.Method);
            request.Headers.Add(header);
        }
    }

    public abstract class OAuthAuthenticator : Authenticator
    {
        private string _consumerKey;
        private string _consumerSecret;

        public OAuthAuthenticator(
            string applicationName,
            string consumerKey,
            string consumerSecret)
            : base(applicationName)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
        }

        /// <summary>
        /// returns the ConsumerKey
        /// </summary>
        /// <returns></returns>
        public string ConsumerKey
        {
            get
            {
                return _consumerKey;
            }
        }

        /// <summary>
        /// returns the ConsumerSecret
        /// </summary>
        /// <returns></returns>
        public string ConsumerSecret
        {
            get
            {
                return _consumerSecret;
            }
        }
    }

    public class OAuth2LeggedAuthenticator : OAuthAuthenticator
    {
        private string _oAuthUser;
        private string _oAuthDomain;
        private OAuthParameters _parameters;

        public static string OAuthParameter = "xoauth_requestor_id";

        /// <summary>
        /// a constructor for 2-legged OAuth
        /// </summary>
        /// <param name="applicationName">The name of the application</param>
        /// <param name="consumerKey">the consumerKey to use</param>
        /// <param name="consumerSecret">the consumerSecret to use</param>
        /// <param name="user">the username to use</param>
        /// <param name="domain">the domain to use</param>
        /// <returns></returns>
        public OAuth2LeggedAuthenticator(
            string applicationName,
            string consumerKey,
            string consumerSecret,
            string user,
            string domain,
            string signatureMethod)
            : base(applicationName, consumerKey, consumerSecret)
        {
            _oAuthUser = user;
            _oAuthDomain = domain;
            _parameters = new OAuthParameters() { ConsumerKey = consumerKey, ConsumerSecret = consumerSecret, SignatureMethod = signatureMethod };
        }

        /// <summary>
        /// returns the OAuth User
        /// </summary>
        /// <returns></returns>
        public string OAuthUser
        {
            get
            {
                return _oAuthUser;
            }
        }

        /// <summary>
        /// returns the OAuth Domain
        /// </summary>
        /// <returns></returns>
        public string OAuthDomain
        {
            get
            {
                return _oAuthDomain;
            }
        }

        /// <summary>
        /// Takes an existing httpwebrequest and modifies its headers according to
        /// the authentication system used.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override void ApplyAuthenticationToRequest(HttpWebRequest request)
        {
            base.ApplyAuthenticationToRequest(request);

            string oauthHeader = OAuthUtil.GenerateHeader(
                request.RequestUri,
                request.Method,
                _parameters);
            request.Headers.Add(oauthHeader);
        }

        /// <summary>
        /// Takes an existing httpwebrequest and modifies its uri according to
        /// the authentication system used. Only overridden in 2-legged OAuth case
        /// Here we need to add the xoauth_requestor_id parameter
        /// </summary>
        /// <param name="source">the original uri</param>
        /// <returns></returns>
        public override Uri ApplyAuthenticationToUri(Uri source)
        {
            UriBuilder builder = new UriBuilder(source);
            string queryToAppend = OAuthParameter + "=" + _oAuthUser + "%40" + OAuthDomain;

            if (builder.Query != null && builder.Query.Length > 1)
            {
                builder.Query = builder.Query.Substring(1) + "&" + queryToAppend;
            }
            else
            {
                builder.Query = queryToAppend;
            }

            return builder.Uri;
        }
    }

    public class OAuth3LeggedAuthenticator : OAuthAuthenticator
    {
        // private string token;
        // private string tokenSecret;
        private OAuthParameters _parameters;

        /// <summary>
        /// a constructor for 3-legged OAuth
        /// </summary>
        /// <param name="applicationName">The name of the application</param>
        /// <param name="consumerKey">the consumerKey to use</param>
        /// <param name="consumerSecret">the consumerSecret to use</param>
        /// <param name="token">The token to be used</param>
        /// <param name="tokenSecret">The tokenSecret to be used</param>
        /// <returns></returns>
        public OAuth3LeggedAuthenticator(
            string applicationName,
            string consumerKey,
            string consumerSecret,
            string token,
            string tokenSecret,
            string scope,
            string signatureMethod)
            : base(applicationName, consumerKey, consumerSecret)
        {
            _parameters = new OAuthParameters() { ConsumerKey = consumerKey, ConsumerSecret = consumerSecret, Token = token, TokenSecret = tokenSecret, Scope = scope, SignatureMethod = signatureMethod };
        }

        /// <summary>
        /// returns the Token for oAuth
        /// </summary>
        /// <returns></returns>
        public string Token
        {
            get
            {
                return _parameters.Token;
            }
        }

        /// <summary>
        /// returns the TokenSecret for oAuth
        /// </summary>
        /// <returns></returns>
        public string TokenSecret
        {
            get
            {
                return _parameters.TokenSecret;
            }
        }

        /// <summary>
        /// Takes an existing httpwebrequest and modifies its headers according to
        /// the authentication system used.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override void ApplyAuthenticationToRequest(HttpWebRequest request)
        {
            base.ApplyAuthenticationToRequest(request);

            string oauthHeader = OAuthUtil.GenerateHeader(
                request.RequestUri,
                request.Method,
                _parameters);
            request.Headers.Add(oauthHeader);
        }
    }

    public class OAuth2Authenticator : Authenticator
    {
        private OAuth2Parameters _parameters;

        /// <summary>
        /// a constructor for OAuth 2.0
        /// </summary>
        /// <param name="applicationName">The name of the application</param>
        /// <param name="consumerKey">the consumerKey to use</param>
        /// <param name="consumerSecret">the consumerSecret to use</param>
        /// <param name="token">The token to be used</param>
        /// <param name="tokenSecret">The tokenSecret to be used</param>
        /// <returns></returns>
        public OAuth2Authenticator(string applicationName, OAuth2Parameters parameters)
            : base(applicationName)
        {
            _parameters = parameters;
        }

        /// <summary>
        /// Takes an existing httpwebrequest and modifies its headers according to
        /// the authentication system used.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override void ApplyAuthenticationToRequest(HttpWebRequest request)
        {
            base.ApplyAuthenticationToRequest(request);

            if (!string.IsNullOrEmpty(_parameters.AccessCode) && string.IsNullOrEmpty(_parameters.AccessToken))
            {
                OAuthUtil.GetAccessToken(_parameters);
            }

            request.Headers.Set("Authorization", String.Format(
                "{0} {1}", _parameters.TokenType, _parameters.AccessToken));
        }
    }
}
