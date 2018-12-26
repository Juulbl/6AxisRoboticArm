using System;
using System.Collections.Generic;
using Gtk;

namespace RobotInterface
{
    public class Timeline
    {

        #region PROPERTIES

        public uint Duration
        {
            get;
            set;
        } = 10000;

        public int Width
        {
            get => this.DrawingArea.Allocation.Width;
            set
            {
                this.DrawingArea.WidthRequest = value;
                this.Draw();
            }
        }

        public int Height
        {
            get => this.DrawingArea.Allocation.Height;
            set
            {
                this.DrawingArea.HeightRequest = value;
                this.Draw();
            }
        }

        public Gtk.DrawingArea DrawingArea
        {
            get;
            private set;
        } = null;

        public List<TimelineKeyframe> Keyframes
        {
            get;
            private set;
        } = new List<TimelineKeyframe>();

        #endregion


        #region CONSTRUCTORS

        public Timeline(ref Gtk.DrawingArea drawingArea)
        {
            this.DrawingArea = drawingArea;

            //Set size of drawing area.
            this.Width = this.DrawingArea.Parent.Allocation.Width;
            this.Height = this.DrawingArea.Parent.Allocation.Height;
        }

        #endregion


        #region METHODS

        public void Draw()
        {
            //If width or height are lower than or equal to zero, return.
            if (this.Width <= 0 || this.Height <= 0) return;

            //Draw keyframes.
            foreach(TimelineKeyframe frame in this.Keyframes)
            { 
                
            }

        }

        #endregion
    }
}
