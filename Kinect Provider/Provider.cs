using System;
using System.Reflection;
using org.btg.Star.Rhapsody;

namespace org.btg.Star.Rhapsody.Providers.Kinect
{
    /// <summary>
    /// Implement a Kinect provider service for Rhapsody. Note that fully qualified class names are used instead of
    /// <code>using Microsoft.Kinect</code> in order to avoid ambiguity. They are used throughout for consistency.
    /// </summary>
    public sealed class Provider : IProvider
    {
        public event EventHandler<ColourFrameReadyEventArgs> ColourFrameReady;

        public event EventHandler<DepthFrameReadyEventArgs> DepthFrameReady;

        public event EventHandler<SkeletonFrameReadyEventArgs> SkeletonFrameReady;

        private Microsoft.Kinect.KinectSensor _kinect;

        public Provider()
        {
            // Setup the Kinect
            foreach (var potential in Microsoft.Kinect.KinectSensor.KinectSensors)
            {
                if (potential.Status == Microsoft.Kinect.KinectStatus.Connected)
                {
                    this._kinect = potential;
                    break;
                }
            }

            // Ensure a Kinect was found
            if (this._kinect == null)
            {
                //#todo: change the exception type here
                throw new Exception();
            }

            Console.WriteLine("Found Kinect");
        }

        public void Start()
        {
            this._kinect.Start();
        }

        public void AddStream(StreamType stream)
        {
            if (!this.RespondsTo(stream))
            {
                throw new InvalidOperationException("This provider does not respond to " + stream.ToString());
            }
            switch (stream)
            {
                case StreamType.ColourStream:
                        this._kinect.ColorStream.Enable(Microsoft.Kinect.ColorImageFormat.RgbResolution640x480Fps30);
                        this._kinect.ColorFrameReady += this._HandleColourFrameReady;
                        Console.WriteLine("Added depth listener");
                break;

                case StreamType.DepthStream:
                        this._kinect.DepthStream.Enable(Microsoft.Kinect.DepthImageFormat.Resolution640x480Fps30);
                        this._kinect.DepthFrameReady += this._HandleDepthFrameReady;
                        Console.WriteLine("Added depth listener");
                break;

                case StreamType.SkeletonStream:
                        this._kinect.SkeletonStream.Enable();
                        this._kinect.SkeletonFrameReady += this._HandleSkeletonFrameReady;
                        Console.WriteLine("Added Skeleton listener");
                break;
            }

            Console.WriteLine("Started " + stream.ToString());
        }

        public void Stop()
        {
            ;
        }

        public void Stop(StreamType stream)
        {
            if (!this.RespondsTo(stream))
            {
                throw new InvalidOperationException("This provider does not respond to " + stream.ToString());
            }

            switch (stream)
            {
                case StreamType.ColourStream:
                    if (this.ColourFrameReady != null)
                    {
                        this._kinect.ColorStream.Disable();
                        this._kinect.ColorFrameReady -= this._HandleColourFrameReady;
                    }
                    break;

                case StreamType.DepthStream:
                    if (this.DepthFrameReady != null)
                    {
                        this._kinect.DepthStream.Disable();
                        this._kinect.DepthFrameReady -= this._HandleDepthFrameReady;
                    }
                    break;

                case StreamType.SkeletonStream:
                    if (this.SkeletonFrameReady != null)
                    {
                        this._kinect.SkeletonStream.Disable();
                        this._kinect.SkeletonFrameReady -= this._HandleSkeletonFrameReady;
                    }
                    break;
            }

            this._kinect.Stop();
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
                case StreamType.SkeletonStream:
                    return true;

                default:
                    return false;
            }
        }

