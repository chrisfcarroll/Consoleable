#if serilog
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;

namespace Consoleable.SelfHosted
{
    static class UseSerilog
    {
        public static ILoggerFactory GetFactory()
        {
            return new SerilogLoggerFactory(
                Log.Logger = 
                    new LoggerConfiguration()
                        .MinimumLevel.Is(LogEventLevel.Verbose)
                        .Enrich.FromLogContext()
                        .WriteTo.ColoredConsole()
                        .CreateLogger());
        }
    }
    
    static class MsLoggingToSerilogLogLevel
    {
        public static LogEventLevel ToSerilogEventLevel(this LogLevel msLevel)
        {
            switch (msLevel)
            {
                case LogLevel.Critical : return LogEventLevel.Fatal;
                case LogLevel.Error : return LogEventLevel.Error;
                case LogLevel.Warning : return LogEventLevel.Warning;
                case LogLevel.Information : return LogEventLevel.Information;
                case LogLevel.Debug : return LogEventLevel.Debug;
                case LogLevel.Trace : return LogEventLevel.Verbose;
                default:return (LogEventLevel) int.MaxValue;
            }
        }
    }
}
#endif
