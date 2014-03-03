using System.Collections.Generic;

namespace org.btg.Star.Rhapsody
{
    public sealed class Skeleton
    {
        public int id;
        public TrackingState TrackingState;

        public Dictionary<JointType, Joint?> Joints = new Dictionary<JointType, Joint?>(20);

        public Skeleton()
        {
            ;
        }

        public Skeleton(int _id)
        {
            this.id = _id;
        }
    }
}
    