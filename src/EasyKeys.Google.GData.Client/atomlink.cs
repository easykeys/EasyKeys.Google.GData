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

using System.Globalization;
using System.Xml;

#endregion

//////////////////////////////////////////////////////////////////////
// <summary>Contains AtomLink, an object to represent the atom:link
// element.</summary>
//////////////////////////////////////////////////////////////////////
namespace EasyKeys.Google.GData.Client
{
    //////////////////////////////////////////////////////////////////////

    /// <summary>AtomLink represents an atom:link element
    /// atomLink = element atom:link {
    ///    atomCommonAttributes,
    ///    attribute href { atomUri },
    ///    attribute rel { atomNCName | atomUri }?,
    ///    attribute type { atomMediaType }?,
    ///    attribute hreflang { atomLanguageTag }?,
    ///    attribute title { text }?,
    ///    attribute length { text }?,
    ///    empty
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    public class AtomLink : AtomBase
    {
        /// <summary>property holder exposed over get/set</summary>
        private AtomUri _href;

        /// <summary>property holder exposed over get/set</summary>
        private string _rel;

        /// <summary>property holder exposed over get/set</summary>
        private string _type;

        /// <summary>property holder exposed over get/set</summary>
        private string _hreflang;

        /// <summary>property holder exposed over get/set</summary>
        private string _title;

        /// <summary>property holder exposed over get/set</summary>
        private int _length;

        /// <summary>HTML Link Type</summary>
        public const string HTML_TYPE = "text/html";

        /// <summary>ATOM Link Type</summary>
        public const string ATOM_TYPE = "application/atom+xml";

        //////////////////////////////////////////////////////////////////////

        /// <summary>default empty constructor</summary>
        //////////////////////////////////////////////////////////////////////
        public AtomLink()
        {
        }
        /////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////

        /// <summary>public AtomLink(string uri)</summary>
        /// <param name="link">the uri for the link </param>
        //////////////////////////////////////////////////////////////////////
        public AtomLink(string link)
        {
            HRef = new AtomUri(link);
        }

        /// <summary>
        /// constructor used in atomfeed to create new links
        /// </summary>
        /// <param name="type">the type of link to create</param>
        /// <param name="rel">the rel value</param>
        public AtomLink(string type, string rel)
        {
            Type = type;
            Rel = rel;
        }

        //////////////////////////////////////////////////////////////////////

        /// <summary>accessor method public Uri HRef</summary>
        /// <returns> </returns>
        //////////////////////////////////////////////////////////////////////
        public AtomUri HRef
        {
            get { return _href; }
            set { Dirty = true; _href = value; }
        }
        /////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////

        /// <summary>public string AbsoluteUri</summary>
        //////////////////////////////////////////////////////////////////////
        public string AbsoluteUri
        {
            get
            {
                if (HRef != null)
                    return GetAbsoluteUri(HRef.ToString());
                return null;
            }
        }
        /////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////

        /// <summary>accessor method public string Rel</summary>
        /// <returns> </returns>
        //////////////////////////////////////////////////////////////////////
        public string Rel
        {
            get { return _rel; }
            set { Dirty = true; _rel = value; }
        }
        /////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////

        /// <summary>accessor method public string Type</summary>
        /// <returns> </returns>
        //////////////////////////////////////////////////////////////////////
        public string Type
        {
            get { return _type; }
            set { Dirty = true; _type = value; }
        }
        /////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////

        /// <summary>accessor method public string HrefLang</summary>
        /// <returns> </returns>
        //////////////////////////////////////////////////////////////////////
        public string HRefLang
        {
            get { return _hreflang; }
            set { Dirty = true; _hreflang = value; }
        }
        /////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////

        /// <summary>accessor method public int Lenght</summary>
        /// <returns> </returns>
        //////////////////////////////////////////////////////////////////////
        public int Length
        {
            get { return _length; }
            set { Dirty = true; _length = value; }
        }
        /////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////

        /// <summary>accessor method public string Title</summary>
        /// <returns> </returns>
        //////////////////////////////////////////////////////////////////////
        public string Title
        {
            get { return _title; }
            set { Dirty = true; _title = value; }
        }
        /////////////////////////////////////////////////////////////////////////////

        #region Persistence overloads
        //////////////////////////////////////////////////////////////////////

        /// <summary>Returns the constant representing this XML element.</summary>
        //////////////////////////////////////////////////////////////////////
        public override string XmlName
        {
            get { return AtomParserNameTable.XmlLinkElement; }
        }
        /////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////

        /// <summary>overridden to save attributes for this(XmlWriter writer)</summary>
        /// <param name="writer">the xmlwriter to save into </param>
        //////////////////////////////////////////////////////////////////////
        protected override void SaveXmlAttributes(XmlWriter writer)
        {
            WriteEncodedAttributeString(writer, AtomParserNameTable.XmlAttributeHRef, HRef);
            WriteEncodedAttributeString(writer, AtomParserNameTable.XmlAttributeHRefLang, HRefLang);
            WriteEncodedAttributeString(writer, AtomParserNameTable.XmlAttributeRel, Rel);
            WriteEncodedAttributeString(writer, AtomParserNameTable.XmlAttributeType, Type);

            if (_length > 0)
            {
                WriteEncodedAttributeString(writer, AtomParserNameTable.XmlAttributeLength, Length.ToString(CultureInfo.InvariantCulture));
            }

            WriteEncodedAttributeString(writer, AtomParserNameTable.XmlTitleElement, Title);
            // call base later as base takes care of writing out extension elements that might close the attribute list
            base.SaveXmlAttributes(writer);
        }
        /////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////

        /// <summary>figures out if this object should be persisted</summary>
        /// <returns> true, if it's worth saving</returns>
        //////////////////////////////////////////////////////////////////////
        public override bool ShouldBePersisted()
        {
            if (base.ShouldBePersisted())
            {
                return true;
            }

            if (Utilities.IsPersistable(_href))
            {
                return true;
            }

            if (Utilities.IsPersistable(_hreflang))
            {
                return true;
            }

            if (Utilities.IsPersistable(_rel))
            {
                return true;
            }

            if (Utilities.IsPersistable(_type))
            {
                return true;
            }

            if (Utilities.IsPersistable(Length))
            {
                return true;
            }

            if (Utilities.IsPersistable(_title))
            {
                return true;
            }

            return false;
        }
        /////////////////////////////////////////////////////////////////////////////

        #endregion

    }
    /////////////////////////////////////////////////////////////////////////////
} /////////////////////////////////////////////////////////////////////////////

