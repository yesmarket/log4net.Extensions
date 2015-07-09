using log4net.Appender;
using log4net.Core;
using log4net.Repository;
using log4net.Repository.Hierarchy;

namespace log4net.Extensions.Tests.Extensions
{
    public static class LoggerRepositoryExtensions
    {
        public static void InjectMemoryAppender(this ILoggerRepository value, MemoryAppender memoryAppender)
        {
            var hierarchy = (Hierarchy)value;
            hierarchy.Root.AddAppender(memoryAppender);
            hierarchy.Root.Level = Level.Debug;
            hierarchy.Configured = true;
        }
    }
}