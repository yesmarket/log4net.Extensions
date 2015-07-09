using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using log4net.Extensions;

namespace log4net.AutoFac
{
    public class LogContextInterceptor : IInterceptor
    {
        private readonly KeyValuePair<string, object>[] _contexts;

        public LogContextInterceptor(string key, object value)
        {
            _contexts = new[] {new KeyValuePair<string, object>(key, value)};
        }

        public LogContextInterceptor(params KeyValuePair<string, object>[] contexts)
        {
            _contexts = contexts;
        }

        public void Intercept(IInvocation invocation)
        {
            if (_contexts != null && _contexts.Any())
            {
                using ((new LogContext()).With(_contexts))
                {
                    invocation.Proceed();
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}
