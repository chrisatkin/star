using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace org.btg.Star.Rhapsody
{
    public static class SkeletonDrawingHelper
    {
        public static void Draw(List<Skeleton> skeletons, DrawingContext context)
        {
            foreach (Skeleton skeleton in skeletons)
            {
                if (skeleton.TrackingState == TrackingState.Tracked)
                {
                    SkeletonDrawingHelper._DrawSkeleton(skeleton, context);
                }
            }
        }

        private static void _DrawSkeleton(Skeleton skeleton, DrawingContext context)
        {
            // Draw Joints
            foreach (KeyValuePair<JointType, Joint?> pair in skeleton.Joints)
            {
                JointType type = pair.Key;
                Joint? joint = pair.Value;

                if (joint == null)
                {
                    continue;
                }

                Joint j1 = joint.GetValueOrDefault();
                Point draw_at = SkeletonDrawingHelper._MapSkeletonPointToScreen(j1.X, j1.Y, j1.Z);

                context.DrawEllipse(Brushes.Green, null, draw_at, 10d, 10d);

                // Really bad performance
                //System.Console.WriteLine("Drawing " + type.ToString() + " at (" + draw_at.ToString() + ")");
            }

            // Draw bones
            // Torso
            SkeletonDrawingHelper._DrawBone(skeleton.Joints[JointType.Head], skeleton.Joints[JointType.ShoulderCentre], context);                // head to shoulder centre
            SkeletonDrawingHelper._DrawBone(skeleton.Joints[JointType.ShoulderCentre], skeleton.Joints[JointType.Spine], context);               // shoulder centre to spine
            SkeletonDrawingHelper._DrawBone(skeleton.Joints[JointType.Spine], skeleton.Joints[JointType.HipCentre], context);                    // spine to hip centre
            SkeletonDrawingHelper._DrawBone(skeleton.Joints[JointType.HipCentre], skeleton.Joints[JointType.HipLeft], context);                  // hip centre to left
            SkeletonDrawingHelper._DrawBone(skeleton.Joints[JointType.HipCentre], skeleton.Joints[JointType.HipRight], context);                 // hip centre to right
            SkeletonDrawingHelper._DrawBone(skeleton.Joints[JointType.ShoulderCentre], skeleton.Joints[JointType.ShoulderLeft], context);        // shoulder centre to left
            SkeletonDrawingHelper._DrawBone(skeleton.Joints[JointType.ShoulderCentre], skeleton.Joints[JointType.ShoulderRight], context);       // shoulder centre to right

            // Left arm
            SkeletonDrawingHelper._DrawBone(skeleton.Joints[JointType.ShoulderLeft], skeleton.Joints[JointType.ElbowLeft], context);             // shoulder to elbow
            SkeletonDrawingHelper._DrawBone(skeleton.Joints[JointType.ElbowLeft], skeleton.Joints[JointType.WristLeft], context);                // elbow to wrist
            SkeletonDrawingHelper._DrawBone(skeleton.Joints[JointType.WristLeft], skeleton.Joints[JointType.HandLeft], context);                 // wrist to hand

            // Right arm
            SkeletonDrawingHelper._DrawBone(skeleton.Joints[JointType.ShoulderRight], skeleton.Joints[JointType.ElbowRight], context);            // shoulder to elbow
            SkeletonDrawingHelper._DrawBone(skeleton.Joints[JointType.ElbowRight], skeleton.Joints[JointType.WristRight], context);               // elbow to wrist
            SkeletonDrawingHelper._DrawBone(skeleton.Joints[JointType.WristRight], skeleton.Joints[JointType.HandRight], context);                // wrist to hand

            // Left leg
            SkeletonDrawingHelper._DrawBone(skeleton.Joints[JointType.HipLeft], skeleton.Joints[JointType.KneeLeft], context);                    // hip to knee
            SkeletonDrawingHelper._DrawBone(skeleton.Joints[JointType.KneeLeft], skeleton.Joints[JointType.AnkleLeft], context);                  // knee to ankle
            SkeletonDrawingHelper._DrawBone(skeleton.Joints[JointType.AnkleLeft], skeleton.Joints[JointType.FootLeft], context);                  // ankle to foot

            // Right leg
            SkeletonDrawingHelper._DrawBone(skeleton.Joints[JointType.HipRight], skeleton.Joints[JointType.KneeRight], context);                    // hip to knee
            SkeletonDrawingHelper._DrawBone(skeleton.Joints[JointType.KneeRight], skeleton.Joints[JointType.AnkleRight], context);                  // knee to ankle
            SkeletonDrawingHelper._DrawBone(skeleton.Joints[JointType.AnkleRight], skeleton.Joints[JointType.FootRight], context);                  // ankle to foot
        }

        private static void _DrawBone(Joint? j1, Joint? j2, DrawingContext context)
        {
            if (j1 == null || j2 == null)
            {
                return;
            }

            context.DrawLine(new Pen(Brushes.Green, 6), SkeletonDrawingHelper._MapSkeletonPointToScreen(j1.Value.X, j1.Value.Y, j1.Value.Z), SkeletonDrawingHelper._MapSkeletonPointToScreen(j2.Value.X, j2.Value.Y, j2.Value.Z));
        }


        /// <summary>
        /// Source: decompiled Microsoft.Kinect.dll
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        private static Point _MapSkeletonPointToScreen(float x, float y, float z)
        {
            Vector4 vector;
            float num, num2, num3;
            int num4, num5;

            vector.X = x;
            vector.Y = y;
            vector.Z = z;
            vector.W = 1f;

            SkeletonDrawingHelper._TransformSkeletonToDepthImage(vector, out num, out num2, out num3);
            SkeletonDrawingHelper._ResolutionToHeightWidth(out num4, out num5);

            return new Point
            {
                X = (int) ((num * num4) + 0.5f),
                Y = (int) ((num2 * num5) + 0.5f)
            };
        }

        // From decomplation
        private static void _TransformSkeletonToDepthImage(Vector4 vPoint, out float pfDepthX, out float pfDepthY, out float pfDepthValue)
        {
            if (vPoint.Z > float.Epsilon)
            {
                pfDepthX = 0.5f + ((vPoint.X * 285.63f) / (vPoint.Z * 320f));
                pfDepthY = 0.5f - ((vPoint.Y * 285.63f) / (vPoint.Z * 240f));
                pfDepthValue = vPoint.Z * 1000f;
            }
            else
            {
                pfDepthX = 0f;
                pfDepthY = 0f;
                pfDepthValue = 0f;
            }
        }

        // From decompilation
        private static void _ResolutionToHeightWidth(out int width, out int height)
        {
            width = 640;
            height = 480;
        }
    }
}