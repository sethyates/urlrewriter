using System;

namespace Intelligencia.UrlRewriter.Mocks
{
    /// <summary>
    /// A mock IRewriteAction.
    /// </summary>
    public class MockRewriteAction : IRewriteAction
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="result">The IRewriteAction.Execute result</param>
        public MockRewriteAction(RewriteProcessing result)
        {
            _result = result;
        }

        /// <summary>
        /// Mock implementation of IRewriteAction.Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public RewriteProcessing Execute(IRewriteContext context)
        {
            return _result;
        }

        private RewriteProcessing _result;
    }
}
