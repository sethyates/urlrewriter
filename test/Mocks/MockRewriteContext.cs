using System;
using System.Collections.Specialized;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using Intelligencia.UrlRewriter.Configuration;
using Intelligencia.UrlRewriter.Utilities;

namespace Intelligencia.UrlRewriter.Mocks
{
    /// <summary>
    /// A mock IRewriteContext.
    /// </summary>
    public class MockRewriteContext : IRewriteContext
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public MockRewriteContext()
            : this(null, null, null, null, null)
        {
        }

        /// <summary>
        /// Constrtuctor.
        /// </summary>
        /// <param name="configurationManager">The IRewriteContext.ConfigurationManager instance</param>
        /// <param name="httpContext">The The IRewriteContext.HttpContext instance</param>
        /// <param name="properties">The The IRewriteContext.Properties instance</param>
        /// <param name="responseHeaders">The The IRewriteContext.ResponseHeaders instance</param>
        /// <param name="responseCookies">The The IRewriteContext.ResponseCookies instance</param>
        public MockRewriteContext(
            IConfigurationManager configurationManager,
            IHttpContext httpContext,
            NameValueCollection properties,
            NameValueCollection responseHeaders,
            HttpCookieCollection responseCookies)
        {
            ConfigurationManager = configurationManager ?? new MockConfigurationManager();
            HttpContext = httpContext ?? new MockHttpContext();
            Properties = properties ?? new NameValueCollection();
            StatusCode = HttpStatusCode.OK;
            ResponseHeaders = responseHeaders ?? new NameValueCollection();
            ResponseCookies = responseCookies ?? new HttpCookieCollection();
        }

        /// <summary>
        /// Mock implementation of IRewriteContext.ConfigurationManager
        /// </summary>
        public IConfigurationManager ConfigurationManager { get; private set; }

        /// <summary>
        /// Mock implementation of IRewriteContext.HttpContext
        /// </summary>
        public IHttpContext HttpContext { get; private set; }

        /// <summary>
        /// Mock implementation of IRewriteContext.Location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Mock implementation of IRewriteContext.StatusCode
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Mock implementation of IRewriteContext.Properties
        /// </summary>
        public NameValueCollection Properties { get; private set; }

        /// <summary>
        /// Mock implementation of IRewriteContext.ResponseHeaders
        /// </summary>
        public NameValueCollection ResponseHeaders { get; private set; }

        /// <summary>
        /// Mock implementation of IRewriteContext.ResponseCookies
        /// </summary>
        public HttpCookieCollection ResponseCookies { get; private set; }

        /// <summary>
        /// Mock implementation of IRewriteContext.LastMatch
        /// </summary>
        public Match LastMatch { get; set; }

        /// <summary>
        /// Mock implementation of IRewriteContext.Expand
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Expand(string input)
        {
            return input; // I guess?
        }

        /// <summary>
        /// Mock implementation of IRewriteContext.ResolveLocation
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public string ResolveLocation(string location)
        {
            return location; // I guess?
        }
    }
}
