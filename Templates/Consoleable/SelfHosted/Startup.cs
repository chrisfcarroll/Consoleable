using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Consoleable.SelfHosted
{
    /// <summary>
    /// When using <see cref="Consoleable"/> as a self-hosted console application, use <see cref="Configure"/>
    /// to initialize defaults for:
    /// <list type="bullet">
    /// <item>The <see cref="LoggerFactory"/></item>
    /// <item>The <see cref="Configuration"/></item>
    /// <item>The <see cref="Settings"/></item>
    /// </list>
    /// Which you can then access as public static fields.
    /// </summary>
    /// <remarks>
    /// When using <see cref="Consoleable"/> as an in-memory component, your hosting application would typically
    /// be responsible for these three things and this class is not required.
    /// </remarks>
    class Startup
    {
        /// <summary>
        /// When this assembly is used a self-hosted console application,
        /// <see cref="Configure"/> initialises from an <c>appSettings.json</c> file if
        /// one is found.
        /// </summary>
        static IConfigurationRoot Configuration;
        
        /// <summary>
        /// When this assembly is used a self-hosted console application, this
        /// ILoggerFactory will be used to create an ILogger to pass to
        /// <see cref="Consoleable(Microsoft.Extensions.Logging.ILogger,Consoleable.Settings)"/> 
        /// </summary>
        internal static ILoggerFactory LoggerFactory;

        /// <summary>
        /// When this assembly is used a self-hosted console application, this Settings will
        /// be passed to <see cref="Consoleable(Microsoft.Extensions.Logging.ILogger,Consoleable.Settings)"/> 
        /// </summary>
        public static Settings Settings= new Settings();

        /// <summary>
        /// Initialise three things during application startup from the console.
        /// <i>(When using <see cref="Consoleable"/> as an in memory component, your hosting application would typically
        /// be responsible for these three things and this class is not required.)</i>
        /// <list type="bullet">
        /// <item>The <see cref="LoggerFactory"/></item>
        /// <item>The <see cref="Configuration"/></item>
        /// <item>The <see cref="Settings"/></item>
        /// </list>
        /// Default behaviour is:
        /// <list type="bullet">
        /// <item>use <see cref="FallbackLoggerFactory"/> which writes everything to <see cref="Console.Out"/>.</item>
        /// <item>Look for an <c>appSettings.json</c> in the same directory as this assembly, else create an
        /// empty Configuration object</item>
        /// <item>Look in <see cref="Configuration"/> for a <see cref="ConfigurationSection"/> named
        /// <paramref name="appSettingsSectionName"/>, which defaults to <c>nameof(<see cref="Consoleable"/>)</c>
        /// </item>
        /// </list>
        /// </summary>
        /// <returns>Can be changed to return a fluently usable
        /// <see cref="FluentStartupAccessor"/> of the otherwise static
        /// <see cref="Startup"/> settings.
        /// </returns>
        public static void Configure()
        {
            LoggerFactory= ChooseLogger.Choose();
            var startupLog = LoggerFactory.CreateLogger<Startup>();
            Configuration= ReadConfigurationFromAppsettings(startupLog);
            Settings=ReadSettingsFromConfiguration(startupLog);
        }
        
        static Settings ReadSettingsFromConfiguration(ILogger startupLog)
        {
            const string appSettingsSectionName = nameof(Consoleable);
            var settings = new Settings();
            try
            {
                Configuration.GetSection(appSettingsSectionName).Bind(settings);
            }
            catch (Exception e)
            {
                startupLog.LogError(e, "Failed to bind settings from " +
                                       $"Configuration {appSettingsSectionName}");
            }
            return settings;
        }

        static IConfigurationRoot ReadConfigurationFromAppsettings(ILogger startupLog)
        {
            IConfigurationRoot configuration;
            string appsettingsPath=null;
            try
            {
                var startupLocation = Path.GetDirectoryName(typeof(Startup).Assembly.Location) ?? ".";
                appsettingsPath = Path.Combine(startupLocation, "appsettings.json");
                if (File.Exists(appsettingsPath))
                {
                    configuration = new ConfigurationBuilder()
                        .SetBasePath(startupLocation)
                        .AddJsonFile("appsettings.json", false)
                        .Build();
                    startupLog.LogDebug($"Read Configuration from {appsettingsPath} ");
                }
                else
                {
                    configuration = new ConfigurationBuilder().Build();
                    startupLog.LogWarning("No appsettings.json file found for configuration");
                }
            }
            catch (Exception e)
            {
                startupLog.LogError(e, $"At Startup, attempting to read Configuration" +
                                       $" from {appsettingsPath}");
                throw;
            }

            return configuration;
        }
        
        /// <summary>
        /// An optional convenience instance of the otherwise static <see cref="Startup"/>
        /// which you can use to allow <see cref="Startup.Configure"/> to be called in fluent style. 
        /// </summary>
        class FluentStartupAccessor : Startup
        {
            // ReSharper disable MemberHidesStaticFromOuterClass
            public new IConfiguration Configuration => Startup.Configuration;
            public new ILoggerFactory LoggerFactory => Startup.LoggerFactory;
            public new Settings Settings => Startup.Settings;
        }
    }
}
