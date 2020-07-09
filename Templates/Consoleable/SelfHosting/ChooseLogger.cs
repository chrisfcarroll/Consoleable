using Microsoft.Extensions.Logging;

namespace Consoleable.SelfHosting
{
    static class ChooseLogger
    {
        public static ILoggerFactory Choose()
        {
#if (serilog)
            return UseSerilog.GetFactory();
#else
            return new FallbackLoggerFactory();
#endif
        }
    }
}