using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Extras.DynamicProxy2;
using log4net.Appender;
using log4net.Extensions.Tests.Extensions;
using log4net.Extensions.Tests.Helpers;
using log4net.Layout;
using Ploeh.AutoFixture;
using SharpTestsEx;
using Xunit;

namespace log4net.AutoFac.Tests
{
    public class LogContextInterceptorTests
    {
        private readonly IContainer _container;
        private readonly Guid _context;
        private readonly MemoryAppender _memoryAppender;

        public LogContextInterceptorTests()
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(new PatternLayoutSpecimenBuilder("%message %property{context}"));
            fixture.Customizations.Add(new MemoryAppenderSpecimenBuilder(fixture.Create<PatternLayout>()));

            _memoryAppender = fixture.Create<MemoryAppender>();
            LogManager.GetRepository().InjectMemoryAppender(_memoryAppender);

            _context = Guid.NewGuid();
            var builder = new ContainerBuilder();
            builder.RegisterModule(new LoggingModule());

            builder.RegisterType<LogContextInterceptor>()
                .WithParameter(
                    new ResolvedParameter(
                        (info, context) =>
                            info.ParameterType == typeof(KeyValuePair<string, object>[]),
                        (info, context) => new[] { new KeyValuePair<string, object>("context", _context) }));

            builder
                .RegisterType<Test>()
                .As<ITest>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(LogContextInterceptor));

            _container = builder.Build();
        }

        [Fact]
        public void Test()
        {
            var sut1 = _container.Resolve<ITest>();
            sut1.DoSomething();

            var loggingEvents = _memoryAppender.GetRenderedMessages().ToList();
            loggingEvents[0].Should().Be.EqualTo(string.Format("test {0}", _context));
        }
    }

    #region Mocks
    public interface ITest
    {
        void DoSomething();
    }

    public class Test : ITest
    {
        private readonly ILog _logger;

        public Test(ILog logger)
        {
            _logger = logger;
        }

        public virtual void DoSomething()
        {
            _logger.Debug("test");
        }
    } 
    #endregion
}
