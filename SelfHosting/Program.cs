using System;
using System.Collections.Generic;
using System.IO;

namespace Consoleable.SelfHosting
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
                ValidateParametersElseShowHelpTextAndExitImmediately(args);

            Startup.Configure();
            
            new Consoleable(
                Startup.CreateLogger<Consoleable>(),
                Startup.Settings
            ).Do(someFiles, someKeys);
        }

        static 
            (FileInfo[] someFiles, Dictionary<string, string> someKeys) 
                ValidateParametersElseShowHelpTextAndExitImmediately(string[] args)
        {
            ShowHelpTextAndExitImmediatelyIf(shouldShowHelpThenExit: args.Length == 0);
            var (someFiles, someKeys) = ParseArgs.GetSomeFileNamesAndKeys(args);
            ShowHelpTextAndExitImmediatelyIf(someFiles.Length == 0);
            return (someFiles,someKeys);
        }

        const string ConsoleHelpText = @"
Consoleable filename1 filename2 key1=value1 key2=value2 

    Modify this help text to show the parameters usable by your component from a console command line

    You might mention that Settings can be read from the appSettings.json file.
    
    Example:

    Consoleable somefilename.txt this=This that=That

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
                        var kv= arg.Split('=', 2);
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