using System;
using System.Collections.Generic;

namespace log4net.Extensions
{
    public interface ILogContext : IDisposable
    {
        ILogContext With(IContextProvider contextProvider);
        ILogContext With(IList<IContextProvider> contextProviders);
    }
}