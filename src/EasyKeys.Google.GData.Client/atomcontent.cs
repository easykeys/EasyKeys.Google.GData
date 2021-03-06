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
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Xml;

using EasyKeys.Google.GData.Extensions;

#endregion

// Contains AtomContent, an object to represent the
// atom:content element.
// # atom:content
//
// atomInlineTextContent =
//    element atom:content {
//       atomCommonAttributes,
//       attribute type { "TEXT" | "HTML" | atomMediaType }?,
//       (text)*
//    }
//
// atomInlineXHTMLContent =
//    element atom:content {
//       atomCommonAttributes,
//       attribute type { "XHTML" | atomMediaType }?,
//       (text|anyElement)*
//    }
//
// atomOutOfLineContent =
//    element atom:content {
//       atomCommonAttributes,
//       attribute type { "TEXT" | "HTML" | "XHTML" | atomMediaType }?,
//       attribute src { atomUri },
//       empty
//    }
//
// atomContent = atomInlineTextContent
//  | atomInlineXHTMLContent
//  | atomOutOfLineContent
namespace EasyKeys.Google.GData.Client
{
    /// <summary>TypeConverter, so that AtomContentConverter shows up in the property pages
    /// </summary>
    [ComVisible(false)]
    public class AtomContentConverter : ExpandableObjectConverter
    {
        ///<summary>Standard type converter method</summary>
        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            if (destinationType == typeof(AtomContent))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        ///<summary>Standard type converter method</summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
        {
            AtomContent content = value as AtomContent;
            if (destinationType == typeof(System.String) && content != null)
            {
                return "Content-type: " + content.Type;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    /// <summary>atom:content object representation
    /// </summary>
    [TypeConverterAttribute(typeof(AtomContentConverter)), DescriptionAttribute("Expand to see the content objectfor the entry.")]
    public class AtomContent : AtomBase
    {
        /// <summary>holds the type attribute</summary>
        private string _type;

        /// <summary>holds the src URI attribute</summary>
        private AtomUri _src;

        /// <summary>holds the content</summary>
        private string _content;

        /// <summary>default constructor. Sets the content type to text.</summary>
        public AtomContent()
        {
            _type = "text";
            AddExtension(new BatchErrors());
        }

        #region overloaded for persistence

        /// <summary>Returns the constant representing this XML element.</summary>
        public override string XmlName
        {
            get { return AtomParserNameTable.XmlContentElement; }
        }

        /// <summary>figures out if this object should be persisted</summary>
        /// <returns> true, if it's worth saving</returns>
        public override bool ShouldBePersisted()
        {
            if (!base.ShouldBePersisted())
            {
                return Utilities.IsPersistable(_src) || Utilities.IsPersistable(_type) || Utilities.IsPersistable(_content);
            }

            return true;
        }

        /// <summary>overridden to save attributes for this(XmlWriter writer)</summary>
        /// <param name="writer">the xmlwriter to save into</param>
        protected override void SaveXmlAttributes(XmlWriter writer)
        {
            WriteEncodedAttributeString(writer, AtomParserNameTable.XmlAttributeSrc, Src);
            WriteEncodedAttributeString(writer, AtomParserNameTable.XmlAttributeType, Type);
            // call base later, as base will save out extension elements that might create subelements
            base.SaveXmlAttributes(writer);
        }

        /// <summary>saves the inner state of the element. Note that if the
        /// content type is xhtml, no encoding will be done by this object</summary>
        /// <param name="writer">the xmlWriter to save into</param>
        protected override void SaveInnerXml(XmlWriter writer)
        {
            base.SaveInnerXml(writer);
            if (Utilities.IsPersistable(_content))
            {
                if (_type == "html" || _type.StartsWith("text"))
                {
                    // per spec, text/html should be encoded.
                    // but what do we do if we get it encoded? we would now double encode
                    // the string
                    // hence we decode once first, and then encode again
                    String buffer = HttpUtility.HtmlDecode(_content);
                    WriteEncodedString(writer, buffer);
                }
                else
                {
                    // in this case we are not going to encode the inner content.
                    // Developer has to take care of this
                    writer.WriteRaw(_content);
                }
            }
        }

        #endregion

        /// <summary>accessor method public string Type</summary>
        /// <returns> </returns>
        public string Type
        {
            get { return _type; }
            set { Dirty = true; _type = value; }
        }

        /// <summary>accessor method public Uri Src</summary>
        /// <returns> </returns>
        public AtomUri Src
        {
            get { return _src; }
            set { Dirty = true; _src = value; }
        }

        /// <summary>public Uri AbsoluteUri</summary>
        public string AbsoluteUri
        {
            get
            {
                return GetAbsoluteUri(Src.ToString());
            }
        }

        /// <summary>accessor method public string Content</summary>
        /// <returns> </returns>
        public string Content
        {
            get { return _content; }
            set { Dirty = true; _content = value; }
        }

        /// <summary>
        /// gd:errors element
        /// </summary>
        /// <returns></returns>
        public BatchErrors BatchErrors
        {
            get
            {
                return FindExtension(
                    BaseNameTable.gdErrors,
                    BaseNameTable.gNamespace) as BatchErrors;
            }

            set
            {
                ReplaceExtension(
                    BaseNameTable.gdErrors,
                    BaseNameTable.gNamespace,
                    value);
            }
        }
    }
}

