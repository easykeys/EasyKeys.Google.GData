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

using System.Xml;

#endregion

// <summary>basenametable, holds common names for atom&rss parsing</summary>
namespace EasyKeys.Google.GData.Client
{
    /// <summary>BaseNameTable. An initialized nametable for faster XML processing
    /// parses:
    ///     *  opensearch:totalResults - the total number of search results available (not necessarily all present in the feed).
    ///     *  opensearch:startIndex - the 1-based index of the first result.
    ///     *  opensearch:itemsPerPage - the maximum number of items that appear on one page. This allows clients to generate direct links to any set of subsequent pages.
    ///     *  gData:processed
    ///  </summary>
    public class BaseNameTable
    {
        /// <summary>the nametable itself, based on XML core</summary>
        private NameTable _atomNameTable;

        /// <summary>opensearch:totalResults</summary>
        private object _totalResults;

        /// <summary>opensearch:startIndex</summary>
        private object _startIndex;

        /// <summary>opensearch:itemsPerPage</summary>
        private object _itemsPerPage;

        /// <summary>xml base</summary>
        private object _baseUri;

        /// <summary>xml language</summary>
        private object _language;

        // batch extensions
        private object _batchId;
        private object _batchStatus;
        private object _batchOperation;
        private object _batchInterrupt;
        private object _batchContentType;
        private object _batchStatusCode;
        private object _batchReason;
        private object _batchErrors;
        private object _batchError;
        private object _batchSuccessCount;
        private object _batchFailureCount;
        private object _batchParsedCount;
        private object _batchField;
        private object _batchUnprocessed;

        private object _type;
        private object _value;
        private object _name;
        private object _eTagAttribute;

        /// <summary>
        /// namespace of the opensearch v1.0 elements
        /// </summary>
        public const string NSOpenSearchRss = "http://a9.com/-/spec/opensearchrss/1.0/";

        /// <summary>
        /// namespace of the opensearch v1.1 elements
        /// </summary>
        public const string NSOpenSearch11 = "http://a9.com/-/spec/opensearch/1.1/";

        /// <summary>static namespace string declaration</summary>
        public const string NSAtom = "http://www.w3.org/2005/Atom";

        /// <summary>namespace for app publishing control, draft version</summary>
        public const string NSAppPublishing = "http://purl.org/atom/app#";

        /// <summary>namespace for app publishing control, final version</summary>
        public const string NSAppPublishingFinal = "http://www.w3.org/2007/app";

        /// <summary>xml namespace</summary>
        public const string NSXml = "http://www.w3.org/XML/1998/namespace";

        /// <summary>GD namespace</summary>
        public const string gNamespace = "http://schemas.google.com/g/2005";

        /// <summary>GData batch extension namespace</summary>
        public const string gBatchNamespace = "http://schemas.google.com/gdata/batch";

        /// <summary>GD namespace prefix</summary>
        public const string gNamespacePrefix = gNamespace + "#";

        /// <summary>the post definiton in the link collection</summary>
        public const string ServicePost = gNamespacePrefix + "post";

        /// <summary>the feed definition in the link collection</summary>
        public const string ServiceFeed = gNamespacePrefix + "feed";

        /// <summary>the batch URI definition in the link collection</summary>
        public const string ServiceBatch = gNamespacePrefix + "batch";

        /// <summary>GData Kind Scheme</summary>
        public const string gKind = gNamespacePrefix + "kind";

        /// <summary>label scheme</summary>
        public const string gLabels = gNamespace + "/labels";

        /// <summary>the edit definition in the link collection</summary>
        public const string ServiceEdit = "edit";

        /// <summary>the next chunk URI in the link collection</summary>
        public const string ServiceNext = "next";

        /// <summary>the previous chunk URI in the link collection</summary>
        public const string ServicePrev = "previous";

        /// <summary>the self URI in the link collection</summary>
        public const string ServiceSelf = "self";

        /// <summary>the alternate URI in the link collection</summary>
        public const string ServiceAlternate = "alternate";

        /// <summary>the alternate URI in the link collection</summary>
        public const string ServiceMedia = "edit-media";

        /// <summary>prefix for atom if writing</summary>
        public const string AtomPrefix = "atom";

        /// <summary>prefix for gNamespace if writing</summary>
        public const string gDataPrefix = "gd";

        /// <summary>prefix for gdata:batch if writing</summary>
        public const string gBatchPrefix = "batch";

        /// <summary>prefix for gd:errors</summary>
        public const string gdErrors = "errors";

        /// <summary>prefix for gd:error</summary>
        public const string gdError = "error";

        /// <summary>prefix for gd:domain</summary>
        public const string gdDomain = "domain";

        /// <summary>prefix for gd:code</summary>
        public const string gdCode = "code";

        /// <summary>prefix for gd:location</summary>
        public const string gdLocation = "location";

