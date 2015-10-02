using System.Collections.Generic;

namespace log4net.Extensions
{
    public interface IContextProvider
    {
        KeyValuePair<string, object> GetContext();
    }
}