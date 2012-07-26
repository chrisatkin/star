using System;
using System.Windows;

namespace org.btg.Star.Rhapsody.Providers.Csv
{
    class Provider : IProvider
    {
        public event EventHandler<ColourFrameReadyEventArgs> ColourFrameReady;

        public event EventHandler<DepthFrameReadyEventArgs> DepthFrameReady;

        public event EventHandler<SkeletonFrameReadyEventArgs> SkeletonFrameReady;

        public void StartAll()
        {
            foreach(StreamType stream in Enum.GetValues(typeof(StreamType)))
            {
                if (this.RespondsTo(stream))
                {
                    this.AddStream(stream);
                }
            }
        }

        public void AddStream(StreamType stream)
        {
            this.VerifyRespondsTo(stream);


        }

        public void Start()
        {
            ;
        }

        public void Stop()
        {
            ;
        }

        public void Stop(StreamType stream)
        {
            ;
        }

        public void ChangeState(ViewerState to)
        {
            ;
        }

        public bool RespondsTo(StreamType stream)
        {
            switch (stream)
            {
                case StreamType.ColourStream:
                case StreamType.DepthStream:
                default:
                    return false;

                case StreamType.SkeletonStream:
                    return true;
            }
        }

        private void VerifyRespondsTo(StreamType type)
        {
            if (!this.RespondsTo(type))
            {
                throw new StreamTypeNotSupportedException();
            }
        }
    }
}
