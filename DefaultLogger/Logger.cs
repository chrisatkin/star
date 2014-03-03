using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using org.btg.Star.Rhapsody;

namespace org.btg.Star.Rhapsody.Loggers.Default
{
    public class Logger : ILogger
    {
        private StreamWriter _SkeletonWriter;
        private object _ColourFrameLock;
        private object _DepthFrameLock;
        private object _SkeletonFrameLock;

        public string Location
        {
            get;
            set;
        }

        public void Start()
        {
            this.Location = Path.Combine(new string[] { this.Location, DateTime.Now.ToString("F").Replace(':', '-') });
            Console.WriteLine("Starting logger at " + this.Location);
            
            // Create directory
            Directory.CreateDirectory(this.Location);
            Directory.CreateDirectory(Path.Combine(new string[] { this.Location, "ColourFrames" }));
            Directory.CreateDirectory(Path.Combine(new string[] { this.Location, "DepthFrames" }));

            // Create files within the directory
            this._SkeletonWriter = new StreamWriter(Path.Combine(new string[] { this.Location, "Skeleton.log" }));

            // Create locks
            this._ColourFrameLock = new object();
            this._DepthFrameLock = new object();
            this._SkeletonFrameLock = new object();
        }

        public void Stop()
        {

            if (this._SkeletonWriter != null)
            {
                this._SkeletonWriter.Close();
            }
        }

        public void LogFrame(ColourFrame frame)
        {
            /*Thread t = new Thread(new ParameterizedThreadStart(this._LogColourFrame))
            {
                Name = "ColourFrameWriter"
            };
            t.Start(frame);*/
        }

        public void LogFrame(DepthFrame frame)
        {
            /*Thread t = new Thread(new ParameterizedThreadStart(this._LogDepthFrame))
            {
                Name = "DepthFrameWriter"
            };
            t.Start(frame);*/
        }

        public void LogFrame(SkeletonFrame frame)
        {
            /*Thread t = new Thread(new ParameterizedThreadStart(this._LogSkeletonFrame))
            {
                Name = "SkeletonFrameWriter"
            }; 
            t.Start();*/
            this._LogSkeletonFrame(frame);
        }

        private void _LogColourFrame(object data)
        {
            lock (this._ColourFrameLock)
            {
                ColourFrame frame = (ColourFrame)data;

                WriteableBitmap bitmap = new WriteableBitmap(640, 480, 96.0, 96.0, System.Windows.Media.PixelFormats.Bgr32, null);
                bitmap.WritePixels(new Int32Rect(0, 0, 640, 480), frame.Data, 640 * sizeof(int), 0);

                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));

                string path = Path.Combine(new string[] { this.Location, "ColourFrames", DateTime.Now.ToString("ffffff") + ".png" });

                try
                {
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        encoder.Save(fs);
                    }
                }
                catch (IOException)
                {
                    ;
                }
                finally
                {
                    Monitor.Pulse(this._ColourFrameLock);
                }
            }
        }

        private void _LogDepthFrame(object data)
        {
            lock (this._DepthFrameLock)
            {
                DepthFrame frame = (DepthFrame)data;

                WriteableBitmap bitmap = new WriteableBitmap(640, 480, 96.0, 96.0, System.Windows.Media.PixelFormats.Bgr32, null);
                bitmap.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), frame.Data, sizeof(int) * 640, 0);

                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));

                string path = Path.Combine(new string[] { this.Location, "DepthFrames", DateTime.Now.ToString("ffffff") + ".png" });

                try
                {
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        encoder.Save(fs);
                    }
                }
                catch (IOException)
                {
                    ;
                }
                finally
                {
                    Monitor.Pulse(this._DepthFrameLock);
                }
            }
        }

        private void _LogSkeletonFrame(object data)
        {
            lock (this._SkeletonFrameLock)
            {
                SkeletonFrame frame = (SkeletonFrame)data;

                if (frame == null || frame.Skeletons.Capacity == 0)
                {
                    Console.WriteLine("No skeletons");
                    return;
                }

                Console.WriteLine(frame);
                Console.WriteLine(frame.Skeletons.Capacity);

                foreach (Skeleton skeleton in frame.Skeletons)
                {
                    foreach (KeyValuePair<JointType, Joint?> pair in skeleton.Joints)
                    {
                        JointType type = (JointType) pair.Key;
                        Joint? joint = (Joint) pair.Value;

                        string value = (joint == null) ? "null" : joint.Value.X + ", " + joint.Value.Y + ", " + joint.Value.Z;
                        
                        this._SkeletonWriter.WriteLine(type.ToString() + ": " + value);
                    }

                    this._SkeletonWriter.WriteLine();
                }

                this._SkeletonWriter.WriteLine();
                this._SkeletonWriter.WriteLine();

                Monitor.Pulse(this._SkeletonFrameLock);
            }
        }
    }
}
