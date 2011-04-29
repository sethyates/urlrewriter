using System;
using System.Net;
using System.Xml;
using Intelligencia.UrlRewriter.Configuration;
using Intelligencia.UrlRewriter.Mocks;
using Intelligencia.UrlRewriter.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Intelligencia.UrlRewriter.Tests
{
    [TestClass]
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


        // TODO: Up to here.....
        private XmlNode CreateEmptyXmlNode()
        {
            XmlDocument doc = new XmlDocument();
            return doc.CreateNode(XmlNodeType.Element, "rewriter", null);
        }
    }
}
