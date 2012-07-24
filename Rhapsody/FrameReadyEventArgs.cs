using System;

namespace org.btg.Star.Rhapsody
{
    public abstract class FrameReadyEventArgs : EventArgs
    {
        public Frame Frame { get; set; }

        public DateTime Time { get; set; }

        public FrameReadyEventArgs()
        {
            this.Time = DateTime.Now;
        }

        public FrameReadyEventArgs(Frame _frame)
        {
            this.Frame = _frame;
            this.Time = DateTime.Now;
        }
    }
}