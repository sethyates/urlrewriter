using System;
using Intelligencia.UrlRewriter.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Intelligencia.UrlRewriter.Actions.Tests
{
    [TestClass]
    public class RewriteActionTest
    {
        [TestMethod]
        public void Constructor_WithNullLocation_Throws()
        {
            // Arrange
            string location = null;
            RewriteProcessing processing = RewriteProcessing.StopProcessing;

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => new RewriteAction(location, processing));
        }

        [TestMethod]
        public void Execute_WithNullContext_Throws()
        {
            // Arrange
            RewriteAction action = new RewriteAction("/", RewriteProcessing.RestartProcessing);
            IRewriteContext context = null;

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => action.Execute(context));
        }

        [TestMethod]
        public void Execute_SetsLocation_ReturnsCorrectValue()
        {
            // Arrange
            string location = "/NewLocation";
            RewriteProcessing processing = RewriteProcessing.RestartProcessing;
            RewriteAction action = new RewriteAction(location, processing);
            action.Conditions.Add(new MockRewriteCondition(true));
            IRewriteContext context = new MockRewriteContext();

            // Act
            RewriteProcessing result = action.Execute(context);

            // Assert
            Assert.AreEqual(processing, result);
            Assert.AreEqual(location, context.Location);
        }

        [TestMethod]
        public void IsMatch_WhenNoConditions_ReturnsTrue()
        {
            // Arrange
            RewriteAction action = new RewriteAction("/", RewriteProcessing.RestartProcessing);
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
            RewriteAction action = new RewriteAction("/", RewriteProcessing.RestartProcessing);
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
            RewriteAction action = new RewriteAction("/", RewriteProcessing.RestartProcessing);
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
            RewriteAction action = new RewriteAction("/", RewriteProcessing.RestartProcessing);
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
            RewriteAction action = new RewriteAction("/", RewriteProcessing.RestartProcessing);
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
