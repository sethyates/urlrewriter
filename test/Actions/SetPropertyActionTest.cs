using System;
using Intelligencia.UrlRewriter.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Intelligencia.UrlRewriter.Actions.Tests
{
    [TestClass]
    public class SetPropertyActionTest
    {
        [TestMethod]
        public void Constructor_WithNullName_Throws()
        {
            // Arrange
            string propertyName = null;
            string propertyValue = "PropertyValue";

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => new SetPropertyAction(propertyName, propertyValue));
        }

        [TestMethod]
        public void Constructor_WithNullValue_Throws()
        {
            // Arrange
            string propertyName = "PropertyName";
            string propertyValue = null;

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => new SetPropertyAction(propertyName, propertyValue));
        }

        [TestMethod]
        public void Constructor_SetsNameAndValue()
        {
            // Arrange
            string propertyName = "PropertyName";
            string propertyValue = "PropertyValue";

            // Act
            SetPropertyAction action = new SetPropertyAction(propertyName, propertyValue);

            // Assert
            Assert.AreEqual(propertyName, action.Name);
            Assert.AreEqual(propertyValue, action.Value);
        }

        [TestMethod]
        public void Execute_WithNullContext_Throws()
        {
            // Arrange
            SetPropertyAction action = new SetPropertyAction("Name", "Value");
            IRewriteContext context = null;

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => action.Execute(context));
        }

        [TestMethod]
        public void Execute_SetsProperty_ReturnsContinueProcessing()
        {
            // Arrange
            string propertyName = "PropertyName";
            string propertyValue = "PropertyValue";
            SetPropertyAction action = new SetPropertyAction(propertyName, propertyValue);
            IRewriteContext context = new MockRewriteContext();

            // Act
            RewriteProcessing result = action.Execute(context);

            // Assert
            Assert.AreEqual(RewriteProcessing.ContinueProcessing, result);
            CollectionAssert.Contains(context.Properties.Keys, propertyName);
            Assert.AreEqual(propertyValue, context.Properties[propertyName]);
        }
    }
}
