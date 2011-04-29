using System;
using Intelligencia.UrlRewriter.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Intelligencia.UrlRewriter.Actions.Tests
{
    [TestClass]
    public class ConditionalActionTest
    {
        [TestMethod]
        public void Execute_WithNullContext_Throws()
        {
            // Arrange
            ConditionalAction action = new ConditionalAction();
            IRewriteContext context = null;

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => action.Execute(context));
        }

        [TestMethod]
        public void Execute_WhenNoActionsOrConditions_ReturnsContinueProcessing()
        {
            // Arrange
            ConditionalAction action = new ConditionalAction();
            IRewriteContext context = new MockRewriteContext();

            // Act
            RewriteProcessing result = action.Execute(context);

            // Assert
            Assert.AreEqual(RewriteProcessing.ContinueProcessing, result);
        }

        [TestMethod]
        public void Execute_WhenConditionAndAction_ReturnsExpectedResult()
        {
            // Arrange
            ConditionalAction action = new ConditionalAction();
            IRewriteContext context = new MockRewriteContext();
            action.Conditions.Add(new MockRewriteCondition(true));
            action.Actions.Add(new MockRewriteAction(RewriteProcessing.ContinueProcessing));

            // Act
            RewriteProcessing result = action.Execute(context);

            // Assert
            Assert.AreEqual(RewriteProcessing.ContinueProcessing, result);
        }

        [TestMethod]
        public void IsMatch_WhenNoConditions_ReturnsTrue()
        {
            // Arrange
            ConditionalAction action = new ConditionalAction();
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
            ConditionalAction action = new ConditionalAction();
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
            ConditionalAction action = new ConditionalAction();
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
            ConditionalAction action = new ConditionalAction();
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
            ConditionalAction action = new ConditionalAction();
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
