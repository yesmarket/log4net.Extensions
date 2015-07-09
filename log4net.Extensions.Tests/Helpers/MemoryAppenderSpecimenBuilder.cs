using System;
using log4net.Appender;
using log4net.Layout;
using Ploeh.AutoFixture.Kernel;

namespace log4net.Extensions.Tests.Helpers
{
    public class MemoryAppenderSpecimenBuilder : ISpecimenBuilder
    {
        private readonly PatternLayout _patternLayout;

        public MemoryAppenderSpecimenBuilder(PatternLayout patternLayout)
        {
            _patternLayout = patternLayout;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var t = request as Type;
            if (typeof(MemoryAppender) != t) return new NoSpecimen(request);

            //var patternLayout = new PatternLayout
            //{
            //    ConversionPattern = "%message %property{context1} %property{context2}"
            //};
            _patternLayout.ActivateOptions();

            var memoryAppender = new MemoryAppender
            {
                Layout = _patternLayout
            };
            memoryAppender.ActivateOptions();

            return memoryAppender;
        }
    }
}