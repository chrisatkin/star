using System;
using System.IO;
using System.Collections.Generic;

namespace org.btg.Star.Rhapsody
{
    public class Sensor
    {
        public event EventHandler<ColourFrameReadyEventArgs> ColourFrameReady;

        public event EventHandler<DepthFrameReadyEventArgs> DepthFrameReady;

        public event EventHandler<SkeletonFrameReadyEventArgs> SkeletonFrameReady;

        public IProvider Provider;

        private bool _started;

        public Sensor()
        {
            this.Provider = null;
            this._started = false;
        }

        public void Start(StreamType[] streams)
        {
            // Ensure there's a provider
            if (this.Provider == null)
            {
                throw new InvalidOperationException("A data source/provider has not been specified");
            }

            // Add listeners
            if (this.ColourFrameReady != null)
            {
                this.Provider.ColourFrameReady += this._HandleColourFrameReady;
            }

            if (this.DepthFrameReady != null)
            {
                this.Provider.DepthFrameReady += this._HandleDepthFrameReady;
            }

            if (this.SkeletonFrameReady != null)
            {
                this.Provider.SkeletonFrameReady += this._HandleSkeletonFrameReady;
            }

            // Start the Sensor
            foreach (StreamType stream in streams)
            {
                this.Provider.Start(stream);
            }

            this._started = true;
        }

        public void Stop()
        {
            if (this.Provider == null)
            {
                throw new InvalidOperationException("A data source/provider has not been specified");
            }

            if (this._started == false)
            {
                throw new InvalidOperationException("Data collection has not been started");
            }

            // Remove listeners
            if (this.ColourFrameReady != null)
            {
                this.Provider.ColourFrameReady -= this._HandleColourFrameReady;
            }

            if (this.DepthFrameReady != null)
            {
                this.Provider.DepthFrameReady -= this._HandleDepthFrameReady;
            }

            if (this.SkeletonFrameReady != null)
            {
                this.Provider.SkeletonFrameReady -= this._HandleSkeletonFrameReady;
            }

            this._started = false;
        }

        public static List<string> DiscoverServiceProviders()
        {
            List<string> providers = new List<string>();

            foreach (FileInfo file in new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles("*Provider.dll", SearchOption.TopDirectoryOnly))
            {
                providers.Add(file.Name);
            }

            return providers;
        }

        // Listeners
        // ------------------------------------------------

        private void _HandleColourFrameReady(object sender, ColourFrameReadyEventArgs frame)
        {
            foreach (Delegate d in this.ColourFrameReady.GetInvocationList())
            {
                d.DynamicInvoke(new object[] { sender, frame });
            }
        }

        private void _HandleDepthFrameReady(object sender, DepthFrameReadyEventArgs frame)
        {
            foreach (Delegate d in this.DepthFrameReady.GetInvocationList())
            {
                d.DynamicInvoke(new object[] { sender, frame });
            }
        }

        private void _HandleSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs frame)
        {
            foreach (Delegate d in this.SkeletonFrameReady.GetInvocationList())
            {
                d.DynamicInvoke(new object[] { sender, frame });
            }
        }
    }
}
