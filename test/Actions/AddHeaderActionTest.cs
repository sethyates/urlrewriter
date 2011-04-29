using System;
using Intelligencia.UrlRewriter.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Intelligencia.UrlRewriter.Actions.Tests
{
    [TestClass]
    public class AddHeaderActionTest
    {
        [TestMethod]
        public void Constructor_WithNullHeader_Throws()
        {
            // Arrange
            string header = null;
            string value = "HeaderValue";

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => new AddHeaderAction(header, value));
        }

        [TestMethod]
        public void Constructor_WithNullValue_Throws()
        {
            // Arrange
            string header = "HeaderName";
            string value = null;

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => new AddHeaderAction(header, value));
        }

        [TestMethod]
        public void Constructor_SetsHeaderAndValue()
        {
            // Arrange
            string header = "HeaderName";
            string value = "HeaderValue";

            // Act
            AddHeaderAction action = new AddHeaderAction(header, value);

            // Assert
            Assert.AreEqual(header, action.Header);
            Assert.AreEqual(value, action.Value);
        }

        [TestMethod]
        public void Execute_WithNullContext_Throws()
        {
            // Arrange
            string header = "HeaderName";
            string value = "HeaderValue";
            IRewriteContext context = null;
            AddHeaderAction action = new AddHeaderAction(header, value);

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => action.Execute(context));
        }

        [TestMethod]
        public void Execute_SetsResponseHeader_ReturnsContinueProcessing()
        {
            // Arrange
            string header = "HeaderName";
            string value = "HeaderValue";
            IRewriteContext context = new MockRewriteContext();
            AddHeaderAction action = new AddHeaderAction(header, value);

            // Act
            RewriteProcessing result = action.Execute(context);

            // Assert
            CollectionAssert.Contains(context.ResponseHeaders.Keys, header);
            Assert.AreEqual(value, context.ResponseHeaders[header]);
            Assert.AreEqual(RewriteProcessing.ContinueProcessing, result);
        }
    }
}
