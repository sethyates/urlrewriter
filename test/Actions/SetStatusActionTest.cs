using System;
using System.Net;
using Intelligencia.UrlRewriter.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Intelligencia.UrlRewriter.Actions.Tests
{
    [TestClass]
    public class SetStatusActionTest
    {
        [TestMethod]
        public void Constructor_SetsStatusCode()
        {
            // Arrange
            HttpStatusCode code = HttpStatusCode.InternalServerError;

            // Act
            SetStatusAction action = new SetStatusAction(code);

            // Assert
            Assert.AreEqual(code, action.StatusCode);
        }
        
        [TestMethod]
        public void Execute_WithNullContext_Throws()
        {
            // Arrange
            SetStatusAction action = new SetStatusAction(HttpStatusCode.OK);
            IRewriteContext context = null;

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => action.Execute(context));
        }

        [TestMethod]
        public void Execute_WhenStatusCodeAccepted_SetsStatusCode_ReturnsContinueProcessing()
        {
            // Arrange
            HttpStatusCode code = HttpStatusCode.Accepted;
            SetStatusAction action = new SetStatusAction(code);
            IRewriteContext context = new MockRewriteContext();

            // Act
            RewriteProcessing result = action.Execute(context);

            // Assert
            Assert.AreEqual(RewriteProcessing.ContinueProcessing, result);
            Assert.AreEqual(code, context.StatusCode);
        }

        [TestMethod]
        public void Execute_WhenStatusCodeError_SetsStatusCode_ReturnsStopProcessing()
        {
            // Arrange
            HttpStatusCode code = HttpStatusCode.InternalServerError;
            SetStatusAction action = new SetStatusAction(code);
            IRewriteContext context = new MockRewriteContext();

            // Act
            RewriteProcessing result = action.Execute(context);

            // Assert
            Assert.AreEqual(RewriteProcessing.StopProcessing, result);
            Assert.AreEqual(code, context.StatusCode);
        }
    }
}
