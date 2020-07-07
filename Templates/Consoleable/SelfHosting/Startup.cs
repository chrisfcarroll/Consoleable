using System;
using System.IO;
using Consoleable.Dependencies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Consoleable.SelfHosting
{
    /// <summary>
    /// When using <see cref="Consoleable"/> as a self-hosted console application, use <see cref="Configure"/>
    /// to initialize defaults for:
    /// <list type="bullet">
    /// <item>The <see cref="LoggerFactory"/></item>
    /// <item>The <see cref="Configuration"/></item>
    /// <item>The <see cref="Settings"/></item>
    /// </list>
    /// Access them as public static fields. 
    /// </summary>
    /// <remarks>
    /// When using <see cref="Consoleable"/> as an in memory component, your hosting application would typically
    /// be responsible for these three things and this class is not required.
    /// </remarks>
    class Startup
    {
        /// <summary>
        /// Typically used to read an <c>appSettings.json</c> file, or other configuration source, in order to
        /// populate <see cref="LoggerFactory"/> and <see cref="Settings"/>.
        /// </summary>
        static IConfiguration Configuration;
        
        /// <summary>
        /// When this assembly is used a self-hosted console application, the ILoggerFactory that will be used
        /// to create an ILogger to pass to <see cref="Consoleable(Microsoft.Extensions.Logging.ILogger,Consoleable.SelfHosting.Settings)"/> 
        /// </summary>
        static ILoggerFactory LoggerFactory;

        /// <summary>
        /// When this assembly is used a self-hosted console application, the Settings that will be
        /// passed to <see cref="Consoleable(Microsoft.Extensions.Logging.ILogger,Consoleable.SelfHosting.Settings)"/> 
        /// </summary>
        public static Settings Settings= new Settings();
        
        /// <summary>Use <see cref="LoggerFactory"/> to create a logger</summary>
        public static ILogger CreateLogger<T>() { return LoggerFactory.CreateLogger<T>(); }
        /// <summary>Use <see cref="LoggerFactory"/> to create a logger</summary>
        public static ILogger CreateLogger(Type type) { return LoggerFactory.CreateLogger(type); }
        /// <summary>Use <see cref="LoggerFactory"/> to create a logger</summary>
        public static ILogger CreateLogger(string name) { return LoggerFactory.CreateLogger(name); }

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
        /// <param name="appSettingsSectionName">The section of the appSettings.json to look for
        /// component configuration</param>
        /// <param name="factory">Your preferred LoggerFactory. Defaults to <see cref="FallbackLoggerFactory"/>
        /// which will only create instances of <see cref="FallbackLogger"/>, which writes all messages to
        /// <see cref="Console.Out"/></param>
        /// <returns>A fluently usable <see cref="Instance"/> of the
        /// otherwise static <see cref="Startup"/> settings.
        /// </returns>
        public static Instance Configure(string appSettingsSectionName=nameof(Consoleable), ILoggerFactory factory=null)
        {
            LoggerFactory = factory?? new FallbackLoggerFactory();
            var startupLogger = LoggerFactory.CreateLogger("StartUp");

            appSettingsSectionName ??= nameof(Consoleable);
            var startupLocation = Path.GetDirectoryName(typeof(Startup).Assembly.Location)??".";
            var appsettingsPath = Path.Combine(startupLocation, "appsettings.json");
            if (File.Exists(appsettingsPath))
            {
                startupLogger.LogInformation($"Found {appsettingsPath}.");
                
                Configuration = new ConfigurationBuilder()
                    .SetBasePath(startupLocation)
                    .AddJsonFile("appsettings.json",false)
                    .Build();
                Configuration.GetSection(appSettingsSectionName).Bind(Settings);
                Configuration.GetSection(LoggingConfig.AppSettingsSectionName).Bind(LoggingConfig.FromConfig);
            }
            else
            {
                Configuration = new ConfigurationBuilder().Build();
                Settings=new Settings();
                startupLogger.LogInformation($"No {appsettingsPath} found.");
            }

            startupLogger.LogInformation("Settings: {@Settings}",Settings.AsJsonElseNull());
            startupLogger.LogInformation("LoggingConfig: {@LoggingConfig}",LoggingConfig.FromConfig.AsJsonElseNull());
            return new Instance();
        }

        /// <summary>
        /// A convenience instance of the otherwise static <see cref="Startup"/> which allows
        /// <see cref="Startup.Configure"/> to be used in fluent style. 
        /// </summary>
        public class Instance : Startup
        {
            // ReSharper disable MemberHidesStaticFromOuterClass
            public new IConfiguration Configuration => Startup.Configuration;
            public new ILoggerFactory LoggerFactory => Startup.LoggerFactory;
            public new Settings Settings => Startup.Settings;
            public new ILogger CreateLogger<T>() => Startup.CreateLogger<T>();
            public new ILogger CreateLogger(string name)=>Startup.CreateLogger(name);
            public new ILogger CreateLogger(Type type) => Startup.CreateLogger(type);
        }
    }
}