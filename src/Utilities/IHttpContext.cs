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
    /// Map path delegate
    /// </summary>
    /// <param name="url">The url to map</param>
    /// <returns>The physical path.</returns>
    public delegate string MapPath(string url);

    /// <summary>
    /// Interface for the HTTP context.
    /// Useful for plugging out the HttpContext.Current object in unit tests.
    /// </summary>
    public interface IHttpContext
    {
        /// <summary>
        /// Retrieves the application path.
        /// </summary>
        string ApplicationPath { get; }

        /// <summary>
        /// Retrieves the raw url.
        /// </summary>
        string RawUrl { get; }

        /// <summary>
        /// Retrieves the current request url.
        /// </summary>
        Uri RequestUrl { get; }

        /// <summary>
        /// Delegate to use for mapping paths.
        /// </summary>
        MapPath MapPath { get; }

        /// <summary>
        /// Sets the status code for the response.
        /// </summary>
        /// <param name="code">The status code.</param>
        void SetStatusCode(int code);

        /// <summary>
        /// Rewrites the request to the new url.
        /// </summary>
        /// <param name="url">The new url to rewrite to.</param>
        void RewritePath(string url);

        /// <summary>
        /// Sets the redirection location to the given url.
        /// </summary>
        /// <param name="url">The url of the redirection location.</param>
        void SetRedirectLocation(string url);

        /// <summary>
        /// Adds a header to the response.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header value.</param>
        void SetResponseHeader(string name, string value);

        /// <summary>
        /// Adds a cookie to the response.
        /// </summary>
        /// <param name="cookie">The cookie to add.</param>
        void SetResponseCookie(HttpCookie cookie);

        /// <summary>
        /// Handles an error with the error handler.
        /// </summary>
        /// <param name="handler">The error handler to use.</param>
        void HandleError(IRewriteErrorHandler handler);

        /// <summary>
        /// Sets a context item.
        /// </summary>
        /// <param name="item">The item key</param>
        /// <param name="value">The item value</param>
        void SetItem(object item, object value);

        /// <summary>
        /// Retrieves a context item.
        /// </summary>
        /// <param name="item">The item key.</param>
        /// <returns>The item value.</returns>
        object GetItem(object item);

        /// <summary>
        /// Retrieves the HTTP method used by the request.
        /// </summary>
        string HttpMethod { get; }

        /// <summary>
        /// Gets a collection of server variables.
        /// </summary>
        NameValueCollection ServerVariables { get; }

        /// <summary>
        /// Gets a collection of request headers.
        /// </summary>
        NameValueCollection RequestHeaders { get; }

        /// <summary>
        /// Gets a collection of request cookies.
        /// </summary>
        HttpCookieCollection RequestCookies { get; }
    }
}
