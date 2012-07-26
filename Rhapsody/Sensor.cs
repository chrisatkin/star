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

        public ILogger Logger;

        private bool _started;

        public Sensor()
        {
            this.Provider = null;
            this._started = false;
        }

        public void AddStreams(StreamType[] streams)
        {
            // Ensure there's a provider
            if (this.Provider == null)
            {
                throw new InvalidOperationException("A data source/provider has not been specified");
            }

            // Start the Sensor
            foreach (StreamType stream in streams)
            {
                this.Provider.AddStream(stream);

                switch (stream)
                {
                    case StreamType.ColourStream:
                        this.Provider.ColourFrameReady += this._HandleColourFrameReady;
                    break;

                    case StreamType.DepthStream:
                        this.Provider.DepthFrameReady += this._HandleDepthFrameReady;
                    break;

                    case StreamType.SkeletonStream:
                        this.Provider.SkeletonFrameReady += this._HandleSkeletonFrameReady;
                    break;
                }
            }
        }

        public void Start()
        {
            this.Logger.Start();
            this.Provider.Start();
            this._started = true;
        }

        public void Stop()
        {
            this.Logger.Stop();

            if (this._started)
            {
                this.Provider.Stop();
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
            // Log frame
            this.Logger.LogFrame((ColourFrame) frame.Frame);

            foreach (Delegate d in this.ColourFrameReady.GetInvocationList())
            {
                d.DynamicInvoke(new object[] { sender, frame });
            }
        }

        private void _HandleDepthFrameReady(object sender, DepthFrameReadyEventArgs frame)
        {
            // Log frame
            this.Logger.LogFrame((DepthFrame) frame.Frame);

            foreach (Delegate d in this.DepthFrameReady.GetInvocationList())
            {
                d.DynamicInvoke(new object[] { sender, frame });
            }
        }

        private void _HandleSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs frame)
        {
            // Log frame
            this.Logger.LogFrame((SkeletonFrame) frame.Frame);

            foreach (Delegate d in this.SkeletonFrameReady.GetInvocationList())
            {
                d.DynamicInvoke(new object[] { sender, frame });
            }
        }
    }
}