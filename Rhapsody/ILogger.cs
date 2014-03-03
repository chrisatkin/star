namespace org.btg.Star.Rhapsody
{
    public interface ILogger
    {
        string Location
        {
            get;
            set;
        }

        void Start();

        void Stop();

        void LogFrame(ColourFrame frame);

        void LogFrame(DepthFrame frame);

        void LogFrame(SkeletonFrame frame);
    }
}