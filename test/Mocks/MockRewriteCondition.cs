using System;

namespace Intelligencia.UrlRewriter.Mocks
{
    /// <summary>
    /// A mock IRewriteCondition
    /// </summary>
    public class MockRewriteCondition : IRewriteCondition
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="result">The IRewriteCondition.IsMatch result</param>
        public MockRewriteCondition(bool result)
        {
            _result = result;
        }

        /// <summary>
        /// Mock implementation of IRewriteCondition.IsMatch
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool IsMatch(IRewriteContext context)
        {
            return _result;
        }

        private bool _result;
    }
}
