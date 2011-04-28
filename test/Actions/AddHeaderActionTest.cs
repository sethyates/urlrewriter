using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Intelligencia.UrlRewriter.Actions.Tests
{
    [TestClass]
    public class AddHeaderActionTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullHeader_Throws()
        {
            // Arrange
            string header = null;
            string value = "HeaderValue";

            // Act
            AddHeaderAction action = new AddHeaderAction(header, value);

            // Assert
            // -- should throw ArgumentNullException
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullValue_Throws()
        {
            // Arrange
            string header = "HeaderName";
            string value = null;

            // Act
            AddHeaderAction action = new AddHeaderAction(header, value);

            // Assert
            // -- should throw ArgumentNullException
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
            StringAssert.Equals(header, action.Header);
            StringAssert.Equals(value, action.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Execute_WithNullContext_Throws()
        {
            // Arrange
            string header = "HeaderName";
            string value = "HeaderValue";
            RewriteContext context = null;
            AddHeaderAction action = new AddHeaderAction(header, value);

            // Act
            action.Execute(context);

            // Assert
            // -- should throw ArgumentNullException
        }

        [TestMethod]
        public void Execute_SetsResponseHeaderAndReturnsContinueProcessing()
        {
            // Arrange
            string header = "HeaderName";
            string value = "HeaderValue";
            RewriteContext context = new RewriteContext();
            AddHeaderAction action = new AddHeaderAction(header, value);

            // Act
            RewriteProcessing result = action.Execute(context);

            // Assert
            CollectionAssert.Contains(context.ResponseHeaders.Keys, header);
            StringAssert.Equals(context.ResponseHeaders[header], value);
            Assert.AreEqual(result, RewriteProcessing.ContinueProcessing);
        }
    }
}
