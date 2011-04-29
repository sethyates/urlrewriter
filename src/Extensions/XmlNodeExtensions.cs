// UrlRewriter - A .NET URL Rewriter module
// Version 2.0
//
// Copyright 2011 Intelligencia
// Copyright 2011 Seth Yates
// 

using System;
using System.Configuration;
using System.Xml;
using Intelligencia.UrlRewriter.Utilities;

namespace Intelligencia.UrlRewriter
{
    /// <summary>
    /// Extension methods for the XmlNode class.
    /// </summary>
    internal static class XmlNodeExtensions
    {
        /// <summary>
        /// Gets a required attribute from an XML node.
        /// Throws an error if the required attribute is missing or empty (blank).
        /// </summary>
        /// <param name="node">The XML node</param>
        /// <param name="attributeName">The XML attribute name</param>
        /// <returns>The attribute value</returns>
        public static string GetRequiredAttribute(this XmlNode node, string attributeName)
        {
            return node.GetRequiredAttribute(attributeName, false);
        }

        /// <summary>
        /// Gets a required attribute from an XML node.
        /// Throws an error if the required attribute is missing.
        /// Throws an error if the required attribute is empty (blank) and allowBlank is set to false.
        /// or empty (blank).
        /// </summary>
        /// <param name="node">The XML node</param>
        /// <param name="attributeName">The XML attribute name</param>
        /// <param name="allowBlank">Blank (empty) values okay?</param>
        /// <returns>The attribute value</returns>
        public static string GetRequiredAttribute(this XmlNode node, string attributeName, bool allowBlank)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            XmlNode attribute = node.Attributes.GetNamedItem(attributeName);
            if (attribute == null)
            {
                throw new ConfigurationErrorsException(MessageProvider.FormatString(Message.AttributeRequired, attributeName), node);
            }

            if (attribute.Value.Length == 0 && !allowBlank)
            {
                throw new ConfigurationErrorsException(MessageProvider.FormatString(Message.AttributeCannotBeBlank, attributeName), node);
            }

            return attribute.Value;
        }
        
        public static string GetOptionalAttribute(this XmlNode node, string attributeName)
        {
            return node.GetOptionalAttribute(attributeName, null);
        }

        public static string GetOptionalAttribute(this XmlNode node, string attributeName, string defaultValue)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            XmlNode attribute = node.Attributes.GetNamedItem(attributeName);
            if (attribute == null)
            {
                return defaultValue;
            }

            return attribute.Value;
        }
    }
}
