using System;
using log4net.Layout;
using Ploeh.AutoFixture.Kernel;

namespace log4net.Extensions.Tests.Helpers
{
    public class PatternLayoutSpecimenBuilder : ISpecimenBuilder
    {
        private readonly string _pattern;

        public PatternLayoutSpecimenBuilder(string pattern)
        {
            _pattern = pattern;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var t = request as Type;
            if (typeof(PatternLayout) != t) return new NoSpecimen(request);

            var patternLayout = new PatternLayout
            {
                ConversionPattern = _pattern
            };
            patternLayout.ActivateOptions();

            return patternLayout;
        }
    }
}