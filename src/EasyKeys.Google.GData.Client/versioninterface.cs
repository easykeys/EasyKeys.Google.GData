/* Copyright (c) 2006-2008 Google Inc.
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
/* Change history
* Oct 13 2008  Joe Feser       joseph.feser@gmail.com
* Converted ArrayLists and other .NET 1.1 collections to use Generics
* Combined IExtensionElement and IExtensionElementFactory interfaces
*
*/
#region Using directives

#define USE_TRACING

using System;
using System.Globalization;

#endregion

//////////////////////////////////////////////////////////////////////
// <summary>contains Service, the base interface that
//   allows to query a service for different feeds
//  </summary>
//////////////////////////////////////////////////////////////////////
namespace EasyKeys.Google.GData.Client
{
    /// <summary>
    /// the default versions that are used. Currently, the default is still
    /// version 1 for most services implemented in this sdk.
    /// </summary>
    public static class VersionDefaults
    {
        /// <summary>
        /// version One is 1
        /// </summary>
        public const int VersionOne = 1;

        /// <summary>
        /// the default major is VersionOne
        /// </summary>
        public const int Major = VersionOne;

        /// <summary>
        /// the default Minor is 0
        /// </summary>
        public const int Minor = 0;

        /// <summary>
        /// and versionTwo is a 2
        /// </summary>
        public const int VersionTwo = 2;

        /// <summary>
        /// and versionThree is a 3
        /// </summary>
        public const int VersionThree = 3;
    }

    // TODO determine if this is the correct approach.

    /// <summary>
    /// Class used as a null version aware seed for the collections
    /// </summary>
    public class NullVersionAware : IVersionAware
    {
        private static object synclock = new object();
        private static IVersionAware _instance;

        /// <summary>
        /// IVersionAware instance property
        /// </summary>
        public static IVersionAware Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (synclock)
                    {
                        if (_instance == null)
                        {
                            _instance = new NullVersionAware();
                        }
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// returns the major version of the protocol used
        /// </summary>
        public int ProtocolMajor
        {
            get
            {
                return 0;
            }

            set
            {
                // throw new Exception("The method or operation is not implemented.");
            }
        }

        /// <summary>
        /// returns the minor version of the protocol used
        /// </summary>
        public int ProtocolMinor
        {
            get
            {
                return 0;
            }

            set
            {
                // throw new Exception("The method or operation is not implemented.");
            }
        }
    }

    /// <summary>
    /// this interface indicates that an element is aware of Core and Service
    /// specific version changes.
    /// </summary>
    /// <returns></returns>
    public interface IVersionAware
    {
        /// <summary>
        /// returns the major version of the protocol this element is using
        /// </summary>
        int ProtocolMajor
        {
            set;
            get;
        }

        /// <summary>
        /// returns the minor version of the protocol this element is using
        /// </summary>
        int ProtocolMinor
        {
            set;
            get;
        }
    }

    internal class VersionInformation : IVersionAware
    {
        private int _majorVersion = VersionDefaults.Major;
        private int _minorVersion = VersionDefaults.Minor;

        /// <summary>
        /// construct a versioninformation object based
        /// on a versionaware object
        /// </summary>
        /// <param name="v">the versioned object to copy the data from</param>
        /// <returns></returns>
        public VersionInformation(IVersionAware v)
        {
            _majorVersion = v.ProtocolMajor;
            _minorVersion = v.ProtocolMinor;
        }

        /// <summary>
        /// construct a versioninformation object based
        /// on the header string of the http request. The string
        /// has the form {major}.{minor}
        /// </summary>
        /// <param name="headerValue">if null creates default version</param>
        /// <returns></returns>
        public VersionInformation(string headerValue)
        {
            if (headerValue != null)
            {
                string[] arr = headerValue.Split('.');
                if (arr.Length == 2)
                {
                    _majorVersion = int.Parse(arr[0], CultureInfo.InvariantCulture);
                    _minorVersion = int.Parse(arr[1], CultureInfo.InvariantCulture);
                }
            }
        }

        public VersionInformation()
        {
        }

        /// <summary>
        /// returns the major protocol version number this element
        /// is working against.
        /// </summary>
        /// <returns></returns>
        public int ProtocolMajor
        {
            get
            {
                return _majorVersion;
            }

            set
            {
                _majorVersion = value;
            }
        }

        /// <summary>
        /// returns the minor protocol version number this element
        /// is working against.
        /// </summary>
        /// <returns></returns>
        public int ProtocolMinor
        {
            get
            {
                return _minorVersion;
            }

            set
            {
                _minorVersion = value;
            }
        }

        /// <summary>
        /// takes an object and set's the version number to the
        /// same as this instance
        /// </summary>
        /// <param name="v"></param>
        public void ImprintVersion(IVersionAware v)
        {
            v.ProtocolMajor = _majorVersion;
            v.ProtocolMinor = _minorVersion;
        }

        /// <summary>
        /// takes an object and set's the version number to the
        /// same as this instance
        /// </summary>
        /// <param name="arr">The array of objects the version should be applied to</param>
        public void ImprintVersion(ExtensionList arr)
        {
            if (arr == null)
                return;
            foreach (Object o in arr)
            {
                IVersionAware v = o as IVersionAware;
                if (v != null)
                {
                    ImprintVersion(v);
                }
            }
        }
    }
}
/////////////////////////////////////////////////////////////////////////////
