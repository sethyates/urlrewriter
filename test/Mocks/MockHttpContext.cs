using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using Intelligencia.UrlRewriter.Utilities;

namespace Intelligencia.UrlRewriter.Mocks
{
    /// <summary>
    /// A mock IHttpContext.
    /// </summary>
    public class MockHttpContext : IHttpContext
    {
        public const string DefaultUrl = "http://localhost/";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MockHttpContext()
            : this(new Uri(DefaultUrl))
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MockHttpContext(string uri)
            : this(new Uri(uri))
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="uri">The pretend request URI</param>
        public MockHttpContext(Uri uri)
        {
            // Default to our default URL if no URI is specified.
            uri = uri ?? new Uri(DefaultUrl);

            // Initialise the IHttpContext interface parameters.
            ApplicationPath = "/";
            RawUrl = uri.AbsolutePath;
            RequestUrl = uri;
            Items = new Dictionary<string, object>();
            HttpMethod = "GET";
            ServerVariables = new NameValueCollection();
            RequestHeaders = new NameValueCollection();

            // Initialise results.
            StatusCode = HttpStatusCode.OK;
            RequestCookies = new HttpCookieCollection();
            ResponseHeaders = new NameValueCollection();
            ResponseCookies = new HttpCookieCollection();
        }

        #region Results

        /// <summary>
        /// The HTTP status code, as set by SetStatusCode().
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// The path, as set by RewritePath().
        /// </summary>
        public string RewrittenPath { get; private set; }

        /// <summary>
        /// The redirect location, as set by SetRedirectLocation().
        /// </summary>
        public string RedirectLocation { get; private set; }

        /// <summary>
        /// The response headers, as affected by SetResponseHeader().
        /// </summary>
        public NameValueCollection ResponseHeaders { get; private set; }

        /// <summary>
        /// The response cookies, as affected by SetResponseCookie().
        /// </summary>
        public HttpCookieCollection ResponseCookies { get; private set; }

        #endregion

        #region IHttpContext Interface

        /// <summary>
        /// Mock implementation of IHttpContext.ApplicationPath
        /// </summary>
        public string ApplicationPath { get; private set; }

        /// <summary>
        /// Mock implementation of IHttpContext.RawUrl
        /// </summary>
        public string RawUrl { get; private set; }

        /// <summary>
        /// Mock implementation of IHttpContext.RequestUrl
        /// </summary>
        public Uri RequestUrl { get; private set; }

        /// <summary>
        /// Mock implementation of IHttpContext.MapPath
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string MapPath(string url)
        {
            return url;
        }

        /// <summary>
        /// Mock implementation of IHttpContext.SetStatusCode
        /// </summary>
        /// <param name="code"></param>
        public void SetStatusCode(HttpStatusCode code)
        {
            StatusCode = code;
        }

        /// <summary>
        /// Mock implementation of IHttpContext.RewritePath
        /// </summary>
        /// <param name="url"></param>
        public void RewritePath(string url)
        {
            RewrittenPath = url;
        }

        /// <summary>
        /// Mock implementation of IHttpContext.SetRedirectLocation
        /// </summary>
        /// <param name="url"></param>
        public void SetRedirectLocation(string url)
        {
            RedirectLocation = url;
        }

        /// <summary>
        /// Mock implementation of IHttpContext.SetResponseHeader
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetResponseHeader(string name, string value)
        {
            ResponseHeaders[name] = value;
        }

        /// <summary>
        /// Mock implementation of IHttpContext.SetResponseCookie
        /// </summary>
        /// <param name="cookie"></param>
        public void SetResponseCookie(HttpCookie cookie)
        {
            ResponseCookies.Add(cookie);
        }

        /// <summary>
        /// Mock implementation of IHttpContext.HandleError
        /// </summary>
        /// <param name="handler"></param>
        public void HandleError(IRewriteErrorHandler handler)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Mock implementation of IHttpContext.Items
        /// </summary>
        public IDictionary Items { get; private set; }

        /// <summary>
        /// Mock implementation of IHttpContext.HttpMethod
        /// </summary>
        public string HttpMethod { get; private set; }

        /// <summary>
        /// Mock implementation of IHttpContext.ServerVariables
        /// </summary>
        public NameValueCollection ServerVariables { get; private set; }

        /// <summary>
        /// Mock implementation of IHttpContext.RequestHeaders
        /// </summary>
        public NameValueCollection RequestHeaders { get; private set; }

        /// <summary>
        /// Mock implementation of IHttpContext.RequestCookies
        /// </summary>
        public HttpCookieCollection RequestCookies { get; private set; }

        #endregion
    }
}
