using System.Collections.Generic;

namespace log4net.Extensions
{
    public class LogContext : ILogContext
    {
        private readonly List<string> _keys = new List<string>();
 
        public ILogContext With<T>(string key, T value)
        {
            _keys.Add(key);
            LogicalThreadContext.Properties[key] = value.ToString();
            return this;
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