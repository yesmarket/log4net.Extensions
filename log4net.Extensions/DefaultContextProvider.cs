using System;
using System.Collections.Generic;

namespace log4net.Extensions
{
    public class DefaultContextProvider : IContextProvider
    {
        private readonly string _key;

        public DefaultContextProvider()
            : this(Constants.DefaultContextName)
        {
        }

        public DefaultContextProvider(string key)
        {
            _key = key;
        }

        public KeyValuePair<string, object> GetContext()
        {
            return new KeyValuePair<string, object>(_key, Guid.NewGuid());
        }
    }
}