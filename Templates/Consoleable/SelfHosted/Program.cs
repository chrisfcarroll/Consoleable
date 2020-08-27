using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Consoleable.SelfHosted
{
    /// <summary>
    /// A commandline wrapper for <see cref="Consoleable"/> which uses <see cref="Startup"/> to
    /// initialize Configuration, Logging, and Settings. 
    /// </summary>
    public static class Program
    {
        public static void Main(params string[] args)
        {
            var (someFiles,someKeys) = 
                    ValidateExampleParametersElseShowHelpTextAndExit(args);
            
            Startup.Configure();
            
        new Consoleable(
                Startup.LoggerFactory.CreateLogger<Consoleable>(),
                Startup.Settings
            ).Do();
        }

        static 
            (FileInfo[] someFiles, Dictionary<string, string> someKeys) 
                ValidateExampleParametersElseShowHelpTextAndExit(string[] args)
        {
            ShowHelpTextAndExitImmediatelyIf(shouldShowHelpThenExit: args.Length == 0);
            ShowHelpTextAndExitImmediatelyIf(HelpOptions.Contains(args[0].TrimStart('/').TrimStart('-')));
            var (someFiles, someKeys) = ParseArgs.GetSomeFileNamesAndKeys(args);
            ShowHelpTextAndExitImmediatelyIf(someFiles.Length == 0 && someKeys.Count==0);
            return (someFiles,someKeys);
        }

        static readonly string[] HelpOptions = {"?", "h","help"};

        const string ConsoleHelpText = @"
Consoleable parameter1 [ parameter2 ] [ key1=value1 [ key2=value2 ]]]

    Example help text for commandline usage.

    You might mention that Settings can be read from the appsettings.json file.
    
    Example:

    Consoleable simple example of parameters

";
        static class ParseArgs
        {
            public static ( FileInfo[], Dictionary<string, string>) GetSomeFileNamesAndKeys(params string[] args)
            {
                var files = new List<FileInfo>(); 
                var someKeys=new Dictionary<string,string>();
                foreach (var arg in args)
                {
                    if (arg.Contains("="))
                    {
                        var kv= arg.Split(new[]{'='}, 2);
                        someKeys.Add( kv[0], kv[1]);
                    }
                    else
                    {
                        files.Add(new FileInfo(arg));
                    }
                }
                return (files.ToArray(), someKeys);
            }
        }

        /// <summary>
        ///If <paramref name="shouldShowHelpThenExit"/> is not true, then show <see cref="ConsoleHelpText"/> and call
        /// <see cref="Environment.Exit"/> with <c>ExitCode</c>==<see cref="ReturnExitCodeIfParametersInvalid"/>  
        /// </summary>
        /// <param name="shouldShowHelpThenExit"></param>
        static void ShowHelpTextAndExitImmediatelyIf(bool shouldShowHelpThenExit)
        {
            if (!shouldShowHelpThenExit) return;
            Console.WriteLine(ConsoleHelpText);
            Environment.Exit(ReturnExitCodeIfParametersInvalid);
        }

        const int ReturnExitCodeIfParametersInvalid = 1;
    }
}