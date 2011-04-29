using System;
using System.Net;
using Intelligencia.UrlRewriter.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Intelligencia.UrlRewriter.Actions.Tests
{
    [TestClass]
    public class RedirectActionTest
    {
        [TestMethod]
        public void Constructor_WithNullLocation_Throws()
        {
            // Arrange
            string location = null;
            bool permanent = true;

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => new RedirectAction(location, permanent));
        }

        [TestMethod]
        public void Execute_WithNullContext_Throws()
        {
            // Arrange
            RedirectAction action = new RedirectAction("/", true);
            IRewriteContext context = null;
            
            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => action.Execute(context));
        }

        [TestMethod]
        public void Execute_WhenPermanent_SetsStatusCodeAndLocation_ReturnsStopProcessing()
        {
            // Arrange
            string location = "/NewLocation";
            bool permanent = true;
            RedirectAction action = new RedirectAction(location, permanent);
            action.Conditions.Add(new MockRewriteCondition(true));
            IRewriteContext context = new MockRewriteContext();
            
            // Act
            RewriteProcessing result = action.Execute(context);

            // Assert
            Assert.AreEqual(RewriteProcessing.StopProcessing, result);
            Assert.AreEqual(HttpStatusCode.Moved, context.StatusCode);
            Assert.AreEqual(location, context.Location);
        }

        [TestMethod]
        public void Execute_WhenTemporary_SetsStatusCodeAndLocation_ReturnsStopProcessing()
        {
            // Arrange
            string location = "/NewLocation";
            bool permanent = false;
            RedirectAction action = new RedirectAction(location, permanent);
            action.Conditions.Add(new MockRewriteCondition(true));
            IRewriteContext context = new MockRewriteContext();

            // Act
            RewriteProcessing result = action.Execute(context);

            // Assert
            Assert.AreEqual(RewriteProcessing.StopProcessing, result);
            Assert.AreEqual(HttpStatusCode.Found, context.StatusCode);
            Assert.AreEqual(location, context.Location);
        }

        [TestMethod]
        public void IsMatch_WhenNoConditions_ReturnsTrue()
        {
            // Arrange
            RedirectAction action = new RedirectAction("/", true);
            IRewriteContext context = new MockRewriteContext();

            // Act
            bool match = action.IsMatch(context);

            // Assert
            Assert.IsTrue(match);
        }

        [TestMethod]
        public void IsMatch_WhenSingleConditionMatches_ReturnsTrue()
        {
            // Arrange
            RedirectAction action = new RedirectAction("/", true);
            IRewriteContext context = new MockRewriteContext();
            action.Conditions.Add(new MockRewriteCondition(true));

            // Act
            bool match = action.IsMatch(context);

            // Assert
            Assert.IsTrue(match);
        }

        [TestMethod]
        public void IsMatch_WhenSingleConditionDoesNotMatch_ReturnsFalse()
        {
            // Arrange
            RedirectAction action = new RedirectAction("/", true);
            IRewriteContext context = new MockRewriteContext();
            action.Conditions.Add(new MockRewriteCondition(false));

            // Act
            bool match = action.IsMatch(context);

            // Assert
            Assert.IsFalse(match);
        }

        [TestMethod]
        public void IsMatch_WhenMultipleMatchingConditions_ReturnsTrue()
        {
            // Arrange
            RedirectAction action = new RedirectAction("/", true);
            IRewriteContext context = new MockRewriteContext();
            action.Conditions.Add(new MockRewriteCondition(true));
            action.Conditions.Add(new MockRewriteCondition(true));

            // Act
            bool match = action.IsMatch(context);

            // Assert
            Assert.IsTrue(match);
        }

        [TestMethod]
        public void IsMatch_WhenMixedConditions_ReturnsFalse()
        {
            // Arrange
            RedirectAction action = new RedirectAction("/", true);
            IRewriteContext context = new MockRewriteContext();
            action.Conditions.Add(new MockRewriteCondition(true));
            action.Conditions.Add(new MockRewriteCondition(false));

            // Act
            bool match = action.IsMatch(context);

            // Assert
            Assert.IsFalse(match);
        }
    }
}
