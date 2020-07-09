using Microsoft.Extensions.Logging;

namespace Consoleable.SelfHosted
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