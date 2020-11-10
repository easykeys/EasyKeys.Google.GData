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
#region Using directives

#define USE_TRACING

using System;

#endregion

//////////////////////////////////////////////////////////////////////
// Contains AtomUri.
//////////////////////////////////////////////////////////////////////
namespace EasyKeys.Google.GData.Client
{
    //////////////////////////////////////////////////////////////////////

    /// <summary>AtomUri object representation
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    public class AtomUri : IComparable
    {
        string _strContent;

        /// <summary>basic constructor for the atomUri</summary>
        public AtomUri(Uri uri)
        {
            Tracing.Assert(uri != null, "uri should not be null");
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            _strContent = HttpUtility.UrlDecode(uri.ToString());
        }

        /// <summary>alternating constructor with a string</summary>
        public AtomUri(string str)
        {
            _strContent = str;
        }

        //////////////////////////////////////////////////////////////////////

        /// <summary>accessor method public string Content</summary>
        /// <returns> </returns>
        //////////////////////////////////////////////////////////////////////
        public string Content
        {
            get { return _strContent; }
            set { _strContent = value; }
        }
        /////////////////////////////////////////////////////////////////////////////

        /// <summary>override for ToString</summary>
        public override string ToString()
        {
            return _strContent;
        }

        /// <summary>comparison method similar to strings</summary>
        public static int Compare(AtomUri oneAtomUri, AtomUri anotherAtomUri)
        {
            if (oneAtomUri != null)
            {
                return oneAtomUri.CompareTo(anotherAtomUri);
            }
            else if (anotherAtomUri == null)
            {
                return 0;
            }

            return -1;
        }

        #region IComparable Members

        /// <summary>
        /// as we do comparisons, we need to override this
        /// we return the hashcode of our string member
        /// </summary>
        /// <returns>int</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// overloaded IComparable interface method
        /// </summary>
        /// <param name="obj">the object to compare this instance with</param>
        /// <returns>int</returns>
		public int CompareTo(object obj)
        {
            if (obj == null)
                return -1;

            if (!(obj is AtomUri))
                return -1;

            return String.Compare(ToString(), obj.ToString());
        }

        #endregion

        /// <summary>
        /// overridden equal method
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>bool</returns>
        public override bool Equals(object obj)
        {
            return CompareTo(obj) == 0;
        }

        /// <summary>
        /// overridden comparson operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>bool</returns>
		public static bool operator ==(AtomUri a, AtomUri b)
        {
            if ((object)a == null && (object)b == null) return true;
            if ((object)a != null && (object)b != null)
            {
                return a.Equals(b);
            }

            return false;
        }

        /// <summary>
        /// overridden comparson operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>bool</returns>
        public static bool operator !=(AtomUri a, AtomUri b)
        {
            return !(a == b);
        }

        /// <summary>
        /// overridden comparson operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>bool</returns>
		public static bool operator >(AtomUri a, AtomUri b)
        {
            if (a != null)
            {
                return a.CompareTo(b) > 0;
            }

            return false;
        }

        /// <summary>
        /// overridden comparson operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>bool</returns>
		public static bool operator <(AtomUri a, AtomUri b)
        {
            if (a != null)
            {
                return a.CompareTo(b) < 0;
            }

            return true;
        }

        /// <summary>
        /// overridden comparson operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>bool</returns>
		public static bool operator >=(AtomUri a, AtomUri b)
        {
            if (a != null)
            {
                return a.CompareTo(b) > 0 || a.Equals(b);
            }
            else if (b == null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// overridden comparson operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>bool</returns>
		public static bool operator <=(AtomUri a, AtomUri b)
        {
            if (a != null)
            {
                return a.CompareTo(b) < 0 || a.Equals(b);
            }

            return true;
        }

        /// <summary>
        /// implicit new instance of AtomUri from string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static implicit operator AtomUri(string s)
        {
            return new AtomUri(s);
        }

        /// <summary>
        /// implicit new instance of AtomUri from Uri object
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public static implicit operator AtomUri(Uri u)
        {
            return new AtomUri(u);
        }
    }
    /////////////////////////////////////////////////////////////////////////////
}
/////////////////////////////////////////////////////////////////////////////
