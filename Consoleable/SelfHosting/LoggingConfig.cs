using Microsoft.Extensions.Logging;

namespace Consoleable.SelfHosting
{
    class LoggingConfig
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Debug;
        /// <summary>
        /// An instance of <see cref="LoggingConfig"/>.
        /// When this assembly is used as a self-hosted console command, <see cref="Startup.Configure"/> will
        /// configure this from an <c>appSettings.json</c> file, if found.
        /// Otherwise, it defaults to <c>LogLevel.Debug</c>
        /// </summary>
        public static readonly LoggingConfig FromConfig = new LoggingConfig();
        
        /// <summary>
        /// When configuring <see cref="FromConfig"/> from an <c>appSettings.json</c> file, look in the
        /// Section with this name for the <see cref="LogLevel"/> to set.
        /// </summary>
        public const string AppSettingsSectionName = "Logging";
    }
}