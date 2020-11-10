using System;
using System.Text;

using EasyKeys.Google.GData.Client;

namespace EasyKeys.Google.GData.ContentForShopping
{
    /// <summary>
    /// A subclass of FeedQuery, to create a ContentForShopping item query URI.
    /// Provides public properties that describe the different
    /// aspects of the URI, as well as a composite URI.
    /// </summary>
    public abstract class ItemQuery : FeedQuery
    {
        private readonly string _dataType;
        private string _accountId;
        private string _projection;
        private string _startToken;
        private string _performanceStart;
        private string _performanceEnd;

        /// <summary>
        /// Constructor
        /// </summary>
        public ItemQuery(string dataType)
            : base(ContentForShoppingNameTable.AllFeedsBaseUri)
        {
            _dataType = dataType;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ItemQuery(string dataType, string projection, string accountId)
            : base(ContentForShoppingNameTable.AllFeedsBaseUri)
        {
            _accountId = accountId;
            _dataType = dataType;
            _projection = projection;
        }

        /// <summary>
        /// Accessor method for AccountId.
        /// </summary>
        public string AccountId
        {
            get { return _accountId; }
            set { _accountId = value; }
        }

        /// <summary>
        /// Accessor method for Projection.
        /// </summary>
        public string Projection
        {
            get { return _projection; }
            set { _projection = value; }
        }

        /// <summary>
        /// Accessor method for StartToken.
        /// </summary>
        public string StartToken
        {
            get { return _startToken; }
            set { _startToken = value; }
        }

        /// <summary>
        /// Accessor method for Performance Start.
        /// </summary>
        public string PerformanceStart
        {
            get { return _performanceStart; }
            set { _performanceStart = value; }
        }

        /// <summary>
        /// Accessor method for Performance End.
        /// </summary>
        public string PerformanceEnd
        {
            get { return _performanceEnd; }
            set { _performanceEnd = value; }
        }

        /// <summary>
        /// Creates the URI query string based on all set properties.
        /// </summary>
        /// <returns>the URI query string</returns>
        protected override string CalculateQuery(string basePath)
        {
            string path = base.CalculateQuery(basePath);
            StringBuilder newPath = new StringBuilder(path, 2048);
            char paramInsertion = InsertionParameter(path);

            if (StartToken != null)
            {
                paramInsertion = AppendQueryPart(StartToken, "start-token", paramInsertion, newPath);
            }

            if (PerformanceStart != null && PerformanceEnd != null)
            {
                paramInsertion = AppendQueryPart(PerformanceStart, "performance.start", paramInsertion, newPath);
                paramInsertion = AppendQueryPart(PerformanceEnd, "performance.end", paramInsertion, newPath);
            }

            return newPath.ToString();
        }

        /// <summary>
        /// Parses an incoming URI string and sets the instance variables of this object.
        /// </summary>
        /// <param name="targetUri">Takes an incoming Uri string and parses all the properties of it</param>
        /// <returns>Throws a query exception when it finds something wrong with the input, otherwise returns a baseuri.</returns>
        protected override Uri ParseUri(Uri targetUri)
        {
            base.ParseUri(targetUri);
            if (targetUri != null)
            {
                char[] delimiters = { '?', '&' };

                string source = HttpUtility.UrlDecode(targetUri.Query);
                TokenCollection tokens = new TokenCollection(source, delimiters);
                foreach (String token in tokens)
                {
                    if (token.Length > 0)
                    {
                        char[] otherDelimiters = { '=' };
                        String[] parameters = token.Split(otherDelimiters, 2);
                        switch (parameters[0])
                        {
                            case "performance.start":
                                PerformanceStart = parameters[1];
                                break;
                            case "performance.end":
                                PerformanceEnd = parameters[1];
                                break;
                        }
                    }
                }
            }

            return Uri;
        }

        /// <summary>
        /// Resets object state to default, as if newly created.
        /// </summary>
        protected override void Reset()
        {
            base.Reset();
            StartToken = null;
        }

        /// <summary>
        /// Returns the base Uri for the feed.
        /// </summary>
        protected override string GetBaseUri()
        {
            StringBuilder sb = new StringBuilder(ContentForShoppingNameTable.AllFeedsBaseUri, 2048);

            sb.Append("/");
            sb.Append(_accountId);
            sb.Append("/items/");
            sb.Append(_dataType);
            sb.Append("/");
            sb.Append(_projection);

            return sb.ToString();
        }
    }
}
