using System;
using Intelligencia.UrlRewriter.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Intelligencia.UrlRewriter.Actions.Tests
{
    [TestClass]
    public class SetAppSettingPropertyActionTest
    {
        [TestMethod]
        public void Constructor_WithNullPropertyName_Throws()
        {
            // Arrange
            string propertyName = null;
            string appSettingKey = "AppSettingKey";

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => new SetAppSettingPropertyAction(propertyName, appSettingKey));
        }

        [TestMethod]
        public void Constructor_WithNullAppSettingKey_Throws()
        {
            // Arrange
            string propertyName = "PropertyName";
            string appSettingKey = null;

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => new SetAppSettingPropertyAction(propertyName, appSettingKey));
        }

        [TestMethod]
        public void Constructor_SetsNameAppSettingKey()
        {
            // Arrange
            string propertyName = "PropertyName";
            string appSettingKey = "AppSettingKey";

            // Act
            SetAppSettingPropertyAction action = new SetAppSettingPropertyAction(propertyName, appSettingKey);

            // Assert
            Assert.AreEqual(propertyName, action.Name);
            Assert.AreEqual(appSettingKey, action.AppSettingKey);
        }

        [TestMethod]
        public void Execute_WithNullContext_Throws()
        {
            // Arrange
            string propertyName = "PropertyName";
            string appSettingKey = "AppSettingKey";
            SetAppSettingPropertyAction action = new SetAppSettingPropertyAction(propertyName, appSettingKey);
            IRewriteContext context = null;

            // Act/Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => action.Execute(context));
        }

        [TestMethod]
        public void Execute_SetsPropertyAppSetting_ReturnsContinueProcessing()
        {
            // Arrange
            string propertyName = "PropertyName";
            string appSettingKey = "AppSettingKey";
            string appSettingValue = "AppSettingValue";
            SetAppSettingPropertyAction action = new SetAppSettingPropertyAction(propertyName, appSettingKey);
            IRewriteContext context = new MockRewriteContext();
            context.ConfigurationManager.AppSettings[appSettingKey] = appSettingValue;

            // Act
            RewriteProcessing result = action.Execute(context);

            // Assert
            Assert.AreEqual(RewriteProcessing.ContinueProcessing, result);
            CollectionAssert.Contains(context.Properties.Keys, propertyName);
            Assert.AreEqual(appSettingValue, context.Properties[propertyName]);
        }

        [TestMethod]
        public void Execute_WhenMissingAppSetting_SetsPropertyToEmptyString_ReturnsContinueProcessing()
        {
            // Arrange
            string propertyName = "PropertyName";
            string appSettingKey = "MissingAppSettingKey";
            SetAppSettingPropertyAction action = new SetAppSettingPropertyAction(propertyName, appSettingKey);
            IRewriteContext context = new MockRewriteContext();

            // Act
            RewriteProcessing result = action.Execute(context);

            // Assert
            Assert.AreEqual(RewriteProcessing.ContinueProcessing, result);
            CollectionAssert.Contains(context.Properties.Keys, propertyName);
            Assert.AreEqual(String.Empty, context.Properties[propertyName]);
        }
    }
}
