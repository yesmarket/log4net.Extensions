using System;
using System.Collections.Generic;

namespace log4net.Extensions
{
    public interface ILogContext : IDisposable
    {
        ILogContext With<T>(string key, T value);
        ILogContext With<T>(KeyValuePair<string, T>[] contexts);
    }
}