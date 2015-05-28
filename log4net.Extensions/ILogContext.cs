using System;

namespace log4net.Extensions
{
    public interface ILogContext : IDisposable
    {
        ILogContext With<T>(string key, T value);
    }
}