using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Consoleable
{
    /// <summary>
    /// A component which …
    /// </summary>
    public class Consoleable
    {
        /// <summary>Summary</summary>
        /// <param name="someFiles"></param>
        /// <param name="someKeys"></param>
        /// <returns></returns>
        public bool Do( FileInfo[] someFiles, Dictionary<string,string> someKeys)
        {
            var paramsOk=SanitiseAndValidateParameters(someFiles, ref someKeys);
            return false;
        }

        bool SanitiseAndValidateParameters(FileInfo[] someFiles, ref Dictionary<string,string> someKeys)
        {
            someKeys ??= new Dictionary<string, string>();
            var parametersAreValid = someKeys.Count==0 ||  someKeys.First().Key!="exampleError";
            
            log.Log(parametersAreValid ? LogLevel.Information:LogLevel.Error,
                "Called with {files} {@keys}", 
                someFiles.Select(f => f.Name), 
                someKeys.Select(kv => $"{kv.Key}={kv.Value}"));
            
            return parametersAreValid;
        }

        public Consoleable(ILogger log, Settings settings)
        {
            this.log = log;
            this.settings = settings;
        }
        readonly ILogger log;
        readonly Settings settings;

    }
}