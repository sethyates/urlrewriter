using System;
using Intelligencia.UrlRewriter.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Intelligencia.UrlRewriter.Actions.Tests
{
    [TestClass]
    public class SetCookieActionTest
    {
        [TestMethod]
        public void Constructor_WithNullCookieName_Throws()
        {
            // Arrange
            string cookieName = null;
            string cookieValue = "CookieValue";

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => new SetCookieAction(cookieName, cookieValue));
        }

        [TestMethod]
        public void Constructor_WithNullCookieValue_Throws()
        {
            // Arrange
            string cookieName = "CookieName";
            string cookieValue = null;

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => new SetCookieAction(cookieName, cookieValue));
        }

        [TestMethod]
        public void Constructor_SetsNameAndValue()
        {
            // Arrange
            string cookieName = "CookieName";
            string cookieValue = "CookieValue";

            // Act
            SetCookieAction action = new SetCookieAction(cookieName, cookieValue);

            // Assert
            Assert.AreEqual(cookieName, action.Name);
            Assert.AreEqual(cookieValue, action.Value);
        }

        [TestMethod]
        public void Execute_WithNullContext_Throws()
        {
            // Arrange
            SetCookieAction action = new SetCookieAction("CookieName", "CookieValue");
            IRewriteContext context = null;

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => action.Execute(context));
        }

        [TestMethod]
        public void Execute_SetsCookie_ReturnsContinueProcessing()
        {
            // Arrange
            string cookieName = "CookieName";
            string cookieValue = "CookieValue";
            SetCookieAction action = new SetCookieAction(cookieName, cookieValue);
            IRewriteContext context = new MockRewriteContext();

            // Act
            RewriteProcessing result = action.Execute(context);

            // Assert
            Assert.AreEqual(RewriteProcessing.ContinueProcessing, result);
            CollectionAssert.Contains(context.ResponseCookies.Keys, cookieName);
            Assert.AreEqual(cookieValue, context.ResponseCookies[cookieName].Value);
        }
    }
}
