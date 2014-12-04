using System;
using System.Net;
using Intelligencia.UrlRewriter.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Intelligencia.UrlRewriter.Actions.Tests
{
    [TestClass]
    public class ForbiddenActionTest
    {
        [TestMethod]
        public void Constructor_SetsStatusCode()
        {
            // Act
            ForbiddenAction action = new ForbiddenAction();

            // Assert
            Assert.AreEqual(HttpStatusCode.Forbidden, action.StatusCode);
        }

        [TestMethod]
        public void Execute_WithNullContext_Throws()
        {
            // Arrange
            ForbiddenAction action = new ForbiddenAction();
            IRewriteContext context = null;

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => action.Execute(context));
        }

        [TestMethod]
        public void Execute_SetsStatusCode_ReturnsStopProcessing()
        {
            // Arrange
            ForbiddenAction action = new ForbiddenAction();
            IRewriteContext context = new MockRewriteContext();

            // Act
            RewriteProcessing result = action.Execute(context);

            // Assert
            Assert.AreEqual(HttpStatusCode.Forbidden, context.StatusCode);
            Assert.AreEqual(RewriteProcessing.StopProcessing, result);
        }
    }
}
