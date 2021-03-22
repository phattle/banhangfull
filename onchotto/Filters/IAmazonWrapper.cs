﻿using OnChotto.Models.Amazon;
using OnChotto.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnChotto.Filters
{
    public interface IAmazonWrapper
    {
        void SetEndpoint(AmazonEndpoint amazonEndpoint);
        void SetUserAgent(string userAgent);

        ExtendedWebResponse Request(AmazonOperationBase amazonOperation);
        Task<ExtendedWebResponse> RequestAsync(AmazonOperationBase amazonOperation);

        #region Lookup

        AmazonItemResponse Lookup(string articleNumber, AmazonResponseGroup responseGroup = AmazonResponseGroup.Large);
        Task<AmazonItemResponse> LookupAsync(string articleNumber, AmazonResponseGroup responseGroup = AmazonResponseGroup.Large);

        AmazonItemResponse Lookup(IList<string> articleNumbers, AmazonResponseGroup responseGroup = AmazonResponseGroup.Large);
        Task<AmazonItemResponse> LookupAsync(IList<string> articleNumbers, AmazonResponseGroup responseGroup = AmazonResponseGroup.Large);

        #endregion

        #region Search

        AmazonItemResponse Search(string search, int pageIndex, AmazonSearchIndex searchIndex = AmazonSearchIndex.All, AmazonResponseGroup responseGroup = AmazonResponseGroup.Large);
        Task<AmazonItemResponse> SearchAsync(string search, int pageIndex, AmazonSearchIndex searchIndex = AmazonSearchIndex.All, AmazonResponseGroup responseGroup = AmazonResponseGroup.Large);

        #endregion

        #region Cart

        CartCreateResponse CartCreate(IList<AmazonCartItem> amazonCartItems);
        Task<CartCreateResponse> CartCreateAsync(IList<AmazonCartItem> amazonCartItems);

        CartAddResponse CartAdd(AmazonCartItem item, string cartId, string hmac);
        Task<CartAddResponse> CartAddAsync(AmazonCartItem item, string cartId, string hmac);

        CartGetResponse CartGet(string cartId, string hmac);
        Task<CartGetResponse> CartGetAsync(string cartId, string hmac);

        CartClearResponse CartClear(string cartId, string hmac);
        Task<CartClearResponse> CartClearAsync(string cartId, string hmac);

        #endregion

        #region BrowseNode

        BrowseNodeLookupResponse BrowseNodeLookup(long browseNodeId, AmazonResponseGroup responseGroup = AmazonResponseGroup.BrowseNodeInfo);

        Task<BrowseNodeLookupResponse> BrowseNodeLookupAsync(long browseNodeId, AmazonResponseGroup responseGroup = AmazonResponseGroup.BrowseNodeInfo);

        #endregion
    }
}