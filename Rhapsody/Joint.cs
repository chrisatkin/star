using System;

namespace org.btg.Star.Rhapsody
{
    public struct Joint
    {
        public float X;

        public float Y;

        public float Z;

        // I am not yet convinced of the need for Joints to have a type. We shall see.
        /*public JointType type
        {
            get;
            set;
        }*/

        public Joint(float _x, float _y, float _z/*, JointType _type*/)
        {
            X = _x;
            Y = _y;
            Z = _z;
            //this.type = _type;
        }
    }
}