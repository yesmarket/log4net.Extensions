using System.Collections.Generic;
using System.IO;
using log4net.Appender;

namespace log4net.Extensions.Tests.Extensions
{
    public static class MemoryAppenderExtensions
    {
        public static IEnumerable<string> GetRenderedMessages(this MemoryAppender memoryAppender)
        {
            var loggingEvents = memoryAppender.GetEvents();
            foreach (var loggingEvent in loggingEvents)
            {
                var stringWriter = new StringWriter();
                memoryAppender.Layout.Format(stringWriter, loggingEvent);
                var renderedMessages = stringWriter.ToString();
                stringWriter.Dispose();

                yield return renderedMessages;
            }
        }
    }
}