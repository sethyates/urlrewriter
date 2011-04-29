using System;
using System.Collections.Specialized;
using Intelligencia.UrlRewriter.Configuration;

namespace Intelligencia.UrlRewriter.Mocks
{
    /// <summary>
    /// A mock IConfigurationManager.
    /// </summary>
    public class MockConfigurationManager : IConfigurationManager
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public MockConfigurationManager()
            : this(null, null)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="appSettings">The IConfigurationManager.AppSettings collection</param>
        public MockConfigurationManager(NameValueCollection appSettings)
            : this(null, appSettings)
        {
        }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="section">The IConfigurationManager.GetSection result</param>
        public MockConfigurationManager(object section)
            : this(section, null)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="section">The IConfigurationManager.GetSection result</param>
        /// <param name="appSettings">The IConfigurationManager.AppSettings collection</param>
        public MockConfigurationManager(object section, NameValueCollection appSettings)
        {
            _section = section;
            _appSettings = appSettings ?? new NameValueCollection();
        }

        /// <summary>
        /// Mock implementation of IConfigurationManager.GetSection
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public object GetSection(string sectionName)
        {
            return _section;
        }

        /// <summary>
        /// Mock implementation of IConfigurationManager.AppSettings
        /// </summary>
        public NameValueCollection AppSettings
        {
            get { return _appSettings; }
        }

        private object _section;
        private NameValueCollection _appSettings;
    }
}
