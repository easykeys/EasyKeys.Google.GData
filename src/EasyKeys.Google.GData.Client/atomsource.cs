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

#endregion

// <summary>Contains AtomSource, an object to represent the atom:source
// element.</summary>
namespace EasyKeys.Google.GData.Client
{
    /// <summary>TypeConverter, so that AtomHead shows up in the property pages
    /// </summary>
    [ComVisible(false)]
    public class AtomSourceConverter : ExpandableObjectConverter
    {
        ///<summary>Standard type converter method</summary>
        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            if (destinationType == typeof(AtomSource) || destinationType == typeof(AtomFeed))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        ///<summary>Standard type converter method</summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
        {
            AtomSource atomSource = value as AtomSource;

            if (destinationType == typeof(System.String) && atomSource != null)
            {
                return "Feed: " + atomSource.Title;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    /// <summary>Represents the AtomSource object. If an atom:entry is copied from one feed
    /// into another feed, then the source atom:feed's metadata (all child elements of atom:feed other
    /// than the atom:entry elements) MAY be preserved within the copied entry by adding an atom:source
    /// child element, if it is not already present in the entry, and including some or all of the source
    /// feed's Metadata elements as the atom:source element's children. Such metadata SHOULD be preserved
    /// if the source atom:feed contains any of the child elements atom:author, atom:contributor,
    /// atom:rights, or atom:category and those child elements are not present in the source atom:entry.
    /// </summary>
    /*
    atomSource =
        element atom:source {
           atomCommonAttributes,
           (atomAuthor?
            & atomCategory*
            & atomContributor*
            & atomGenerator?
            & atomIcon?
            & atomId?
            & atomLink*
            & atomLogo?
            & atomRights?
            & atomSubtitle?
            & atomTitle?
            & atomUpdated?
            & extensionElement*)
        }
    */

    [TypeConverterAttribute(typeof(AtomSourceConverter)), DescriptionAttribute("Expand to see the options for the feed")]
    public class AtomSource : AtomBase
    {
        /// <summary>author collection</summary>
        private AtomPersonCollection _authors;

        /// <summary>contributors collection</summary>
        private AtomPersonCollection _contributors;

        /// <summary>category collection</summary>
        private AtomCategoryCollection _categories;

        /// <summary>the generator</summary>
        private AtomGenerator _generator;

        /// <summary>icon, essentially an atom link</summary>
        private AtomIcon _icon;

        /// <summary>ID</summary>
        private AtomId _id;

        /// <summary>link collection</summary>
        private AtomLinkCollection _links;

        /// <summary>logo, essentially an image link</summary>
        private AtomLogo _logo;

        /// <summary>rights, former copyrights</summary>
        private AtomTextConstruct _rights;

        /// <summary>subtitle as string</summary>
        private AtomTextConstruct _subTitle;

        /// <summary>title property as string</summary>
        private AtomTextConstruct _title;

        /// <summary>updated time stamp</summary>
        private DateTime _updated;

        /// <summary>public void AtomSource()</summary>
        public AtomSource()
        {
        }

        /// <summary>public AtomSource(AtomFeed feed)</summary>
        public AtomSource(AtomFeed feed)
            : this()
        {
            Tracing.Assert(feed != null, "feed should not be null");
            if (feed == null)
            {
                throw new ArgumentNullException("feed");
            }

            // now copy them
            _authors = feed.Authors;
            _contributors = feed.Contributors;
            _categories = feed.Categories;
            Generator = feed.Generator;
            Icon = feed.Icon;
            Logo = feed.Logo;
            Id = feed.Id;
            _links = feed.Links;
            Rights = feed.Rights;
            Subtitle = feed.Subtitle;
            Title = feed.Title;
            Updated = feed.Updated;
        }

        /// <summary>accessor method public Authors AtomPersonCollection</summary>
        /// <returns> </returns>
        public AtomPersonCollection Authors
        {
            get
            {
                if (_authors == null)
                {
                    _authors = new AtomPersonCollection();
                }

                return _authors;
            }
        }

        /// <summary>accessor method public Contributors AtomPersonCollection</summary>
        /// <returns> </returns>
        public AtomPersonCollection Contributors
        {
            get
            {
                if (_contributors == null)
                {
                    _contributors = new AtomPersonCollection();
                }

                return _contributors;
            }
        }

        /// <summary>accessor method public Links AtomLinkCollection</summary>
        /// <returns> </returns>
        public AtomLinkCollection Links
        {
            get
            {
                if (_links == null)
                {
                    _links = new AtomLinkCollection();
                }

                return _links;
            }
        }

        /// <summary>returns the category collection</summary>
        public AtomCategoryCollection Categories
        {
            get
            {
                if (_categories == null)
                {
                    _categories = new AtomCategoryCollection();
                }

                return _categories;
            }
        }

        /// <summary>accessor method public FeedGenerator Generator</summary>
        /// <returns> </returns>
        public AtomGenerator Generator
        {
            get { return _generator; }
            set { Dirty = true; _generator = value; }
        }

        /// <summary>accessor method public AtomIcon Icon</summary>
        /// <returns> </returns>
        public AtomIcon Icon
        {
            get { return _icon; }
            set { Dirty = true; _icon = value; }
        }

        /// <summary>accessor method public AtomLogo Logo</summary>
        /// <returns> </returns>
        public AtomLogo Logo
        {
            get { return _logo; }
            set { Dirty = true; _logo = value; }
        }

        /// <summary>accessor method public DateTime LastUpdated</summary>
        /// <returns> </returns>
        public DateTime Updated
        {
            get { return _updated; }
            set { Dirty = true; _updated = value; }
        }

        /// <summary>accessor method public string Title</summary>
        /// <returns> </returns>
        public AtomTextConstruct Title
        {
            get { return _title; }
            set { Dirty = true; _title = value; }
        }

        /// <summary>accessor method public string Subtitle</summary>
        /// <returns> </returns>
        public AtomTextConstruct Subtitle
        {
            get { return _subTitle; }
            set { Dirty = true; _subTitle = value; }
        }

        /// <summary>accessor method public string Id</summary>
        /// <returns> </returns>
        public AtomId Id
        {
            get { return _id; }
            set { Dirty = true; _id = value; }
        }

        /// <summary>accessor method public string Rights</summary>
        /// <returns> </returns>
        public AtomTextConstruct Rights
        {
            get { return _rights; }
            set { Dirty = true; _rights = value; }
        }

        #region Persistence overloads

        /// <summary>Returns the constant representing this XML element.</summary>
        public override string XmlName
        {
            get { return AtomParserNameTable.XmlSourceElement; }
        }

        /// <summary>saves the inner state of the element</summary>
        /// <param name="writer">the xmlWriter to save into </param>
        protected override void SaveInnerXml(XmlWriter writer)
        {
            base.SaveInnerXml(writer);
            // saving Authors
            foreach (AtomPerson person in Authors)
            {
                person.SaveToXml(writer);
            }

            // saving Contributors
            foreach (AtomPerson person in Contributors)
            {
                person.SaveToXml(writer);
            }

            // saving Categories
            foreach (AtomCategory category in Categories)
            {
                category.SaveToXml(writer);
            }

            // saving the generator
            if (Generator != null)
            {
                Generator.SaveToXml(writer);
            }

            // save the icon
            if (Icon != null)
            {
                Icon.SaveToXml(writer);
            }

            // save the logo
            if (Logo != null)
            {
                Logo.SaveToXml(writer);
            }

            // save the ID
            if (Id != null)
            {
                Id.SaveToXml(writer);
            }

            // save the Links
            foreach (AtomLink link in Links)
            {
                link.SaveToXml(writer);
            }

            if (Rights != null)
            {
                Rights.SaveToXml(writer);
            }

            if (Subtitle != null)
            {
                Subtitle.SaveToXml(writer);
            }

            if (Title != null)
            {
                Title.SaveToXml(writer);
            }

            // date time construct, save here.
            WriteLocalDateTimeElement(writer, AtomParserNameTable.XmlUpdatedElement, Updated);
        }

        #endregion

        #region overloaded for property changes, xml:base

        /// <summary>just go down the child collections</summary>
        /// <param name="uriBase"> as currently calculated</param>
        internal override void BaseUriChanged(AtomUri uriBase)
        {
            base.BaseUriChanged(uriBase);

            foreach (AtomPerson person in Authors)
            {
                person.BaseUriChanged(uriBase);
            }

            // saving Contributors
            foreach (AtomPerson person in Contributors)
            {
                person.BaseUriChanged(uriBase);
            }

            // saving Categories
            foreach (AtomCategory category in Categories)
            {
                category.BaseUriChanged(uriBase);
            }

            // saving the generator
            if (Generator != null)
            {
                Generator.BaseUriChanged(uriBase);
            }

            // save the icon
            if (Icon != null)
            {
                Icon.BaseUriChanged(uriBase);
            }

            // save the logo
            if (Logo != null)
            {
                Logo.BaseUriChanged(uriBase);
            }

            // save the ID
            if (Id != null)
            {
                Id.BaseUriChanged(uriBase);
            }

            // save the Links
            foreach (AtomLink link in Links)
            {
                link.BaseUriChanged(uriBase);
            }

            if (Rights != null)
            {
                Rights.BaseUriChanged(uriBase);
            }

            if (Subtitle != null)
            {
                Subtitle.BaseUriChanged(uriBase);
            }

            if (Title != null)
            {
                Title.BaseUriChanged(uriBase);
            }
        }

        /// <summary>calls the action on this object and all children</summary>
        /// <param name="action">an IAtomBaseAction interface to call </param>
        /// <returns>true or false, pending outcome</returns>
        public override bool WalkTree(IBaseWalkerAction action)
        {
            if (base.WalkTree(action))
            {
                return true;
            }

            foreach (AtomPerson person in Authors)
            {
                if (person.WalkTree(action))
                {
                    return true;
                }
            }

            // saving Contributors
            foreach (AtomPerson person in Contributors)
            {
                if (person.WalkTree(action))
                {
                    return true;
                }
            }

            // saving Categories
            foreach (AtomCategory category in Categories)
            {
                if (category.WalkTree(action))
                {
                    return true;
                }
            }

            // saving the generator
            if (Generator != null)
            {
                if (Generator.WalkTree(action))
                {
                    return true;
                }
            }

            // save the icon
            if (Icon != null)
            {
                if (Icon.WalkTree(action))
                {
                    return true;
                }
            }

            // save the logo
            if (Logo != null)
            {
                if (Logo.WalkTree(action))
                {
                    return true;
                }
            }

            // save the ID
            if (Id != null)
            {
                if (Id.WalkTree(action))
                {
                    return true;
                }
            }

            // save the Links
            foreach (AtomLink link in Links)
            {
                if (link.WalkTree(action))
                {
                    return true;
                }
            }

            if (Rights != null)
            {
                if (Rights.WalkTree(action))
                {
                    return true;
                }
            }

            if (Subtitle != null)
            {
                if (Subtitle.WalkTree(action))
                {
                    return true;
                }
            }

            if (Title != null)
            {
                if (Title.WalkTree(action))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}

