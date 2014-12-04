using System;
using System.Net;
using Intelligencia.UrlRewriter.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Intelligencia.UrlRewriter.Actions.Tests
{
    [TestClass]
    public class NotFoundActionTest
    {
        [TestMethod]
        public void Constructor_SetsStatusCode()
        {
            // Act
            NotFoundAction action = new NotFoundAction();

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, action.StatusCode);
        }

        [TestMethod]
        public void Execute_WithNullContext_Throws()
        {
            // Arrange
            NotFoundAction action = new NotFoundAction();
            IRewriteContext context = null;

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => action.Execute(context));
        }

        [TestMethod]
        public void Execute_SetsStatusCode_ReturnsStopProcessing()
        {
            // Arrange
            NotFoundAction action = new NotFoundAction();
            IRewriteContext context = new MockRewriteContext();

            // Act
            RewriteProcessing result = action.Execute(context);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, context.StatusCode);
            Assert.AreEqual(RewriteProcessing.StopProcessing, result);
        }
    }
}