        /// <summary>prefix for gd:internalReason</summary>
        public const string gdInternalReason = "internalReason";

        // app publishing control strings

        /// <summary>prefix for appPublishing if writing</summary>
        public const string gAppPublishingPrefix = "app";

        /// <summary>xmlelement for app:control</summary>
        public const string XmlElementPubControl = "control";

        /// <summary>xmlelement for app:draft</summary>
        public const string XmlElementPubDraft = "draft";

        /// <summary>xmlelement for app:draft</summary>
        public const string XmlElementPubEdited = "edited";

        /// <summary>
        /// static string for parsing the etag attribute
        /// </summary>
        /// <returns></returns>
        public const string XmlEtagAttribute = "etag";

        // batch strings:

        /// <summary>xmlelement for batch:id</summary>
        public const string XmlElementBatchId = "id";

        /// <summary>xmlelement for batch:operation</summary>
        public const string XmlElementBatchOperation = "operation";

        /// <summary>xmlelement for batch:status</summary>
        public const string XmlElementBatchStatus = "status";

        /// <summary>xmlelement for batch:interrupted</summary>
        public const string XmlElementBatchInterrupt = "interrupted";

        /// <summary>xmlattribute for batch:status@contentType</summary>
        public const string XmlAttributeBatchContentType = "content-type";

        /// <summary>xmlattribute for batch:status@code</summary>
        public const string XmlAttributeBatchStatusCode = "code";

        /// <summary>xmlattribute for batch:status@reason</summary>
        public const string XmlAttributeBatchReason = "reason";

        /// <summary>xmlelement for batch:status:errors</summary>
        public const string XmlElementBatchErrors = "errors";

        /// <summary>xmlelement for batch:status:errors:error</summary>
        public const string XmlElementBatchError = "error";

        /// <summary>xmlattribute for batch:interrupted@success</summary>
        public const string XmlAttributeBatchSuccess = "success";

        /// <summary>XmlAttribute for batch:interrupted@parsed</summary>
        public const string XmlAttributeBatchParsed = "parsed";

        /// <summary>XmlAttribute for batch:interrupted@field</summary>
        public const string XmlAttributeBatchField = "field";

        /// <summary>XmlAttribute for batch:interrupted@unprocessed</summary>
        public const string XmlAttributeBatchUnprocessed = "unprocessed";

        /// <summary>XmlConstant for value in enums</summary>
        public const string XmlValue = "value";

        /// <summary>XmlConstant for name in enums</summary>
        public const string XmlName = "name";

        /// <summary>XmlAttribute for type in enums</summary>
        public const string XmlAttributeType = "type";

        /// <summary>XmlAttribute for key in enums</summary>
        public const string XmlAttributeKey = "key";

        /// <summary>initializes the name table for use with atom parsing. This is the
        /// only place where strings are defined for parsing</summary>
        public virtual void InitAtomParserNameTable()
        {
            // create the nametable object
            Tracing.TraceCall("Initializing basenametable support");
            _atomNameTable = new NameTable();
            // <summary>add the keywords for the Feed
            _totalResults = _atomNameTable.Add("totalResults");
            _startIndex = _atomNameTable.Add("startIndex");
            _itemsPerPage = _atomNameTable.Add("itemsPerPage");
            _baseUri = _atomNameTable.Add("base");
            _language = _atomNameTable.Add("lang");

            // batch keywords
            _batchId = _atomNameTable.Add(BaseNameTable.XmlElementBatchId);
            _batchOperation = _atomNameTable.Add(BaseNameTable.XmlElementBatchOperation);
            _batchStatus = _atomNameTable.Add(BaseNameTable.XmlElementBatchStatus);
            _batchInterrupt = _atomNameTable.Add(BaseNameTable.XmlElementBatchInterrupt);
            _batchContentType = _atomNameTable.Add(BaseNameTable.XmlAttributeBatchContentType);
            _batchStatusCode = _atomNameTable.Add(BaseNameTable.XmlAttributeBatchStatusCode);
            _batchReason = _atomNameTable.Add(BaseNameTable.XmlAttributeBatchReason);
            _batchErrors = _atomNameTable.Add(BaseNameTable.XmlElementBatchErrors);
            _batchError = _atomNameTable.Add(BaseNameTable.XmlElementBatchError);
            _batchSuccessCount = _atomNameTable.Add(BaseNameTable.XmlAttributeBatchSuccess);
            _batchFailureCount = _batchError;
            _batchParsedCount = _atomNameTable.Add(BaseNameTable.XmlAttributeBatchParsed);
            _batchField = _atomNameTable.Add(BaseNameTable.XmlAttributeBatchField);
            _batchUnprocessed = _atomNameTable.Add(BaseNameTable.XmlAttributeBatchUnprocessed);

            _type = _atomNameTable.Add(BaseNameTable.XmlAttributeType);
            _value = _atomNameTable.Add(BaseNameTable.XmlValue);
            _name = _atomNameTable.Add(BaseNameTable.XmlName);
            _eTagAttribute = _atomNameTable.Add(BaseNameTable.XmlEtagAttribute);
        }

