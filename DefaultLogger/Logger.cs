using System;
using System.IO;
using org.btg.Star.Rhapsody;

namespace org.btg.Star.Rhapsody.Loggers.Default
{
    public class Logger : ILogger
    {
        public string Location
        {
            get;
            set;
        }

        public Logger()
        {
            Console.WriteLine("Created logger (" + this.Location + ")");
        }

        public void Start()
        {
            this.Location = Path.Combine(new string[] { this.Location, DateTime.Now.ToString("F").Replace(':', '-') });
            Console.WriteLine("Starting logger at " + this.Location);
            
            // Create directory
            Directory.CreateDirectory(this.Location);
        }

        public void Stop()
        {
            ;
        }

        public void LogFrame(ColourFrame frame)
        {
            ;
        }

        public void LogFrame(DepthFrame frame)
        {
            ;
        }

        public void LogFrame(SkeletonFrame frame)
        {
            ;
        }
    }
}
