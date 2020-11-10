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
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Xml;

using EasyKeys.Google.GData.Extensions.AppControl;

#endregion

// <summary>Contains AtomEntry, an object to represent the atom:entry
// element.</summary>
namespace EasyKeys.Google.GData.Client
{
    /// <summary>TypeConverter, so that AtomEntry shows up in the property pages
    /// </summary>
    [ComVisible(false)]
    public class AtomEntryConverter : ExpandableObjectConverter
    {
        ///<summary>Standard type converter method</summary>
        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            if (destinationType == typeof(AtomEntry))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        ///<summary>Standard type converter method</summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
        {
            AtomEntry entry = value as AtomEntry;
            if (destinationType == typeof(System.String) && entry != null)
            {
                return "Entry: " + entry.Title;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    /// <summary>AtomEntry object, representing an item in the RSS/Atom feed
    ///  Version 1.0 removed atom-Head
    ///    element atom:entry {
    ///       atomCommonAttributes,
    ///       (atomAuthor*
    ///         atomCategory*
    ///        atomContent?
    ///        atomContributor*
    ///        atomId
    ///        atomLink*
    ///        atomPublished?
    ///        atomRights?
    ///        atomSource?
    ///        atomSummary?
    ///        atomTitle
    ///        atomUpdated
    ///        extensionElement*)
    ///    }
    ///  </summary>
    [TypeConverterAttribute(typeof(AtomEntryConverter)), DescriptionAttribute("Expand to see the entry objects for the feed.")]
    public class AtomEntry : AtomBase
    {
        #region standard entry properties as returned by query

        /// <summary>/feed/entry/title property as string</summary>
        private AtomTextConstruct _title;

        /// <summary>/feed/entry/id property as string</summary>
        private AtomId _id;

        /// <summary>/feed/entry/link collection</summary>
        private AtomLinkCollection _links;

        /// <summary>/feed/entry/updated property as string</summary>
        private DateTime _lastUpdateDate;

        /// <summary>/feed/entry/published property as string</summary>
        private DateTime _publicationDate;

        /// <summary>/feed/entry/author property as Author object</summary>
        private AtomPersonCollection _authors;

        /// <summary>/feed/entry/atomContributor property as Author object</summary>
        private AtomPersonCollection _contributors;

        /// <summary>The "atom:rights" element is a Text construct that conveys a human-readable copyright statement for an entry or feed.</summary>
        private AtomTextConstruct _rights;

        /// <summary>/feed/entry/category/@term property as a list of AtomCategories</summary>
        private AtomCategoryCollection _categories;

        /// <summary>The "atom:summary" element is a Text construct that conveys a short summary, abstract or excerpt of an entry.</summary>
        private AtomTextConstruct _summary;

        /// <summary>contains the content as an object</summary>
        private AtomContent _content;

        /// <summary>atom:source element</summary>
        private AtomSource _source;

        /// <summary>GData service to use</summary>
        private IService _service;

        /// <summary>holds the owning feed</summary>
        private AtomFeed _feed;
        // holds batch information for an entry
        private GDataBatchEntryData _batchData;

        #endregion

        #region Persistence overloads

        /// <summary>Returns the constant representing this XML element.</summary>
        public override string XmlName
        {
            get { return AtomParserNameTable.XmlAtomEntryElement; }
        }

        /// <summary>checks to see if we are a batch feed, if so, adds the batchNS</summary>
        /// <param name="writer">the xmlwriter, where we want to add default namespaces to</param>
        protected override void AddOtherNamespaces(XmlWriter writer)
        {
            base.AddOtherNamespaces(writer);
            if (BatchData != null)
            {
                Utilities.EnsureGDataBatchNamespace(writer);
            }
        }

        /// <summary>checks if this is a namespace
        /// declaration that we already added</summary>
        /// <param name="node">XmlNode to check</param>
        /// <returns>true if this node should be skipped </returns>
        protected override bool SkipNode(XmlNode node)
        {
            if (base.SkipNode(node))
            {
                return true;
            }

            Tracing.TraceMsg("in skipnode for node: " + node.Name + "--" + node.Value);
            if (BatchData != null)
            {
                if (node.NodeType == XmlNodeType.Attribute &&
                    node.Name.StartsWith("xmlns") &&
                    (String.Compare(node.Value, BaseNameTable.gBatchNamespace) == 0))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>saves the inner state of the element</summary>
        /// <param name="writer">the xmlWriter to save into </param>
        protected override void SaveInnerXml(XmlWriter writer)
        {
            // saving title
            Tracing.TraceMsg("Entering save inner XML on AtomEntry");

            if (_batchData != null)
            {
                _batchData.Save(writer);
            }

            if (_title != null)
            {
                Tracing.TraceMsg("Saving Title: " + Title.Text);
                Title.SaveToXml(writer);
            }

            if (_id != null)
            {
                Id.SaveToXml(writer);
            }

            foreach (AtomLink link in Links)
            {
                link.SaveToXml(writer);
            }

            foreach (AtomPerson person in Authors)
            {
                person.SaveToXml(writer);
            }

            foreach (AtomPerson person in Contributors)
            {
                person.SaveToXml(writer);
            }

            foreach (AtomCategory category in Categories)
            {
                category.SaveToXml(writer);
            }

            if (_rights != null)
            {
                Rights.SaveToXml(writer);
            }

            if (_summary != null)
            {
                Summary.SaveToXml(writer);
            }

            if (_content != null)
            {
                Content.SaveToXml(writer);
            }

            if (_source != null)
            {
                Source.SaveToXml(writer);
            }

            WriteLocalDateTimeElement(writer, AtomParserNameTable.XmlUpdatedElement, Updated);
            WriteLocalDateTimeElement(writer, AtomParserNameTable.XmlPublishedElement, Published);
        }

        #endregion

        /// <summary>
        /// default AtomEntry constructor. Adds the AppControl element
        /// as a default extension
        /// </summary>
        public AtomEntry()
        {
            AddExtension(new AppControl());
        }

        /// <summary>Read only accessor for feed</summary>
        public AtomFeed Feed
        {
            get { return _feed; }
        }

        /// <summary>internal method to set the feed</summary>
        internal void setFeed(AtomFeed feed)
        {
            if (feed != null)
            {
                Dirty = true;
                Service = feed.Service;
            }

            _feed = feed;
        }

        /// <summary>helper method to create a new, decoupled entry based on a feedEntry</summary>
        /// <param name="entryToImport">the entry from a feed that you want to put somewhere else</param>
        /// <returns> the new entry ready to be inserted</returns>
        public static AtomEntry ImportFromFeed(AtomEntry entryToImport)
        {
            Tracing.Assert(entryToImport != null, "entryToImport should not be null");
            if (entryToImport == null)
            {
                throw new ArgumentNullException("entryToImport");
            }

            AtomEntry entry = null;
            entry = (AtomEntry)Activator.CreateInstance(entryToImport.GetType());
            entry.CopyEntry(entryToImport);

            entry.Id = null;

            // if the source is empty, set the source to the old feed

            if (entry.Source == null)
            {
                entry.Source = entryToImport.Feed;
            }

            Tracing.TraceInfo("Imported entry: " + entryToImport.Title.Text + " to: " + entry.Title.Text);
            return entry;
        }

        /// <summary>accessor method for the GData Service to use</summary>
        public IService Service
        {
            get { return _service; }
            set { Dirty = true; _service = value; }
        }

        /// <summary>accessor to the batchdata for the entry</summary>
        /// <returns>GDataBatch object</returns>
        public GDataBatchEntryData BatchData
        {
            get { return _batchData; }
            set { _batchData = value; }
        }

        /// <summary>accessor method public Uri EditUri</summary>
        /// <returns> </returns>
        public AtomUri EditUri
        {
            get
            {
                AtomLink link = Links.FindService(BaseNameTable.ServiceEdit, AtomLink.ATOM_TYPE);
                // scan the link collection
                return link == null ? null : link.HRef;
            }

            set
            {
                AtomLink link = Links.FindService(BaseNameTable.ServiceEdit, AtomLink.ATOM_TYPE);
                if (link == null)
                {
                    link = new AtomLink(AtomLink.ATOM_TYPE, BaseNameTable.ServiceEdit);
                    Links.Add(link);
                }

                link.HRef = value;
            }
        }

        /// <summary>accessor for the self URI</summary>
        /// <returns> </returns>
        public AtomUri SelfUri
        {
            get
            {
                AtomLink link = Links.FindService(BaseNameTable.ServiceSelf, AtomLink.ATOM_TYPE);
                // scan the link collection
                return link == null ? null : link.HRef;
            }

            set
            {
                AtomLink link = Links.FindService(BaseNameTable.ServiceSelf, AtomLink.ATOM_TYPE);
                if (link == null)
                {
                    link = new AtomLink(AtomLink.ATOM_TYPE, BaseNameTable.ServiceSelf);
                    Links.Add(link);
                }

                link.HRef = value;
            }
        }

        /// <summary>accessor to find the edit-media link</summary>
        /// <returns>the Uri as AtomUri to the media upload Service</returns>
        public AtomUri MediaUri
        {
            get
            {
                // scan the link collection
                AtomLink link = Links.FindService(BaseNameTable.ServiceMedia, null);
                return link == null ? null : link.HRef;
            }

            set
            {
                AtomLink link = Links.FindService(BaseNameTable.ServiceMedia, null);
                if (link == null)
                {
                    link = new AtomLink(null, BaseNameTable.ServiceMedia);
                    Links.Add(link);
                }

                link.HRef = value;
            }
        }

        /// <summary>accessor to find the alternate link, in HTML only
        /// The method scans the link collection for a link that is of type rel=alternate
        /// and has a media type of HTML, otherwise it return NULL. The same is true for setting this.
        /// If you need to use a rel/alternate with a different media type, you need
        /// to use the links collection directly</summary>
        /// <returns>the Uri as AtomUri to HTML representation</returns>
        public AtomUri AlternateUri
        {
            get
            {
                // scan the link collection
                AtomLink link = Links.FindService(BaseNameTable.ServiceAlternate, AtomLink.HTML_TYPE);
                return link == null ? null : link.HRef;
            }

            set
            {
                AtomLink link = Links.FindService(BaseNameTable.ServiceAlternate, AtomLink.HTML_TYPE);
                if (link == null)
                {
                    link = new AtomLink(AtomLink.HTML_TYPE, BaseNameTable.ServiceAlternate);
                    Links.Add(link);
                }

                link.HRef = value;
            }
        }

        /// <summary>accessor method public string Feed</summary>
        /// <returns>returns the Uri as string for the feed service </returns>
        public string FeedUri
        {
            get
            {
                AtomLink link = Links.FindService(BaseNameTable.ServiceFeed, AtomLink.ATOM_TYPE);
                // scan the link collection
                return link == null ? null : Utilities.CalculateUri(Base, ImpliedBase, link.HRef.ToString());
            }

            set
            {
                AtomLink link = Links.FindService(BaseNameTable.ServiceFeed, AtomLink.ATOM_TYPE);
                if (link == null)
                {
                    link = new AtomLink(AtomLink.ATOM_TYPE, BaseNameTable.ServiceFeed);
                    Links.Add(link);
                }

                link.HRef = new AtomUri(value);
            }
        }

        /// <summary>accessor method public DateTime UpdateDate</summary>
        /// <returns> </returns>
        public DateTime Updated
        {
            get { return _lastUpdateDate; }
            set { Dirty = true; _lastUpdateDate = value; }
        }

        /// <summary>accessor method public DateTime PublicationDate</summary>
        /// <returns> </returns>
        public DateTime Published
        {
            get { return _publicationDate; }
            set { Dirty = true; _publicationDate = value; }
        }

        /// <summary>
        /// returns the app:control element
        /// </summary>
        /// <returns></returns>
        public AppControl AppControl
        {
            get
            {
                return FindExtension(
                    BaseNameTable.XmlElementPubControl,
                    BaseNameTable.AppPublishingNamespace(this)) as AppControl;
            }

            set
            {
                ReplaceExtension(
                    BaseNameTable.XmlElementPubControl,
                    BaseNameTable.AppPublishingNamespace(this),
                    value);
            }
        }

        /// <summary>specifies if app:control/app:draft is yes or no.
        /// this is determined by walking the extension elements collection</summary>
        /// <returns>true if this is a draft element</returns>
        public bool IsDraft
        {
            get
            {
                if (AppControl != null && AppControl.Draft != null)
                {
                    return AppControl.Draft.BooleanValue;
                }

                return false;
            }

            set
            {
                Dirty = true;
                if (AppControl == null)
                {
                    AppControl = new AppControl();
                }

                if (AppControl.Draft == null)
                {
                    AppControl.Draft = new AppDraft();
                }

                AppControl.Draft.BooleanValue = value;
            }
        }

        /// <summary>accessor method public Contributors AtomPersonCollection</summary>
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

        /// <summary>accessor method public string Content</summary>
        /// <returns> </returns>
        public AtomContent Content
        {
            get
            {
                if (_content == null)
                {
                    _content = new AtomContent();
                }

                return _content;
            }

            set { Dirty = true; _content = value; }
        }

        /// <summary>accessor method public string Summary</summary>
        /// <returns> </returns>
        public AtomTextConstruct Summary
        {
            get
            {
                if (_summary == null)
                {
                    _summary = new AtomTextConstruct(AtomTextConstructElementType.Summary);
                }

                return _summary;
            }

            set { Dirty = true; _summary = value; }
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

        /// <summary>holds an array of AtomCategory objects</summary>
        /// <returns> </returns>
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

        /// <summary>accessor method public AtomId Id</summary>
        /// <returns> </returns>
        public AtomId Id
        {
            get
            {
                if (_id == null)
                {
                    _id = new AtomId();
                }

                return _id;
            }

            set { Dirty = true; _id = value; }
        }

        /// <summary>accessor method public AtomTextConstruct Title</summary>
        /// <returns> </returns>
        public AtomTextConstruct Title
        {
            get
            {
                if (_title == null)
                {
                    _title = new AtomTextConstruct(AtomTextConstructElementType.Title);
                }

                return _title;
            }

            set { Dirty = true; _title = value; }
        }

        /// <summary>if the entry was copied, represents the source</summary>
        /// <returns> </returns>
        public AtomSource Source
        {
            get { return _source; }

            set
            {
                Dirty = true;
                AtomFeed feed = value as AtomFeed;
                if (feed != null)
                {
                    Tracing.TraceInfo("need to copy a feed to a source");
                    _source = new AtomSource(feed);
                }
                else
                {
                    _source = value;
                }
            }
        }

        /// <summary>accessor method public string rights</summary>
        /// <returns> </returns>
        public AtomTextConstruct Rights
        {
            get
            {
                if (_rights == null)
                {
                    _rights = new AtomTextConstruct(AtomTextConstructElementType.Rights);
                }

                return _rights;
            }

            set { Dirty = true; _rights = value; }
        }

        #region EDITING

        /// <summary>returns whether or not the entry is read-only</summary>
        public bool ReadOnly
        {
            get
            {
                return EditUri == null;
            }
        }

        /// <summary>commits the item to the server</summary>
        /// <returns>throws an exception if an error occured updating, returns
        /// the updated entry from the service</returns>
        public AtomEntry Update()
        {
            if (Service == null)
            {
                throw new InvalidOperationException("No Service object set");
            }

            AtomEntry updatedEntry = Service.Update(this);
            if (updatedEntry != null)
            {
                CopyEntry(updatedEntry);
                MarkElementDirty(false);
                return updatedEntry;
            }

            return null;
        }

        /// <summary>deletes the item from the server</summary>
        /// <returns>throws an exception if an error occured updating</returns>
        public void Delete()
        {
            if (Service == null)
            {
                throw new InvalidOperationException("No Service object set");
            }

            Service.Delete(this);
        }

        /// <summary>takes the updated entry returned and sets the properties to this object</summary>
        /// <param name="updatedEntry"> </param>
        protected void CopyEntry(AtomEntry updatedEntry)
        {
            Tracing.Assert(updatedEntry != null, "updatedEntry should not be null");
            if (updatedEntry == null)
            {
                throw new ArgumentNullException("updatedEntry");
            }

            _title = updatedEntry.Title;
            _authors = updatedEntry.Authors;
            _id = updatedEntry.Id;
            _links = updatedEntry.Links;
            _lastUpdateDate = updatedEntry.Updated;
            _publicationDate = updatedEntry.Published;
            _authors = updatedEntry.Authors;
            _rights = updatedEntry.Rights;
            _categories = updatedEntry.Categories;
            _summary = updatedEntry.Summary;
            _content = updatedEntry.Content;
            _source = updatedEntry.Source;

            ExtensionElements.Clear();

            foreach (IExtensionElementFactory extension in updatedEntry.ExtensionElements)
            {
                ExtensionElements.Add(extension);
            }
        }

        #endregion

        /// <summary>
        /// this is the subclassing method for AtomBase derived
        /// classes to overload what childelements should be created
        /// needed to create CustomLink type objects, like WebContentLink etc
        /// </summary>
        /// <param name="reader">The XmlReader that tells us what we are working with</param>
        /// <param name="parser">the parser is primarily used for nametable comparisons</param>
        /// <returns>AtomBase</returns>
        public override AtomBase CreateAtomSubElement(XmlReader reader, AtomFeedParser parser)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (parser == null)
            {
                throw new ArgumentNullException("parser");
            }

            Object localname = reader.LocalName;

            if (localname.Equals(parser.Nametable.Source))
            {
                return new AtomSource();
            }
            else if (localname.Equals(parser.Nametable.Content))
            {
                return new AtomContent();
            }

            return base.CreateAtomSubElement(reader, parser);
        }

        #region overloaded for property changes, xml:base

        /// <summary>just go down the child collections</summary>
        /// <param name="uriBase"> as currently calculated</param>
        internal override void BaseUriChanged(AtomUri uriBase)
        {
            base.BaseUriChanged(uriBase);
            // now pass it to the properties.
            uriBase = new AtomUri(Utilities.CalculateUri(Base, uriBase, null));

            if (Title != null)
            {
                Title.BaseUriChanged(uriBase);
            }

            if (Id != null)
            {
                Id.BaseUriChanged(uriBase);
            }

            foreach (AtomLink link in Links)
            {
                link.BaseUriChanged(uriBase);
            }

            foreach (AtomPerson person in Authors)
            {
                person.BaseUriChanged(uriBase);
            }

            foreach (AtomPerson person in Contributors)
            {
                person.BaseUriChanged(uriBase);
            }

            foreach (AtomCategory category in Categories)
            {
                category.BaseUriChanged(uriBase);
            }

            if (Rights != null)
            {
                Rights.BaseUriChanged(uriBase);
            }

            if (Summary != null)
            {
                Summary.BaseUriChanged(uriBase);
            }

            if (Content != null)
            {
                Content.BaseUriChanged(uriBase);
            }

            if (Source != null)
            {
                Source.BaseUriChanged(uriBase);
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

            if (_id != null)
            {
                if (_id.WalkTree(action))
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

            if (_rights != null)
            {
                if (_rights.WalkTree(action))
                {
                    return true;
                }
            }

            if (_title != null)
            {
                if (_title.WalkTree(action))
                {
                    return true;
                }
            }

            if (_summary != null)
            {
                if (_summary.WalkTree(action))
                {
                    return true;
                }
            }

            if (_content != null)
            {
                if (_content.WalkTree(action))
                {
                    return true;
                }
            }

            if (_source != null)
            {
                if (_source.WalkTree(action))
                {
                    return true;
                }
            }

            // nothing dirty at all
            return false;
        }

        #endregion

        /// <summary>
        /// Parses the inner state of the element
        /// </summary>
        /// <param name="e">The extension element that should be added to this entry</param>
        /// <param name="parser">The AtomFeedParser that called this</param>
        public virtual void Parse(ExtensionElementEventArgs e, AtomFeedParser parser)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            Tracing.TraceMsg("Entering Parse on AbstractEntry");
            XmlNode node = e.ExtensionElement;
            if (ExtensionFactories != null && ExtensionFactories.Count > 0)
            {
                Tracing.TraceMsg("Entring default Parsing for AbstractEntry");

                IExtensionElementFactory f = FindExtensionFactory(
                    node.LocalName,
                    node.NamespaceURI);
                if (f != null)
                {
                    ExtensionElements.Add(f.CreateInstance(node, parser));
                    e.DiscardEntry = true;
                }
            }
        }
    }
}
