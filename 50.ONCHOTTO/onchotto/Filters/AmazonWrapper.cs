﻿using OnChotto.Models.Amazon;
using OnChotto.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace OnChotto.Filters
{
    public partial class AmazonWrapper : IAmazonWrapper
    {
        private AmazonAuthentication _authentication;
        private AmazonEndpoint _endpoint;
        private string _associateTag;
        private string _userAgent = null;

        public event Action<string> XmlReceived;
        public event Action<AmazonErrorResponse> ErrorReceived;

        public AmazonWrapper(AmazonAuthentication authentication, AmazonEndpoint endpoint, string associateTag = "smuavn0b-20")
        {
            this._authentication = authentication;
            this._endpoint = endpoint;
            this._associateTag = associateTag;
        }

        private ExtendedWebResponse SendRequest(string uri)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.UserAgent = this._userAgent ?? "OnChotto.Filters";

            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var xml = streamReader.ReadToEnd();
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
                        var xml = streamReader.ReadToEnd();
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

        public void SetEndpoint(AmazonEndpoint amazonEndpoint)
        {
            this._endpoint = amazonEndpoint;
        }

        public void SetUserAgent(string userAgent)
        {  
            this._userAgent = userAgent;  
        }

        public ExtendedWebResponse Request(AmazonOperationBase amazonOperation)
        {
            using (var amazonSign = new AmazonSign(this._authentication, this._endpoint))
            {
                var requestUri = amazonSign.Sign(amazonOperation);
                return SendRequest(requestUri);
            }
        }

        #region Operations

        public AmazonItemLookupOperation ItemLookupOperation(IList<string> articleNumbers, AmazonResponseGroup amazonResponseGroup = AmazonResponseGroup.Large)
        {
            var operation = new AmazonItemLookupOperation();
            operation.ResponseGroup(amazonResponseGroup);
            operation.Get(articleNumbers);
            operation.AssociateTag(this._associateTag);

            return operation;
        }

        public AmazonItemSearchOperation ItemSearchOperation(string search, int pageIndex, AmazonSearchIndex amazonSearchIndex = AmazonSearchIndex.All, AmazonResponseGroup amazonResponseGroup = AmazonResponseGroup.Large)
        {
            AmazonItemSearchOperation operation = new AmazonItemSearchOperation();
            operation.ResponseGroup(amazonResponseGroup);
            operation.Keywords(search);           
            operation.SearchIndex(amazonSearchIndex);
            operation.AssociateTag(this._associateTag);
            operation.Skip(pageIndex);
            return operation;
        }

        public AmazonBrowseNodeLookupOperation BrowseNodeLookupOperation(long browseNodeId, AmazonResponseGroup amazonResponseGroup = AmazonResponseGroup.BrowseNodeInfo)
        {
            var operation = new AmazonBrowseNodeLookupOperation();
            operation.ResponseGroup(amazonResponseGroup);
            operation.BrowseNodeId(browseNodeId);
            operation.AssociateTag(this._associateTag);

            return operation;
        }

        public AmazonCartCreateOperation CartCreateOperation(IList<AmazonCartItem> amazonCartItems)
        {
            var operation = new AmazonCartCreateOperation();
            operation.AssociateTag(this._associateTag);
            operation.AddArticles(amazonCartItems);
            return operation;
        }

        public AmazonCartAddOperation CartAddOperation(AmazonCartItem amazonCartItem, Cart cart)
        {
            var operation = new AmazonCartAddOperation();
            operation.AssociateTag(this._associateTag);
            operation.AddItemToCart(amazonCartItem, cart);
            return operation;

        }

        public AmazonCartGetOperation CartGetOperation(Cart cart)
        {
            var operation = new AmazonCartGetOperation();
            operation.AssociateTag(this._associateTag);
            operation.GetCart(cart);
            return operation;
        }

        public AmazonCartClearOperation CartClearOperation(Cart cart)
        {
            var operation = new AmazonCartClearOperation();
            operation.AssociateTag(this._associateTag);
            operation.ParameterDictionary.Add("CartId", cart.CartId);
            operation.ParameterDictionary.Add("HMAC", cart.HMAC);
            return operation;
        }

        #endregion

        #region Lookup

        /// <summary>
        /// ItemLookup
        /// </summary>
        /// <param name="articleNumber">ASIN, EAN, GTIN, ISBN</param>
        /// <param name="responseGroup"></param>
        /// <returns></returns>
        public AmazonItemResponse Lookup(string articleNumber, AmazonResponseGroup responseGroup = AmazonResponseGroup.Large)
        {
            return this.Lookup(new string[1] { articleNumber }, responseGroup);
        }

        public AmazonItemResponse Lookup(IList<string> articleNumbers, AmazonResponseGroup responseGroup = AmazonResponseGroup.Large)
        {
            var operation = this.ItemLookupOperation(articleNumbers, responseGroup);

            var webResponse = this.Request(operation);
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

        public AmazonItemResponse Search(string search,int pageIndex, AmazonSearchIndex searchIndex = AmazonSearchIndex.All, AmazonResponseGroup responseGroup = AmazonResponseGroup.Large)
        {
            var operation = this.ItemSearchOperation(search, pageIndex, searchIndex, responseGroup);
            var webResponse = this.Request(operation);
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

        public CartCreateResponse CartCreate(IList<AmazonCartItem> amazonCartItems)
        {
            var operation = this.CartCreateOperation(amazonCartItems);

            var webResponse = this.Request(operation);
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

        public CartAddResponse CartAdd(AmazonCartItem item, string cartId, string hmac)
        {
            var cart = new Cart { CartId = cartId, HMAC = hmac };
            var operation = this.CartAddOperation(item, cart);

            var webResponse = this.Request(operation);
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

        public CartGetResponse CartGet(string cartId, string hmac)
        {
            var cart = new Cart { CartId = cartId, HMAC = hmac };
            var operation = this.CartGetOperation(cart);

            var webResponse = this.Request(operation);
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

        public CartClearResponse CartClear(string cartId, string hmac)
        {
            var cart = new Cart { CartId = cartId, HMAC = hmac };
            var operation = this.CartClearOperation(cart);

            var webResponse = this.Request(operation);
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

        public BrowseNodeLookupResponse BrowseNodeLookup(long browseNodeId, AmazonResponseGroup responseGroup = AmazonResponseGroup.BrowseNodeInfo)
        {
            var operation = this.BrowseNodeLookupOperation(browseNodeId, responseGroup);

            var webResponse = this.Request(operation);
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