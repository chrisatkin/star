using System;
using System.Reflection;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using org.btg.Star.Rhapsody;

namespace Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Sensor _sensor;
        private List<string> _providers;
        private DrawingGroup group;
        private DrawingImage imageSource;

        public MainWindow()
        {
            InitializeComponent();

            // Create the Sensor object.
            // Note that at this point it will not have a data source, and so cannot be started.
            this._sensor = new Sensor();
            this._sensor.ColourFrameReady += this._ColourFrameReady;
            this._sensor.DepthFrameReady += this._DepthFrameReady;
            this._sensor.SkeletonFrameReady += this._SkeletonFrameReady;

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
            this._sensor.Provider = ProviderLoader.Load(provider);

            // Change the UI (load from XAML)
            //Assembly.LoadFrom(provider);
            //todo: this
        }

        // Device Event Listeners
        // -------------------------------------------------

        private void _ColourFrameReady(object sender, ColourFrameReadyEventArgs frame)
        {
            Console.WriteLine("Viewer: Color frame");
        }

        private void _DepthFrameReady(object sender, DepthFrameReadyEventArgs frame)
        {
            Console.WriteLine("Viewer: Depth frame");
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
                Console.WriteLine("Start button pressed");
                this._sensor.Start(new StreamType[] { StreamType.SkeletonStream});
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
            this.group = new DrawingGroup();
            this.imageSource = new DrawingImage(this.group);
            this.Skeleton.Source = this.imageSource;
        }
    }
}
