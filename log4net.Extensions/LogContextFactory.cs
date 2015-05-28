namespace log4net.Extensions
{
    public class LogContextFactory : ILogContextFactory
    {
        public ILogContext New()
        {
            return new LogContext();
        }
    }
}