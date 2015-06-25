using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuGet.VisualStudio.Implementation
{
    /// <summary>
    /// A logger that logs to nowhere.
    /// </summary>
    internal class VSAPILogger : NuGet.Logging.ILogger
    {
        public void LogDebug(string data)
        {

        }

        public void LogError(string data)
        {

        }

        public void LogInformation(string data)
        {

        }

        public void LogVerbose(string data)
        {

        }

        public void LogWarning(string data)
        {

        }
    }
}
