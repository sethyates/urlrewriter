using System;
using System.Net;
using Intelligencia.UrlRewriter.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Intelligencia.UrlRewriter.Actions.Tests
{
    [TestClass]
    public class NotImplementedActionTest
    {
        [TestMethod]
        public void Constructor_SetsStatusCode()
        {
            // Act
            NotImplementedAction action = new NotImplementedAction();

            // Assert
            Assert.AreEqual(HttpStatusCode.NotImplemented, action.StatusCode);
        }

        [TestMethod]
        public void Execute_WithNullContext_Throws()
        {
            // Arrange
            NotImplementedAction action = new NotImplementedAction();
            IRewriteContext context = null;

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => action.Execute(context));
        }

        [TestMethod]
        public void Execute_SetsStatusCode_ReturnsStopProcessing()
        {
            // Arrange
            NotImplementedAction action = new NotImplementedAction();
            IRewriteContext context = new MockRewriteContext();

            // Act
            RewriteProcessing result = action.Execute(context);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotImplemented, context.StatusCode);
            Assert.AreEqual(RewriteProcessing.StopProcessing, result);
        }
    }
}
