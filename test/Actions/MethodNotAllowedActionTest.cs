using System;
using System.Net;
using Intelligencia.UrlRewriter.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Intelligencia.UrlRewriter.Actions.Tests
{
    [TestClass]
    public class MethodNotAllowedActionTest
    {
        [TestMethod]
        public void Constructor_SetsStatusCode()
        {
            // Act
            MethodNotAllowedAction action = new MethodNotAllowedAction();

            // Assert
            Assert.AreEqual(HttpStatusCode.MethodNotAllowed, action.StatusCode);
        }

        [TestMethod]
        public void Execute_WithNullContext_Throws()
        {
            // Arrange
            MethodNotAllowedAction action = new MethodNotAllowedAction();
            IRewriteContext context = null;

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => action.Execute(context));
        }

        [TestMethod]
        public void Execute_SetsStatusCode_ReturnsStopProcessing()
        {
            // Arrange
            MethodNotAllowedAction action = new MethodNotAllowedAction();
            IRewriteContext context = new MockRewriteContext();

            // Act
            RewriteProcessing result = action.Execute(context);

            // Assert
            Assert.AreEqual(HttpStatusCode.MethodNotAllowed, context.StatusCode);
            Assert.AreEqual(RewriteProcessing.StopProcessing, result);
        }
    }
}
