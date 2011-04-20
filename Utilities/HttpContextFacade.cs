// UrlRewriter - A .NET URL Rewriter module
// Version 2.0
//
// Copyright 2011 Intelligencia
// Copyright 2011 Seth Yates
// 

using System;
using System.Web;
using System.Collections.Specialized;

namespace Intelligencia.UrlRewriter.Utilities
{
    /// <summary>
    /// A naive pass-through implementation of the IHttpContext facade that proxys calls to HttpContext.Current.
    /// Mock implementations would want to do something more interesting like implement checks that the actions
    /// were called.
    /// </summary>
    internal class HttpContextFacade : IHttpContext
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public HttpContextFacade()
        {
            _mapPath = new MapPath(InternalMapPath);
        }

        /// <summary>
        /// MapPath delegate.
        /// </summary>
        public MapPath MapPath
        {
            get { return _mapPath; }
        }

        /// <summary>
        /// Retrieves the application path.
        /// </summary>
        public string ApplicationPath
        {
            get { return HttpContext.Current.Request.ApplicationPath; }
        }

        /// <summary>
        /// Retrieves the raw url.
        /// </summary>
        public string RawUrl
        {
            get { return HttpContext.Current.Request.RawUrl; }
        }

        /// <summary>
        /// Retrieves the current request url.
        /// </summary>
        public Uri RequestUrl
        {
            get { return HttpContext.Current.Request.Url; }
        }

        /// <summary>
        /// Maps the url to the local file path.
        /// </summary>
        /// <param name="url">The url to map.</param>
        /// <returns>The local file path.</returns>
        private string InternalMapPath(string url)
        {
            return HttpContext.Current.Server.MapPath(url);
        }

        /// <summary>
        /// Sets the status code for the response.
        /// </summary>
        /// <param name="code">The status code.</param>
        public void SetStatusCode(int code)
        {
            HttpContext.Current.Response.StatusCode = code;
        }

        /// <summary>
        /// Rewrites the request to the new url.
        /// </summary>
        /// <param name="url">The new url to rewrite to.</param>
        public void RewritePath(string url)
        {
            HttpContext.Current.RewritePath(url, false);
        }

        /// <summary>
        /// Sets the redirection location to the given url.
        /// </summary>
        /// <param name="url">The url of the redirection location.</param>
        public void SetRedirectLocation(string url)
        {
            HttpContext.Current.Response.RedirectLocation = url;
        }

        /// <summary>
        /// Adds a header to the response.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header value.</param>
        public void SetResponseHeader(string name, string value)
        {
            HttpContext.Current.Response.AppendHeader(name, value);
        }

        /// <summary>
        /// Adds a cookie to the response.
        /// </summary>
        /// <param name="cookie">The cookie to add.</param>
        public void SetResponseCookie(HttpCookie cookie)
        {
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// Handles an error with the error handler.
        /// </summary>
        /// <param name="handler">The error handler to use.</param>
        public void HandleError(IRewriteErrorHandler handler)
        {
            handler.HandleError(HttpContext.Current);
        }

        /// <summary>
        /// Sets a context item.
        /// </summary>
        /// <param name="item">The item key</param>
        /// <param name="value">The item value</param>
        public void SetItem(object item, object value)
        {
            HttpContext.Current.Items[item] = value;
        }

        /// <summary>
        /// Retrieves a context item.
        /// </summary>
        /// <param name="item">The item key.</param>
        /// <returns>The item value.</returns>
        public object GetItem(object item)
        {
            return HttpContext.Current.Items[item];
        }

        /// <summary>
        /// Retrieves the HTTP method used by the request.
        /// </summary>
        public string HttpMethod
        {
            get { return HttpContext.Current.Request.HttpMethod; }
        }

        /// <summary>
        /// Gets a collection of server variables.
        /// </summary>
        public NameValueCollection ServerVariables
        {
            get { return HttpContext.Current.Request.ServerVariables; }
        }

        /// <summary>
        /// Gets a collection of the request headers.
        /// </summary>
        public NameValueCollection RequestHeaders
        {
            get { return HttpContext.Current.Request.Headers; }
        }

        /// <summary>
        /// Gets a collection of request cookies.
        /// </summary>
        public HttpCookieCollection RequestCookies
        {
            get { return HttpContext.Current.Request.Cookies; }
        }

        private MapPath _mapPath;
    }
}