        private void _HandleColourFrameReady(object sender, Microsoft.Kinect.ColorImageFrameReadyEventArgs args)
        {
            byte[] frame;

            // Get the Kinect colour frame
            using (Microsoft.Kinect.ColorImageFrame kinect_frame = args.OpenColorImageFrame())
            {
                if (kinect_frame == null)
                {
                    return;
                }

                frame = new byte[this._kinect.ColorStream.FramePixelDataLength];
                kinect_frame.CopyPixelDataTo(frame);
            }

            // Convert to Rhapsody frame
            ColourFrame rhapsody_frame = new ColourFrame {
                Format = ColorImageFormat.Rgb_640x480_30fps,
                Data = frame
            };

            // Call all listeners
            foreach (Delegate d in this.ColourFrameReady.GetInvocationList())
            {
                d.DynamicInvoke(new object[] { sender, new ColourFrameReadyEventArgs {
                    Frame = rhapsody_frame
                } });
            }
        }

        private void _HandleDepthFrameReady(object sender, Microsoft.Kinect.DepthImageFrameReadyEventArgs args)
        {
            short[] frame;

            using (Microsoft.Kinect.DepthImageFrame kinect_frame = args.OpenDepthImageFrame())
            {
                if (kinect_frame == null)
                {
                    return;
                }

                frame = new short[kinect_frame.PixelDataLength];
                kinect_frame.CopyPixelDataTo(frame);

                // Discard irrelevant data
                for (int i = 0; i < frame.Length; ++i)
                {
                    frame[i] = (short) (frame[i] >> Microsoft.Kinect.DepthImageFrame.PlayerIndexBitmaskWidth);
                }

                // Convert to Rhapsody frame
                DepthFrame rhapsody_frame = new DepthFrame
                {
                    Format = DepthImageFormat.Resolution640x480_30fps,
                    Data = frame
                };

                // Call all listeners
                foreach (Delegate d in this.DepthFrameReady.GetInvocationList())
                {
                    d.DynamicInvoke(new object[] { sender, new DepthFrameReadyEventArgs {
                        Frame = rhapsody_frame
                    } });
                }
            } 
        }

        private void _HandleSkeletonFrameReady(object sender, Microsoft.Kinect.SkeletonFrameReadyEventArgs args)
        {
            // Capture Kinect data
            Microsoft.Kinect.Skeleton[] skeletons = new Microsoft.Kinect.Skeleton[0];

            using (Microsoft.Kinect.SkeletonFrame kinect_frame = args.OpenSkeletonFrame())
            {
                if (kinect_frame != null)
                {
                    skeletons = new Microsoft.Kinect.Skeleton[kinect_frame.SkeletonArrayLength];
                    kinect_frame.CopySkeletonDataTo(skeletons);
                }
            }

            // Convert the Microsoft Skeleton format to Rhapsody skeletons
            SkeletonFrame frame = new SkeletonFrame();
            
            foreach (Microsoft.Kinect.Skeleton kinect_skeleton in skeletons)
            {
                if (kinect_skeleton.TrackingState != Microsoft.Kinect.SkeletonTrackingState.Tracked)
                {
                    continue;
                }

                //Console.WriteLine("Kinect found skeleton " + kinect_skeleton.TrackingId + " (" + kinect_skeleton.Position.X + ", " + kinect_skeleton.Position.Y + ")");
                Skeleton skeleton = new Skeleton
                {
                    TrackingState = TrackingState.Tracked
                };

                foreach (Microsoft.Kinect.Joint kinect_joint in kinect_skeleton.Joints)
                {
                    Microsoft.Kinect.SkeletonPoint kinect_point = kinect_joint.Position;
                    skeleton.Joints.Add(JointTypeConverter.Convert(kinect_joint.JointType), new Joint(kinect_point.X, kinect_point.Y, kinect_point.Z));
                }

                frame.Skeletons.Add(skeleton);
            }

            // Invoke each of the Listeners
            foreach (Delegate d in this.SkeletonFrameReady.GetInvocationList())
            {
                d.DynamicInvoke(new object[] { sender, new SkeletonFrameReadyEventArgs {
                    Frame = frame
                }});
            }
        }
    }
}