        #region Read only accessors 8/10/2005

        /// <summary>Read only accessor for atomNameTable</summary>
        internal NameTable Nametable
        {
            get { return _atomNameTable; }
        }

        /// <summary>Read only accessor for BatchId</summary>
        public object BatchId
        {
            get { return _batchId; }
        }

        /// <summary>Read only accessor for BatchOperation</summary>
        public object BatchOperation
        {
            get { return _batchOperation; }
        }

        /// <summary>Read only accessor for BatchStatus</summary>
        public object BatchStatus
        {
            get { return _batchStatus; }
        }

        /// <summary>Read only accessor for BatchInterrupt</summary>
        public object BatchInterrupt
        {
            get { return _batchInterrupt; }
        }

        /// <summary>Read only accessor for BatchContentType</summary>
        public object BatchContentType
        {
            get { return _batchContentType; }
        }

        /// <summary>Read only accessor for BatchStatusCode</summary>
        public object BatchStatusCode
        {
            get { return _batchStatusCode; }
        }

        /// <summary>Read only accessor for BatchErrors</summary>
        public object BatchErrors
        {
            get { return _batchErrors; }
        }

        /// <summary>Read only accessor for BatchError</summary>
        public object BatchError
        {
            get { return _batchError; }
        }

        /// <summary>Read only accessor for BatchReason</summary>
        public object BatchReason
        {
            get { return _batchReason; }
        }

        /// <summary>Read only accessor for BatchReason</summary>
        public object BatchField
        {
            get { return _batchField; }
        }

        /// <summary>Read only accessor for BatchUnprocessed</summary>
        public object BatchUnprocessed
        {
            get { return _batchUnprocessed; }
        }

        /// <summary>Read only accessor for BatchSuccessCount</summary>
        public object BatchSuccessCount
        {
            get { return _batchSuccessCount; }
        }

        /// <summary>Read only accessor for BatchFailureCount</summary>
        public object BatchFailureCount
        {
            get { return _batchFailureCount; }
        }

        /// <summary>Read only accessor for BatchParsedCount</summary>
        public object BatchParsedCount
        {
            get { return _batchParsedCount; }
        }

        /// <summary>Read only accessor for totalResults</summary>
        public object TotalResults
        {
            get { return _totalResults; }
        }

        /// <summary>Read only accessor for startIndex</summary>
        public object StartIndex
        {
            get { return _startIndex; }
        }

        /// <summary>Read only accessor for itemsPerPage</summary>
        public object ItemsPerPage
        {
            get { return _itemsPerPage; }
        }

        /// <summary>Read only accessor for parameter</summary>
        public static string Parameter
        {
            get { return "parameter"; }
        }

        /// <summary>Read only accessor for baseUri</summary>
        public object Base
        {
            get { return _baseUri; }
        }

        /// <summary>Read only accessor for language</summary>
        public object Language
        {
            get { return _language; }
        }

        /// <summary>Read only accessor for value</summary>
        public object Value
        {
            get { return _value; }
        }

        /// <summary>Read only accessor for value</summary>
        public object Type
        {
            get { return _type; }
        }

        /// <summary>Read only accessor for name</summary>
        public object Name
        {
            get { return _name; }
        }

        /// <summary>Read only accessor for etag</summary>
        public object ETag
        {
            get { return _eTagAttribute; }
        }

        #endregion end of Read only accessors

        /// <summary>
        /// returns the correct opensearchnamespace to use based
        /// on the version information passed in. All protocols with
        /// version > 1 use opensearch1.1 where version 1 uses
        /// opensearch 1.0
        /// </summary>
        /// <param name="v">The versioninformation</param>
        /// <returns></returns>
        public static string OpenSearchNamespace(IVersionAware v)
        {
            int major = VersionDefaults.Major;
            if (v != null)
            {
                major = v.ProtocolMajor;
            }

            if (major == 1)
            {
                return BaseNameTable.NSOpenSearchRss;
            }

            return BaseNameTable.NSOpenSearch11;
        }

        /// <summary>
        /// returns the correct app:publishing namespace to use based
        /// on the version information passed in. All protocols with
        /// version > 1 use the final version of the namespace, where
        /// version 1 uses the draft version.
        /// </summary>
        /// <param name="v">The versioninformation</param>
        /// <returns></returns>
        public static string AppPublishingNamespace(IVersionAware v)
        {
            int major = VersionDefaults.Major;

            if (v != null)
            {
                major = v.ProtocolMajor;
            }

            if (major == 1)
            {
                return BaseNameTable.NSAppPublishing;
            }

            return BaseNameTable.NSAppPublishingFinal;
        }
    }
}
