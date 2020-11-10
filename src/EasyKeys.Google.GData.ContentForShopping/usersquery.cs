using System.Text;

using EasyKeys.Google.GData.Client;

namespace EasyKeys.Google.GData.ContentForShopping
{
    /// <summary>
    /// A subclass of FeedQuery, to create a ContentForShopping users feed
    /// query URI. Provides public properties that describe the different
    /// aspects of the URI, as well as a composite URI.
    /// </summary>
    public class UsersQuery : FeedQuery
    {
        private string _accountId;

        /// <summary>
        /// Constructor
        /// </summary>
        public UsersQuery()
            : base(ContentForShoppingNameTable.AllFeedsBaseUri)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UsersQuery(string accountId)
            : base(ContentForShoppingNameTable.AllFeedsBaseUri)
        {
            _accountId = accountId;
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
        /// Returns the base Uri for the feed.
        /// </summary>
        protected override string GetBaseUri()
        {
            StringBuilder sb = new StringBuilder(baseUri, 2048);

            sb.Append("/");
            sb.Append(_accountId);
            sb.Append("/users/");

            return sb.ToString();
        }
    }
}
