using System.Net;

namespace OnChotto.Models.Amazon
{
    public class ItemLink
    {
        private string _url;

        public string Description { get; set; }
        public string URL
        {
            get { return this._url; }
            set { this._url = WebUtility.UrlDecode(value); }
        }

        public override string ToString()
        {
            return this.Description;
        }
    }
}
