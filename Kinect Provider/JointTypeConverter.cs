using System;

namespace org.btg.Star.Rhapsody.Providers.Kinect
{
    /// <summary>
    /// Performs conversions between potential joint types
    /// </summary>
    public static class JointTypeConverter
    {
        /// <summary>
        /// Convert from <code>Microsoft.Kinect.JointType</code> to <code>org.btg.Star.Rhapsody.JointType</code>
        /// </summary>
        /// <param name="joint">A Kinect SDK JointType</param>
        /// <returns>The equivalent JointType in Rhapsody</returns>
        public static org.btg.Star.Rhapsody.JointType Convert(Microsoft.Kinect.JointType joint)
        {
            switch (joint)
            {
                case Microsoft.Kinect.JointType.AnkleLeft:
                    return org.btg.Star.Rhapsody.JointType.AnkleLeft;
                
                case Microsoft.Kinect.JointType.AnkleRight:
                    return org.btg.Star.Rhapsody.JointType.AnkleRight;

                case Microsoft.Kinect.JointType.ElbowLeft:
                    return org.btg.Star.Rhapsody.JointType.ElbowLeft;

                case Microsoft.Kinect.JointType.ElbowRight:
                    return org.btg.Star.Rhapsody.JointType.ElbowRight;

                case Microsoft.Kinect.JointType.FootLeft:
                    return org.btg.Star.Rhapsody.JointType.FootLeft;

                case Microsoft.Kinect.JointType.FootRight:
                    return org.btg.Star.Rhapsody.JointType.FootRight;

                case Microsoft.Kinect.JointType.HandLeft:
                    return org.btg.Star.Rhapsody.JointType.HandLeft;

                case Microsoft.Kinect.JointType.HandRight:
                    return org.btg.Star.Rhapsody.JointType.HandRight;

                case Microsoft.Kinect.JointType.Head:
                    return org.btg.Star.Rhapsody.JointType.Head;

                case Microsoft.Kinect.JointType.HipCenter:
                    return org.btg.Star.Rhapsody.JointType.HipCentre;

                case Microsoft.Kinect.JointType.HipLeft:
                    return org.btg.Star.Rhapsody.JointType.HipLeft;

                case Microsoft.Kinect.JointType.HipRight:
                    return org.btg.Star.Rhapsody.JointType.HipRight;

                case Microsoft.Kinect.JointType.KneeLeft:
                    return org.btg.Star.Rhapsody.JointType.KneeLeft;

                case Microsoft.Kinect.JointType.KneeRight:
                    return org.btg.Star.Rhapsody.JointType.KneeRight;

                case Microsoft.Kinect.JointType.ShoulderCenter:
                    return org.btg.Star.Rhapsody.JointType.ShoulderCentre;

                case Microsoft.Kinect.JointType.ShoulderLeft:
                    return org.btg.Star.Rhapsody.JointType.ShoulderLeft;

                case Microsoft.Kinect.JointType.ShoulderRight:
                    return org.btg.Star.Rhapsody.JointType.ShoulderRight;

                case Microsoft.Kinect.JointType.Spine:
                    return org.btg.Star.Rhapsody.JointType.Spine;

                case Microsoft.Kinect.JointType.WristLeft:
                    return org.btg.Star.Rhapsody.JointType.WristLeft;

                case Microsoft.Kinect.JointType.WristRight:
                    return org.btg.Star.Rhapsody.JointType.WristRight;

                default:
                    throw new ArgumentOutOfRangeException("joint", "The specified joint type is not supported");
            }
        }
    }
}