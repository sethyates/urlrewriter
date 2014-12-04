using System;
using System.Net;
using Intelligencia.UrlRewriter.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Intelligencia.UrlRewriter.Actions.Tests
{
    [TestClass]
    public class GoneActionTest
    {
        [TestMethod]
        public void Constructor_SetsStatusCode()
        {
            // Act
            GoneAction action = new GoneAction();

            // Assert
            Assert.AreEqual(HttpStatusCode.Gone, action.StatusCode);
        }

        [TestMethod]
        public void Execute_WithNullContext_Throws()
        {
            // Arrange
            GoneAction action = new GoneAction();
            IRewriteContext context = null;

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => action.Execute(context));
        }

        [TestMethod]
        public void Execute_SetsStatusCode_ReturnsStopProcessing()
        {
            // Arrange
            GoneAction action = new GoneAction();
            IRewriteContext context = new MockRewriteContext();

            // Act
            RewriteProcessing result = action.Execute(context);

            // Assert
            Assert.AreEqual(HttpStatusCode.Gone, context.StatusCode);
            Assert.AreEqual(RewriteProcessing.StopProcessing, result);
        }
    }
}
