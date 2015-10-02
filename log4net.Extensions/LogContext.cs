using System.Collections.Generic;
using System.Linq;

namespace log4net.Extensions
{
    public class LogContext : ILogContext
    {
        private readonly List<string> _keys = new List<string>();

        private ILogContext With<T>(string key, T value)
        {
            _keys.Add(key);
            LogicalThreadContext.Properties[key] = value.ToString();
            return this;
        }

        private ILogContext With<T>(params KeyValuePair<string, T>[] contexts)
        {
            if (contexts == null) return this;
            foreach (var context in contexts)
            {
                With(context.Key, context.Value);
            }
            return this;
        }

        public ILogContext With(IContextProvider contextProvider)
        {
            var keyValuePair = contextProvider.GetContext();
            return With(keyValuePair.Key, keyValuePair.Value);
        }

        public ILogContext With(IList<IContextProvider> contextProviders)
        {
            var keyValuePairs = contextProviders.Select(provider => provider.GetContext());
            return With(keyValuePairs.ToArray());
        }

        public void Dispose()
        {
            foreach (var key in _keys)
            {
                LogicalThreadContext.Properties.Remove(key);
            }
        }
    }
}