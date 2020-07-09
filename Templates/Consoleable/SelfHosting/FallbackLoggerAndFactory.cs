using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Consoleable.Dependencies;
using Microsoft.Extensions.Logging;

namespace Consoleable.SelfHosting
{
    class FallbackLoggerFactory: ILoggerFactory
    {
        static readonly FallbackLoggerProvider Provider= new FallbackLoggerProvider();
        public ILogger CreateLogger(string categoryName){return Provider.CreateLogger(categoryName);}
        public void AddProvider(ILoggerProvider provider){}
        public void Dispose(){Provider?.Dispose();}
    }

    static class FallbackLoggerFactoryExtension
    {
        public static ILoggerFactory AddFallbackLogger(this ILoggerFactory factory, List<string> backingList, string name = "ListOfString")
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            factory.AddProvider(new FallbackLoggerProvider(backingList??new List<string>(), name));
            return factory;
        }
        public static ILoggerFactory AddFallbackLogger(this ILoggerFactory factory,string name = "Console")
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            factory.AddProvider(new FallbackLoggerProvider(name));
            return factory;
        }
    }

    class FallbackLogger : ILogger
    {
        public static ConcurrentDictionary<string, FallbackLogger> Loggers { get; } = new ConcurrentDictionary<string, FallbackLogger>();
        
        Action<string> Output;
        
        static readonly string LoglevelPadding = ": ";

        static readonly string MessagePadding = new string(' ', LogLevel.Information.ToString().Length + LoglevelPadding.Length);

        static readonly string NewLineWithMessagePadding = Environment.NewLine + MessagePadding;
        
        [ThreadStatic] static StringBuilder logBuilder;
        Func<string, LogLevel, bool> filter;

        /// <summary>Defaults to UTC sortable [2020-07-09T15:30:54.003Z]</summary>
        public Func<string> Timestamp { get; set; } = () => 
            DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

        public FallbackLogger(List<string> backingList, string name=null, Func<string, 
        LogLevel, bool> filter = null, bool includeScopes = true, Func<string> 
        timestamp=null)
        {
            Name = name ?? String.Empty;
            Filter = filter ?? ((category, logLevel) => true);
            IncludeScopes = includeScopes;
            LoggedLines = backingList ?? new List<string>();
            Output = LoggedLines.Add;
            Timestamp = timestamp ?? Timestamp;
        }
        public FallbackLogger(string name,
            Func<string, LogLevel, bool> filter = null, bool includeScopes = true, 
            Func<string> timestamp=null)
        {
            Name = name ?? String.Empty;
            Filter = filter ?? ((category, logLevel) => true);
            IncludeScopes = includeScopes;
            Output = Console.WriteLine;
            Timestamp = timestamp ?? Timestamp;
        }

        public List<string> LoggedLines { get; }

        public Func<string, LogLevel, bool> Filter
        {
            protected internal get => filter;
            set => filter = value ?? throw new ArgumentNullException(nameof(value));
        }

        public bool IncludeScopes { get; set; }

        public string Name { get; }

        ScopeStack Scopes {get;}= new ScopeStack();

        public class ScopeStack : Stack<(string, object)>, IDisposable
        {
            public void Dispose(){Pop();}
            public new ScopeStack Push((string,object) item){base.Push(item);return this;}
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
                                Exception exception,
                                Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;
            
            var message = "";
            try
            {
                try
                {
                    var fState = ((IReadOnlyList<KeyValuePair<string, object>>)state);
                    var m = 
                        fState.FirstOrDefault(s => s.Key == "{OriginalFormat}");
                    var values =
                        fState.Where(s => s.Key != "{OriginalFormat}")
                            .Select(v=>v.Key+"="+v.Value.AsJsonElseNull());
                    message =
                        $"{m.Value}\n{string.Join("\n", values)}";
                }
                catch (Exception)
                {
                    if (formatter == null) throw new ArgumentNullException(nameof(formatter));
                    message = formatter(state, exception);
                    throw;
                }
            }
            catch (Exception e)
            {
                message += $"error trying to log {state?.GetType()} {exception}\nException: {e}";
            }

            if (string.IsNullOrEmpty(message) && exception == null) return;
            WriteMessage(logLevel, Name, eventId.Id, message, exception);
        }

        public bool IsEnabled(LogLevel logLevel) { return Filter(Name, logLevel); }

        public IDisposable BeginScope<TState>(TState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            Scopes.Push((Name, state));
            return Scopes;
        }

        void WriteMessage(LogLevel logLevel, string logName, int eventId, string message, Exception exception)
        {
            var builder = logBuilder;
            logBuilder = null;
            if (builder == null) builder = new StringBuilder();
            builder.Append(LoglevelPadding);
            builder.Append(logName);
            builder.Append("[");
            builder.Append(eventId);
            builder.AppendLine("]");
            if (IncludeScopes) GetScopeInformation(builder);
            if (!String.IsNullOrEmpty(message))
            {
                builder.Append(MessagePadding);
                var length = builder.Length;
                builder.AppendLine(message);
                builder.Replace(Environment.NewLine, NewLineWithMessagePadding, length, message.Length);
            }

            if (exception      != null) builder.AppendLine(exception.ToString());
            if (builder.Length > 0) Output($"[{logLevel.ToString()}]" +
                       $"[{Timestamp()}]" +
                       $" {builder}");

            builder.Clear();
            if (builder.Capacity > 1024) builder.Capacity = 1024;
            logBuilder = builder;
        }

        void GetScopeInformation(StringBuilder builder)
        {
            var length = builder.Length;
            foreach(var scope in Scopes)
            {
                var asString = scope.Item2 is Type t ? t.Name : scope.Item2;
                var str = length != builder.Length
                              ? String.Format("=> {0} ", asString)
                              : String.Format("=> {0}",  asString);
                builder.Insert(length, str);
            }

            if (builder.Length <= length)return;
            builder.Insert(length, MessagePadding);
            builder.AppendLine();
        }
    }


    class FallbackLoggerProvider : ILoggerProvider
    {
        static readonly Func<string, LogLevel, bool> FalseFilter = (cat, level) => false;
        static readonly Func<string, LogLevel, bool> TrueFilter = (cat, level) => true;
        readonly Func<string, LogLevel, bool> filter;
        readonly bool includeScopes;

        public FallbackLoggerProvider()
        {
            filter = TrueFilter;
            includeScopes = true;
        }

        public FallbackLoggerProvider(List<String> backingList, string name = null, Func<string, LogLevel, bool> filter = null, bool includeScopes = true)
        {
            this.filter = filter ?? TrueFilter;
            this.includeScopes = includeScopes;
            if (name!= null && backingList == null) CreateLogger(name);
            FallbackLogger.Loggers.GetOrAdd(name ?? String.Empty, n => new FallbackLogger(backingList, n, this.filter, this.includeScopes));
        }

        public FallbackLoggerProvider(string name ) { FallbackLogger.Loggers.TryAdd(name, new FallbackLogger("name")); }

        public ILogger CreateLogger(string name)
        {
            return FallbackLogger.Loggers.GetOrAdd(name ?? String.Empty, CreateLoggerImplementation);
        }

        public void Dispose()
        {
            FallbackLogger.Loggers.Clear();
        }

        FallbackLogger CreateLoggerImplementation(string name)
        {
            return new FallbackLogger(name, GetFilter(), includeScopes);
        }

        Func<string, LogLevel, bool> GetFilter() { return filter ?? TrueFilter; }
    }
}
