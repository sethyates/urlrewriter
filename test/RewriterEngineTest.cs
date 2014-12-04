using System;
using System.Net;
using System.Web;
using System.Xml;
using Intelligencia.UrlRewriter.Configuration;
using Intelligencia.UrlRewriter.Mocks;
using Intelligencia.UrlRewriter.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Intelligencia.UrlRewriter.Tests
{
    [TestClass]
    [DeploymentItem("README.txt")]
    public class RewriterEngineTest
    {
        [TestMethod]
        public void Constructor_WithNullHttpContext_Throws()
        {
            // Arrange
            XmlNode emptySection = CreateEmptyXmlNode();
            IHttpContext httpContext = null;
            IConfigurationManager configurationManager = new MockConfigurationManager(emptySection);
            IRewriterConfiguration configuration = new RewriterConfiguration(configurationManager);

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() =>
                new RewriterEngine(httpContext, configurationManager, configuration));
        }

        [TestMethod]
        public void Constructor_WithNullConfiguratonManager_Throws()
        {
            // Arrange
            XmlNode emptySection = CreateEmptyXmlNode();
            IHttpContext httpContext = new MockHttpContext();
            IConfigurationManager configurationManager = null;
            IRewriterConfiguration configuration = new RewriterConfiguration(new MockConfigurationManager(emptySection));

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() =>
                new RewriterEngine(httpContext, configurationManager, configuration));
        }

        [TestMethod]
        public void Constructor_WithNullConfiguration_Throws()
        {
            // Arrange
            XmlNode emptySection = CreateEmptyXmlNode();
            IHttpContext httpContext = null;
            IConfigurationManager configurationManager = new MockConfigurationManager(emptySection);
            IRewriterConfiguration configuration = null;

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() =>
                new RewriterEngine(httpContext, configurationManager, configuration));
        }

        [TestMethod]
        public void Rewrite_WithRewrittenUrl_SetsLocation()
        {
            // <rewrite url="^/Test1/(.+)$" to="/NewLocation/$1" />

            // Arrange
            MockHttpContext httpContext = new MockHttpContext("http://localhost/Test1/Page.aspx");
            IConfigurationManager configurationManager = new ConfigurationManagerFacade();
            IRewriterConfiguration configuration = new RewriterConfiguration(configurationManager);
            RewriterEngine engine = new RewriterEngine(httpContext, configurationManager, configuration);

            // Act
            engine.Rewrite();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, httpContext.StatusCode);
            Assert.AreEqual("/NewLocation/Page.aspx", httpContext.RewrittenPath);
        }

        [TestMethod]
        public void Rewrite_WithPermanentlyRedirectedUrl_RedirectsWithMoved()
        {
            // <redirect url="^/Test2/(.+)$" to="/NewLocation/$1" permanent="true" />

            // Arrange
            MockHttpContext httpContext = new MockHttpContext("http://localhost/Test2/Page.aspx");
            IConfigurationManager configurationManager = new ConfigurationManagerFacade();
            IRewriterConfiguration configuration = new RewriterConfiguration(configurationManager);
            RewriterEngine engine = new RewriterEngine(httpContext, configurationManager, configuration);

            // Act
            engine.Rewrite();

            // Assert
            Assert.AreEqual(HttpStatusCode.Moved, httpContext.StatusCode);
            Assert.AreEqual("/NewLocation/Page.aspx", httpContext.RedirectLocation);
        }

        [TestMethod]
        public void Rewrite_WithTemporarilyRedirectedUrl_RedirectsWithFound()
        {
            // <redirect url="^/Test3/(.+)$" to="/NewLocation/$1" permanent="false" />

            // Arrange
            MockHttpContext httpContext = new MockHttpContext("http://localhost/Test3/Page.aspx");
            IConfigurationManager configurationManager = new ConfigurationManagerFacade();
            IRewriterConfiguration configuration = new RewriterConfiguration(configurationManager);
            RewriterEngine engine = new RewriterEngine(httpContext, configurationManager, configuration);

            // Act
            engine.Rewrite();

            // Assert
            Assert.AreEqual(HttpStatusCode.Found, httpContext.StatusCode);
            Assert.AreEqual("/NewLocation/Page.aspx", httpContext.RedirectLocation);
        }

        [TestMethod]
        public void Rewrite_WithSetProperty_SubstitutesPropertyValue()
        {
            // <set property="Property1" value="PropertyValue" />
            // <rewrite url="^/Test4/(.+)$" to="/NewLocation/$1?Property1=${Property1}" />

            // Arrange
            MockHttpContext httpContext = new MockHttpContext("http://localhost/Test4/Page.aspx");
            IConfigurationManager configurationManager = new ConfigurationManagerFacade();
            IRewriterConfiguration configuration = new RewriterConfiguration(configurationManager);
            RewriterEngine engine = new RewriterEngine(httpContext, configurationManager, configuration);

            // Act
            engine.Rewrite();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, httpContext.StatusCode);
            Assert.AreEqual("/NewLocation/Page.aspx?Property1=PropertyValue", httpContext.RewrittenPath);
        }

        [TestMethod]
        public void Rewrite_WithSetAppSettingProperty_SubstitutesPropertyValue()
        {
            // From appSettings:
            // <add key="AppSettingKey" value="AppSettingValue"/>
            //
            // From rewriter config:
            // <set-appsetting property="Property2" key="AppSettingKey" />
            // <rewrite url="^/Test5/(.+)$" to="/NewLocation/$1?Property2=${Property2}" />

            // Arrange
            MockHttpContext httpContext = new MockHttpContext("http://localhost/Test5/Page.aspx");
            IConfigurationManager configurationManager = new ConfigurationManagerFacade();
            IRewriterConfiguration configuration = new RewriterConfiguration(configurationManager);
            RewriterEngine engine = new RewriterEngine(httpContext, configurationManager, configuration);

            // Act
            engine.Rewrite();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, httpContext.StatusCode);
            Assert.AreEqual("/NewLocation/Page.aspx?Property2=AppSettingValue", httpContext.RewrittenPath);
        }

        [TestMethod]
        public void Rewrite_WithRewrittenUrlAndContinue_AppliesNextRule()
        {
            // <rewrite url="^/Test6/(.+)" to="/NewLocation1/$1" processing="continue" />
            // <rewrite url="^/NewLocation1/(.+)" to="/NewLocation2/$1" processing="stop" />

            // Arrange
            MockHttpContext httpContext = new MockHttpContext("http://localhost/Test6/Page.aspx");
            IConfigurationManager configurationManager = new ConfigurationManagerFacade();
            IRewriterConfiguration configuration = new RewriterConfiguration(configurationManager);
            RewriterEngine engine = new RewriterEngine(httpContext, configurationManager, configuration);

            // Act
            engine.Rewrite();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, httpContext.StatusCode);
            Assert.AreEqual("/NewLocation2/Page.aspx", httpContext.RewrittenPath);
        }

        [TestMethod]
        public void Rewrite_WithRewrittenUrlAndRestart_AppliesFirstRule()
        {
            // <rewrite url="^/Test7/(.+)" to="/Test1/$1" processing="restart" />
            // (Should also apply the first rule in rewriter config on restart).

            // Arrange
            MockHttpContext httpContext = new MockHttpContext("http://localhost/Test7/Page.aspx");
            IConfigurationManager configurationManager = new ConfigurationManagerFacade();
            IRewriterConfiguration configuration = new RewriterConfiguration(configurationManager);
            RewriterEngine engine = new RewriterEngine(httpContext, configurationManager, configuration);

            // Act
            engine.Rewrite();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, httpContext.StatusCode);
            Assert.AreEqual("/NewLocation/Page.aspx", httpContext.RewrittenPath);
        }

        [TestMethod]
        public void Rewrite_WithSetCookie_SetsOutputCookie()
        {
            // <if url="^/Test8/(.+)$">
            //   <set cookie="Cookie1" value="CookieValue" />
            // </if>

            // Arrange
            MockHttpContext httpContext = new MockHttpContext("http://localhost/Test8/Page.aspx");
            IConfigurationManager configurationManager = new ConfigurationManagerFacade();
            IRewriterConfiguration configuration = new RewriterConfiguration(configurationManager);
            RewriterEngine engine = new RewriterEngine(httpContext, configurationManager, configuration);

            // Act
            engine.Rewrite();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, httpContext.StatusCode);
            Assert.IsNull(httpContext.RewrittenPath);
            Assert.IsNull(httpContext.RedirectLocation);
            CollectionAssert.Contains(httpContext.ResponseCookies.Keys, "Cookie1");
            Assert.AreEqual("CookieValue", httpContext.ResponseCookies["Cookie1"].Value);
        }

        [TestMethod]
        public void Rewrite_WithAddressMatchAndSetStatus_SetsStatus()
        {
            // <if url="^/Test9/(.+)$" address="127.0.0.1">
            //   <set-status status="201" />
            // </if>

            // Arrange
            MockHttpContext httpContext = new MockHttpContext("http://localhost/Test9/Page.aspx");
            httpContext.RequestHeaders.Add("REMOTE_ADDR", "127.0.0.1"); // Should match the address="127.0.0.1" condition.
            IConfigurationManager configurationManager = new ConfigurationManagerFacade();
            IRewriterConfiguration configuration = new RewriterConfiguration(configurationManager);
            RewriterEngine engine = new RewriterEngine(httpContext, configurationManager, configuration);

            // Act
            engine.Rewrite();

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, httpContext.StatusCode); // 201
            Assert.IsNull(httpContext.RewrittenPath);
            Assert.IsNull(httpContext.RedirectLocation);
        }

        [TestMethod]
        public void Rewrite_WithHeaderMatchAndSetStatus_SetsStatus()
        {
            // <if url="^/Test10/(.+)$" header="User-Agent" match="MSIE">
            //   <set-status status="202" />
            // </if>

            // Arrange
            MockHttpContext httpContext = new MockHttpContext("http://localhost/Test10/Page.aspx");
            httpContext.RequestHeaders.Add("User-Agent", "Mozilla etc etc MSIE 9.0 etc etc"); // Should match the match="MSIE" condition.
            IConfigurationManager configurationManager = new ConfigurationManagerFacade();
            IRewriterConfiguration configuration = new RewriterConfiguration(configurationManager);
            RewriterEngine engine = new RewriterEngine(httpContext, configurationManager, configuration);

            // Act
            engine.Rewrite();

            // Assert
            Assert.AreEqual(HttpStatusCode.Accepted, httpContext.StatusCode); // 202
            Assert.IsNull(httpContext.RewrittenPath);
            Assert.IsNull(httpContext.RedirectLocation);
        }

        [TestMethod]
        public void Rewrite_WithMethodMatchAndSetStatus_SetsStatus()
        {
            // <if url="^/Test11/(.+)$" method="POST">
            //   <set status="204" />
            // </if>


            // Arrange
            MockHttpContext httpContext = new MockHttpContext("http://localhost/Test11/Page.aspx");
            httpContext.HttpMethod = "POST";
            IConfigurationManager configurationManager = new ConfigurationManagerFacade();
            IRewriterConfiguration configuration = new RewriterConfiguration(configurationManager);
            RewriterEngine engine = new RewriterEngine(httpContext, configurationManager, configuration);

            // Act
            engine.Rewrite();

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, httpContext.StatusCode); // 204
            Assert.IsNull(httpContext.RewrittenPath);
            Assert.IsNull(httpContext.RedirectLocation);
        }

        [TestMethod]
        public void Rewrite_WithPropertyMatchAndSetStatus_SetsStatus()
        {
            // <set property="Property3" value="PropertyValue" />
            // <if url="^/Test12/(.+)$" property="Property3" match="^PropertyValue$">
            //   <set status="205" />
            // </if>

            // Arrange 
            MockHttpContext httpContext = new MockHttpContext("http://localhost/Test12/Page.aspx");
            IConfigurationManager configurationManager = new ConfigurationManagerFacade();
            IRewriterConfiguration configuration = new RewriterConfiguration(configurationManager);
            RewriterEngine engine = new RewriterEngine(httpContext, configurationManager, configuration);

            // Act
            engine.Rewrite();

            // Assert
            Assert.AreEqual(HttpStatusCode.ResetContent, httpContext.StatusCode); // 205
            Assert.IsNull(httpContext.RewrittenPath);
            Assert.IsNull(httpContext.RedirectLocation);
        }

        [TestMethod]
        public void Rewrite_WithExistsAndForbidden_SetsStatus()
        {
            // <if url="^/Test13/(.+)$" exists="README.txt">
            //   <set status="206" />
            // </if>

            // Arrange 
            MockHttpContext httpContext = new MockHttpContext("http://localhost/Test13/Page.aspx");
            IConfigurationManager configurationManager = new ConfigurationManagerFacade();
            IRewriterConfiguration configuration = new RewriterConfiguration(configurationManager);
            RewriterEngine engine = new RewriterEngine(httpContext, configurationManager, configuration);

            // Act
            engine.Rewrite();

            // Assert
            Assert.AreEqual(HttpStatusCode.PartialContent, httpContext.StatusCode);
            Assert.IsNull(httpContext.RewrittenPath);
            Assert.IsNull(httpContext.RedirectLocation);
        }

        [TestMethod]
        public void Rewrite_WithForbidden_Throws()
        {
            // <if url="^/Test14/(.+)$">
            //   <forbidden />
            // </if>

            // Arrange 
            MockHttpContext httpContext = new MockHttpContext("http://localhost/Test14/Page.aspx");
            IConfigurationManager configurationManager = new ConfigurationManagerFacade();
            IRewriterConfiguration configuration = new RewriterConfiguration(configurationManager);
            RewriterEngine engine = new RewriterEngine(httpContext, configurationManager, configuration);

            // Act/Assert
            HttpException httpEx = ExceptionAssert.Throws<HttpException>(() => engine.Rewrite());
            Assert.AreEqual((int)HttpStatusCode.Forbidden, httpEx.GetHttpCode());
        }

        [TestMethod]
        public void Rewrite_WithGone_Throws()
        {
            // <if url="^/Test15/(.+)$">
            //   <gone />
            // </if>

            // Arrange 
            MockHttpContext httpContext = new MockHttpContext("http://localhost/Test15/Page.aspx");
            IConfigurationManager configurationManager = new ConfigurationManagerFacade();
            IRewriterConfiguration configuration = new RewriterConfiguration(configurationManager);
            RewriterEngine engine = new RewriterEngine(httpContext, configurationManager, configuration);

            // Act/Assert
            HttpException httpEx = ExceptionAssert.Throws<HttpException>(() => engine.Rewrite());
            Assert.AreEqual((int)HttpStatusCode.Gone, httpEx.GetHttpCode());
        }

        [TestMethod]
        public void Rewrite_WithNotFound_Throws()
        {
            // <if url="^/Test16/(.+)$">
            //   <not-found />
            // </if>

            // Arrange 
            MockHttpContext httpContext = new MockHttpContext("http://localhost/Test16/Page.aspx");
            IConfigurationManager configurationManager = new ConfigurationManagerFacade();
            IRewriterConfiguration configuration = new RewriterConfiguration(configurationManager);
            RewriterEngine engine = new RewriterEngine(httpContext, configurationManager, configuration);

            // Act/Assert
            HttpException httpEx = ExceptionAssert.Throws<HttpException>(() => engine.Rewrite());
            Assert.AreEqual((int)HttpStatusCode.NotFound, httpEx.GetHttpCode());
        }

        [TestMethod]
        public void Rewrite_WithNotImplemented_Throws()
        {
            // <if url="^/Test17/(.+)$">
            //   <not-implemented />
            // </if>

            // Arrange 
            MockHttpContext httpContext = new MockHttpContext("http://localhost/Test17/Page.aspx");
            IConfigurationManager configurationManager = new ConfigurationManagerFacade();
            IRewriterConfiguration configuration = new RewriterConfiguration(configurationManager);
            RewriterEngine engine = new RewriterEngine(httpContext, configurationManager, configuration);

            // Act/Assert
            HttpException httpEx = ExceptionAssert.Throws<HttpException>(() => engine.Rewrite());
            Assert.AreEqual((int)HttpStatusCode.NotImplemented, httpEx.GetHttpCode());
        }

        [TestMethod]
        public void Rewrite_WithAddHeader_AddsResponseHeader()
        {
            // <if url="^/Test18/(.+)$">
            //   <add header="Header1" value="HeaderValue" />
            // </if>

            // Arrange 
            MockHttpContext httpContext = new MockHttpContext("http://localhost/Test18/Page.aspx");
            IConfigurationManager configurationManager = new ConfigurationManagerFacade();
            IRewriterConfiguration configuration = new RewriterConfiguration(configurationManager);
            RewriterEngine engine = new RewriterEngine(httpContext, configurationManager, configuration);

            // Act
            engine.Rewrite();

            // Assert
            CollectionAssert.Contains(httpContext.ResponseHeaders.Keys, "Header1");
            Assert.AreEqual("HeaderValue", httpContext.ResponseHeaders["Header1"]);
        }

        [TestMethod]
        public void Rewrite_WithAddCookie_AddsCookie()
        {
            // <if url="^/Test19/(.+)$">
            //   <add cookie="CookieName" value="CookieValue" />
            // </if>

            // Arrange 
            MockHttpContext httpContext = new MockHttpContext("http://localhost/Test19/Page.aspx");
            IConfigurationManager configurationManager = new ConfigurationManagerFacade();
            IRewriterConfiguration configuration = new RewriterConfiguration(configurationManager);
            RewriterEngine engine = new RewriterEngine(httpContext, configurationManager, configuration);

            // Act
            engine.Rewrite();

            // Assert
            CollectionAssert.Contains(httpContext.ResponseCookies.Keys, "CookieName");
            HttpCookie cookie = httpContext.ResponseCookies["CookieName"];
            Assert.IsNotNull(cookie);
            Assert.AreEqual("CookieValue", cookie.Value);
        }


        private XmlNode CreateEmptyXmlNode()
        {
            XmlDocument doc = new XmlDocument();
            return doc.CreateNode(XmlNodeType.Element, "rewriter", null);
        }
    }
}
