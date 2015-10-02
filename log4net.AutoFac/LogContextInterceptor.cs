using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using log4net.Extensions;

namespace log4net.AutoFac
{
    public class LogContextInterceptor : IInterceptor
    {
        private readonly IList<IContextProvider> _contexts;

        public LogContextInterceptor() 
            : this(new DefaultContextProvider())
        {
        }

        public LogContextInterceptor(IContextProvider contextProvider)
            : this(new[] {contextProvider})
        {
        }

        public LogContextInterceptor(params IContextProvider[] contexts)
        {
            _contexts = contexts.ToList();
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