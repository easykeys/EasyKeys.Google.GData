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
 * 
 * Author: Andrew Smith <andy@snae.net> 22/11/08
*/

using System;
using System.Net;

namespace EasyKeys.Google.GData.Client
{
    /// <summary>
    /// A request factory to generate an authorization header suitable for use
    /// with OAuth.
    /// </summary>
    public class GOAuthRequestFactory : GDataGAuthRequestFactory
    {
        /// <summary>this factory's agent</summary> 
        public const string GDataGAuthSubAgent = "GOAuthRequestFactory-CS/1.0.0";

        private string _tokenSecret;
        private string _token;
        private string _consumerSecret;
        private string _consumerKey;

        /// <summary>
        /// default constructor.
        /// </summary>
        public GOAuthRequestFactory(string service, string applicationName)
            : base(service, applicationName)
        {
        }

        /// <summary>
        /// overloaded constructor that sets parameters from an OAuthParameter instance.
        /// </summary>
        public GOAuthRequestFactory(string service, string applicationName, OAuthParameters parameters)
            : base(service, applicationName)
        {
            if (parameters.ConsumerKey != null)
            {
                ConsumerKey = parameters.ConsumerKey;
            }

            if (parameters.ConsumerSecret != null)
            {
                ConsumerSecret = parameters.ConsumerSecret;
            }

            if (parameters.Token != null)
            {
                Token = parameters.Token;
            }

            if (parameters.TokenSecret != null)
            {
                TokenSecret = parameters.TokenSecret;
            }
        }

        /// <summary>
        /// default constructor.
        /// </summary>
        public override IGDataRequest CreateRequest(GDataRequestType type, Uri uriTarget)
        {
            return new GOAuthRequest(type, uriTarget, this);
        }

        public string ConsumerSecret
        {
            get { return _consumerSecret; }
            set { _consumerSecret = value; }
        }

        public string ConsumerKey
        {
            get { return _consumerKey; }
            set { _consumerKey = value; }
        }

        public string TokenSecret
        {
            get { return _tokenSecret; }
            set { _tokenSecret = value; }
        }

        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }
    }

    /// <summary>
    /// GOAuthSubRequest implementation.
    /// </summary>
    public class GOAuthRequest : GDataGAuthRequest
    {
        /// <summary>holds the factory instance</summary> 
        private GOAuthRequestFactory _factory;

        /// <summary>
        /// default constructor.
        /// </summary>
        internal GOAuthRequest(GDataRequestType type, Uri uriTarget, GOAuthRequestFactory factory)
            : base(type, uriTarget, factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// sets up the correct credentials for this call.
        /// </summary>
        protected override void EnsureCredentials()
        {
            HttpWebRequest http = Request as HttpWebRequest;

            if (string.IsNullOrEmpty(_factory.ConsumerKey) || string.IsNullOrEmpty(_factory.ConsumerSecret))
            {
                throw new GDataRequestException("ConsumerKey and ConsumerSecret must be provided to use GOAuthRequestFactory");
            }

            string oauthHeader = OAuthUtil.GenerateHeader(
                http.RequestUri,
                _factory.ConsumerKey,
                _factory.ConsumerSecret,
                _factory.Token,
                _factory.TokenSecret,
                http.Method);
            Request.Headers.Remove("Authorization"); // needed?
            Request.Headers.Add(oauthHeader);
        }
    }
}
