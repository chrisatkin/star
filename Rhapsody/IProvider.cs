using System;

namespace org.btg.Star.Rhapsody
{
    /// <summary>
    /// IProvider 
    /// </summary>
    public interface IProvider
    {
        /// <summary>
        /// Start all available streams on the device
        /// </summary>
        void Start();

        /// <summary>
        /// Start a particular StreamType
        /// </summary>
        /// <param name="stream">The stream to start</param>
        void AddStream(StreamType stream);

        /// <summary>
        /// Stop all available streams
        /// </summary>
        void Stop();

        /// <summary>
        /// Stop a particular StreamType
        /// </summary>
        /// <param name="stream">The stream to stop</param>
        void Stop(StreamType stream);

        void ChangeState(ViewerState to);

        /// <summary>
        /// Determine whether the Device supports a given StreamType
        /// </summary>
        /// <param name="stream">The StreamType to query</param>
        /// <returns>Support for the device (bool)</returns>
        bool RespondsTo(StreamType stream);

        event EventHandler<ColourFrameReadyEventArgs> ColourFrameReady;

        event EventHandler<DepthFrameReadyEventArgs> DepthFrameReady;

        event EventHandler<SkeletonFrameReadyEventArgs> SkeletonFrameReady;
    }
}
