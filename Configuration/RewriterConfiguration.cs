// UrlRewriter - A .NET URL Rewriter module
// Version 2.0
//
// Copyright 2011 Intelligencia
// Copyright 2011 Seth Yates
// 

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using System.Collections.Specialized;
using System.Reflection;
using Intelligencia.UrlRewriter.Parsers;
using Intelligencia.UrlRewriter.Transforms;
using Intelligencia.UrlRewriter.Utilities;
using Intelligencia.UrlRewriter.Logging;

namespace Intelligencia.UrlRewriter.Configuration
{
    /// <summary>
    /// Configuration for the URL rewriter.
    /// </summary>
    public class RewriterConfiguration : IRewriterConfiguration
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public RewriterConfiguration()
            : this(new ConfigurationManagerFacade())
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="configurationManager">The configuration manager instance</param>
        public RewriterConfiguration(IConfigurationManager configurationManager)
        {
            if (configurationManager == null)
            {
                throw new ArgumentNullException("configurationManager");
            }

            _configurationManager = configurationManager;

            _xPoweredBy = MessageProvider.FormatString(Message.ProductName, Assembly.GetExecutingAssembly().GetName().Version.ToString(3));

            _actionParserFactory = new ActionParserFactory();
            _actionParserFactory.AddParser(new IfConditionActionParser());
            _actionParserFactory.AddParser(new UnlessConditionActionParser());
            _actionParserFactory.AddParser(new AddHeaderActionParser());
            _actionParserFactory.AddParser(new SetCookieActionParser());
            _actionParserFactory.AddParser(new SetPropertyActionParser());
            _actionParserFactory.AddParser(new SetAppSettingPropertyActionParser());
            _actionParserFactory.AddParser(new RewriteActionParser());
            _actionParserFactory.AddParser(new RedirectActionParser());
            _actionParserFactory.AddParser(new SetStatusActionParser());
            _actionParserFactory.AddParser(new ForbiddenActionParser());
            _actionParserFactory.AddParser(new GoneActionParser());
            _actionParserFactory.AddParser(new NotAllowedActionParser());
            _actionParserFactory.AddParser(new NotFoundActionParser());
            _actionParserFactory.AddParser(new NotImplementedActionParser());

            _conditionParserPipeline = new ConditionParserPipeline();
            _conditionParserPipeline.AddParser(new AddressConditionParser());
            _conditionParserPipeline.AddParser(new HeaderMatchConditionParser());
            _conditionParserPipeline.AddParser(new MethodConditionParser());
            _conditionParserPipeline.AddParser(new PropertyMatchConditionParser());
            _conditionParserPipeline.AddParser(new ExistsConditionParser());
            _conditionParserPipeline.AddParser(new UrlMatchConditionParser());

            _transformFactory = new TransformFactory();
            _transformFactory.AddTransform(new DecodeTransform());
            _transformFactory.AddTransform(new EncodeTransform());
            _transformFactory.AddTransform(new LowerTransform());
            _transformFactory.AddTransform(new UpperTransform());
            _transformFactory.AddTransform(new Base64Transform());
            _transformFactory.AddTransform(new Base64DecodeTransform());

            _defaultDocuments = new StringCollection();

            LoadFromConfig();
        }

        /// <summary>
        /// The rules.
        /// </summary>
        public IList<IRewriteAction> Rules
        {
            get { return _rules; }
        }

        /// <summary>
        /// The action parser factory.
        /// </summary>
        public ActionParserFactory ActionParserFactory
        {
            get { return _actionParserFactory; }
        }

        /// <summary>
        /// The transform factory.
        /// </summary>
        public TransformFactory TransformFactory
        {
            get { return _transformFactory; }
        }

        /// <summary>
        /// The condition parser pipeline.
        /// </summary>
        public ConditionParserPipeline ConditionParserPipeline
        {
            get { return _conditionParserPipeline; }
        }

        /// <summary>
        /// Dictionary of error handlers.
        /// </summary>
        public IDictionary<int, IRewriteErrorHandler> ErrorHandlers
        {
            get { return _errorHandlers; }
        }

        /// <summary>
        /// Logger to use for logging information.
        /// </summary>
        public IRewriteLogger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        /// <summary>
        /// Collection of default document names to use if the result of a rewriting
        /// is a directory name.
        /// </summary>
        public StringCollection DefaultDocuments
        {
            get { return _defaultDocuments; }
        }

        /// <summary>
        /// Additional X-Powered-By header.
        /// </summary>
        public string XPoweredBy
        {
            get { return _xPoweredBy; }
        }

        /// <summary>
        /// The configuration manager instance.
        /// </summary>
        public IConfigurationManager ConfigurationManager
        {
            get { return _configurationManager; }
        }

        /// <summary>
        /// Loads the rewriter configuration from the web.config file.
        /// </summary>
        private void LoadFromConfig()
        {
            XmlNode section = _configurationManager.GetSection(Constants.RewriterNode) as XmlNode;
            if (section == null)
            {
                throw new ConfigurationErrorsException(MessageProvider.FormatString(Message.MissingConfigFileSection, Constants.RewriterNode), section);
            }

            RewriterConfigurationReader.Read(this, section);
        }

        private IConfigurationManager _configurationManager;
        private IRewriteLogger _logger = new NullLogger();
        private IDictionary<int, IRewriteErrorHandler> _errorHandlers = new Dictionary<int, IRewriteErrorHandler>();
        private IList<IRewriteAction> _rules = new List<IRewriteAction>();
        private ActionParserFactory _actionParserFactory;
        private ConditionParserPipeline _conditionParserPipeline;
        private TransformFactory _transformFactory;
        private StringCollection _defaultDocuments;
        private string _xPoweredBy;
    }
}
