﻿using OnChotto.Models.Amazon;
using OnChotto.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace OnChotto.Filters
{
    public partial class AmazonWrapper : IAmazonWrapper
    {
        private async Task<ExtendedWebResponse> SendRequestAsync(string uri)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.UserAgent = this._userAgent ?? "OnChotto.Filters";

            try
            {
                using (var response = await request.GetResponseAsync())
                {
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var xml = await streamReader.ReadToEndAsync();
                        this.XmlReceived?.Invoke(xml);

                        return new ExtendedWebResponse(HttpStatusCode.OK, xml);
                    }
                }
            }
            catch (WebException exception)
            {
                if (exception.Response == null)
                {
                    return new ExtendedWebResponse(HttpStatusCode.SeeOther, exception.Message);
                }

                using (var response = (HttpWebResponse)exception.Response)
                {
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var xml = await streamReader.ReadToEndAsync();
                        this.XmlReceived?.Invoke(xml);

                        return new ExtendedWebResponse(response.StatusCode, xml);
                    }
                }
            }
            catch (Exception exception)
            {
                return new ExtendedWebResponse(HttpStatusCode.SeeOther, exception.Message);
            }
        }

        public async Task<ExtendedWebResponse> RequestAsync(AmazonOperationBase amazonOperation)
        {
            using (var amazonSign = new AmazonSign(this._authentication, this._endpoint))
            {
                var requestUri = amazonSign.Sign(amazonOperation);
                return await SendRequestAsync(requestUri);
            }
        }

        #region Lookup

        /// <summary>
        /// ItemLookup
        /// </summary>
        /// <param name="articleNumber">ASIN, EAN, GTIN, ISBN</param>
        /// <param name="responseGroup"></param>
        /// <returns></returns>
        public Task<AmazonItemResponse> LookupAsync(string articleNumber, AmazonResponseGroup responseGroup = AmazonResponseGroup.Large)
        {
            return this.LookupAsync(new string[1] { articleNumber }, responseGroup);
        }

        public async Task<AmazonItemResponse> LookupAsync(IList<string> articleNumbers, AmazonResponseGroup responseGroup = AmazonResponseGroup.Large)
        {
            var operation = this.ItemLookupOperation(articleNumbers, responseGroup);

            var webResponse = await this.RequestAsync(operation);
            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                return XmlHelper.ParseXml<ItemLookupResponse>(webResponse.Content);
            }
            else
            {
                var errorResponse = XmlHelper.ParseXml<ItemLookupErrorResponse>(webResponse.Content);
                this.ErrorReceived?.Invoke(errorResponse);
            }

            return null;
        }

        #endregion

        #region Search

        public async Task<AmazonItemResponse> SearchAsync(string search, int pageIndex, AmazonSearchIndex searchIndex = AmazonSearchIndex.All, AmazonResponseGroup responseGroup = AmazonResponseGroup.Large)
        {
            var operation = this.ItemSearchOperation(search, pageIndex, searchIndex, responseGroup);

            var webResponse = await this.RequestAsync(operation);
            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                return XmlHelper.ParseXml<ItemSearchResponse>(webResponse.Content);
            }
            else
            {
                var errorResponse = XmlHelper.ParseXml<ItemSearchErrorResponse>(webResponse.Content);
                this.ErrorReceived?.Invoke(errorResponse);
            }

            return null;
        }

        #endregion

        #region Cart

        public async Task<CartCreateResponse> CartCreateAsync(IList<AmazonCartItem> amazonCartItems)
        {
            var operation = this.CartCreateOperation(amazonCartItems);

            var webResponse = await this.RequestAsync(operation);
            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                return XmlHelper.ParseXml<CartCreateResponse>(webResponse.Content);
            }
            else
            {
                var errorResponse = XmlHelper.ParseXml<CartCreateErrorResponse>(webResponse.Content);
                this.ErrorReceived?.Invoke(errorResponse);
            }

            return null;
        }

        public async Task<CartAddResponse> CartAddAsync(AmazonCartItem item, string cartId, string hmac)
        {
            var cart = new Cart { CartId = cartId, HMAC = hmac };
            var operation = this.CartAddOperation(item, cart);

            var webResponse = await this.RequestAsync(operation);
            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                return XmlHelper.ParseXml<CartAddResponse>(webResponse.Content);
            }
            else
            {
                var errorResponse = XmlHelper.ParseXml<CartAddErrorResponse>(webResponse.Content);
                this.ErrorReceived?.Invoke(errorResponse);
            }

            return null;
        }

        public async Task<CartGetResponse> CartGetAsync(string cartId, string hmac)
        {
            var cart = new Cart { CartId = cartId, HMAC = hmac };
            var operation = this.CartGetOperation(cart);

            var webResponse = await this.RequestAsync(operation);
            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                return XmlHelper.ParseXml<CartGetResponse>(webResponse.Content);
            }
            else
            {
                var errorResponse = XmlHelper.ParseXml<CartGetErrorResponse>(webResponse.Content);
                this.ErrorReceived?.Invoke(errorResponse);
            }

            return null;
        }

        public async Task<CartClearResponse> CartClearAsync(string cartId, string hmac)
        {
            var cart = new Cart { CartId = cartId, HMAC = hmac };
            var operation = this.CartClearOperation(cart);

            var webResponse = await this.RequestAsync(operation);
            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                return XmlHelper.ParseXml<CartClearResponse>(webResponse.Content);
            }
            else
            {
                var errorResponse = XmlHelper.ParseXml<CartClearErrorResponse>(webResponse.Content);
                this.ErrorReceived?.Invoke(errorResponse);
            }

            return null;
        }

        #endregion

        #region BrowseNode

        public async Task<BrowseNodeLookupResponse> BrowseNodeLookupAsync(long browseNodeId, AmazonResponseGroup responseGroup = AmazonResponseGroup.BrowseNodeInfo)
        {
            var operation = this.BrowseNodeLookupOperation(browseNodeId, responseGroup);

            var webResponse = await this.RequestAsync(operation);
            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                return XmlHelper.ParseXml<BrowseNodeLookupResponse>(webResponse.Content);
            }
            else
            {
                var errorResponse = XmlHelper.ParseXml<BrowseNodeLookupErrorResponse>(webResponse.Content);
                this.ErrorReceived?.Invoke(errorResponse);
            }

            return null;
        }

        #endregion
    }
}