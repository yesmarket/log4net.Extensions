using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Extensions;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using SharpTestsEx;
using Xunit;

namespace test
{
    public class LogContextTests
    {
        private readonly LogContextFactory _logContextFactory;
        private readonly ILog _logger;
        private readonly MemoryAppender _memoryAppender;

        public LogContextTests()
        {
            var hierarchy = (Hierarchy)LogManager.GetRepository();

            var patternLayout = new PatternLayout
            {
                ConversionPattern = "%message %property{context1} %property{context2}"
            };
            patternLayout.ActivateOptions();

            _memoryAppender = new MemoryAppender
            {
                Layout = patternLayout
            };
            _memoryAppender.ActivateOptions();
            hierarchy.Root.AddAppender(_memoryAppender);

            hierarchy.Root.Level = Level.Debug;
            hierarchy.Configured = true;

            _logContextFactory = new LogContextFactory();
            _logger = LogManager.GetLogger(GetType());
        }

        [Fact]
        public async Task LogContextAsyncReentrancy()
        {
            using (_logContextFactory.New().With("context1", "x"))
            {
                _logger.Debug("a");
                await Task.Delay(1);
                _logger.Debug("b");
            }

            var loggingEvents = _memoryAppender.GetRenderedMessages().ToList();
            loggingEvents[0].Should().Be.EqualTo("a x (null)");
            loggingEvents[1].Should().Be.EqualTo("b x (null)");
        }

        [Fact]
        public async Task LogContextNestedAsyncReentrancy()
        {
            using (_logContextFactory.New().With("context1", "x"))
            {
                _logger.Debug("a");
                await Task.Run(async () =>
                {
                    using (_logContextFactory.New().With("context2", "y"))
                    {
                        _logger.Debug("c");
                        await Task.Delay(1);
                        _logger.Debug("d");
                    }
                });
                _logger.Debug("b");
            }

            var loggingEvents = _memoryAppender.GetRenderedMessages().ToList();
            loggingEvents[0].Should().Be.EqualTo("a x (null)");
            loggingEvents[1].Should().Be.EqualTo("c x y");
            loggingEvents[2].Should().Be.EqualTo("d x y");
            loggingEvents[3].Should().Be.EqualTo("b x (null)");
        }

        [Fact]
        public void LogContextMultiThreaded()
        {
            var autoResetEvent1 = new AutoResetEvent(false);
            var autoResetEvent2 = new AutoResetEvent(false);
            
            var thread1 =
                new Thread(() =>
                {
                    using (_logContextFactory.New().With("context1", "x"))
                    {
                        autoResetEvent2.WaitOne();
                        _logger.Debug("a");
                        autoResetEvent1.Set();
                    }
                });

            var thread2 =
                new Thread(() =>
                {
                    using (_logContextFactory.New().With("context2", "y"))
                    {
                        autoResetEvent2.Set();
                        autoResetEvent1.WaitOne();
                        _logger.Debug("b");
                    }
                });

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            var loggingEvents = _memoryAppender.GetRenderedMessages().ToList();
            loggingEvents[0].Should().Be.EqualTo("a x (null)");
            loggingEvents[1].Should().Be.EqualTo("b (null) y");
        }
    }
}
