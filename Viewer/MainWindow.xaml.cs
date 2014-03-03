using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using org.btg.Star.Rhapsody;

namespace Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Rhapsody
        private Sensor _sensor;
        private List<string> _providers;
        
        // Skeleton viewer
        private DrawingGroup group;
        private DrawingImage imageSource;

        // Depth viewer
        private WriteableBitmap depthBitmap;

        // Color viewer
        private WriteableBitmap colourBitmap;

        public MainWindow()
        {
            InitializeComponent();

            // Create the Sensor object.
            // Note that at this point it will not have a data source, and so cannot be started.
            this._sensor = new Sensor();
            this._sensor.ColourFrameReady += this._ColourFrameReady;
            this._sensor.DepthFrameReady += this._DepthFrameReady;
            this._sensor.SkeletonFrameReady += this._SkeletonFrameReady;

            // Set the logger
            this._sensor.Logger = ModuleLoader.LoadLogger("DefaultLogger.dll");
            this._sensor.Logger.Location = Path.Combine(new string[] { Directory.GetCurrentDirectory(), "logs" });

            // Discover the available providers and update the UI accordingly
            this._providers = Sensor.DiscoverServiceProviders();

            foreach (string provider in this._providers)
            {
                this.Provider.Items.Add(provider);
            }
        }

        /// <summary>
        /// Change the current data provider. Doing so will stop the current provider and will not start the new one automatically.
        /// </summary>
        /// <param name="provider">Location to find the DLL file containing a new class implementing IProvider.</param>
        private void _ChangeProvider(string provider)
        {
            // Change the data source
            Console.WriteLine("Changing providers to " + provider);
            this._sensor.Provider = ModuleLoader.LoadProvider(provider);
        }

        // Device Event Listeners
        // -------------------------------------------------

        private void _ColourFrameReady(object sender, ColourFrameReadyEventArgs frame)
        {
            ColourDrawingHelper.Draw((ColourFrame) frame.Frame, this.colourBitmap);
        }

        private void _DepthFrameReady(object sender, DepthFrameReadyEventArgs frame)
        {
            //DepthDrawingHelper.Draw((DepthFrame) frame.Frame, this.depthBitmap);
        }

        private void _SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs frame)
        {
            SkeletonFrame f = (SkeletonFrame) frame.Frame;

            using (DrawingContext context = this.group.Open())
            {
                // Draw black rectangle for the viewer
                context.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, 640, 480));

                if (f.Skeletons.Count != 0)
                {
                    SkeletonDrawingHelper.Draw(f.Skeletons, context);
                }

                this.group.ClipGeometry = new RectangleGeometry(new Rect(0, 0, 640, 480));
            }
        }

        // Interface Event Listeners
        // -------------------------------------------------

        private void _ProviderChangedEvent(object sender, SelectionChangedEventArgs e)
        {
            this._ChangeProvider((string) e.AddedItems[0]);
        }

        private void _StartClickEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                this._sensor.AddStreams(new StreamType[] { StreamType.SkeletonStream, StreamType.ColourStream, StreamType.DepthStream });
                this._sensor.Start();
            }
            catch (InvalidOperationException excep)
            {
                System.Windows.Forms.MessageBox.Show(text: "Please select a provider",
                                                     caption: "STaR",
                                                     buttons: MessageBoxButtons.OK,
                                                     icon: MessageBoxIcon.Error);
            }
        }

        private void _StopClickEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                this._sensor.Stop();
            }
            catch (InvalidOperationException excep)
            {
                System.Windows.Forms.MessageBox.Show(text: "Please select a provider",
                                                     caption: "STaR",
                                                     buttons: MessageBoxButtons.OK,
                                                     icon: MessageBoxIcon.Error);
            }
        }

        private void Viewer_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this._sensor.Stop();
        }

        private void Viewer_Loaded(object sender, RoutedEventArgs e)
        {
            // Skeleton viewer
            this.group = new DrawingGroup();
            this.imageSource = new DrawingImage(this.group);
            this.Skeleton.Source = this.imageSource;

            // Depth viewer
            this.depthBitmap = new WriteableBitmap(640, 480, 96.0, 96.0, PixelFormats.Bgr32, null);
            //this.Depth.Source = this.depthBitmap;

            // Colour Viewer
            this.colourBitmap = new WriteableBitmap(640, 480, 96.0, 96.0, PixelFormats.Bgr32, null);
            this.Depth.Source = this.colourBitmap;
        }
    }
}
