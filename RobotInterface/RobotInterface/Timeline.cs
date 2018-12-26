using System;
using System.Linq;
using System.Collections.Generic;
using Gtk;

namespace RobotInterface
{
    public class Timeline
    {
        #region FIELDS

        private List<Keyframe> keyframes = new List<Keyframe>();
        private Gtk.ListStore framesListStore;
        private Gtk.TreeView treeView;

        #endregion


        #region PROPERTIES

        public List<Keyframe> Keyframes 
        {
            get => this.keyframes;
            private set => this.keyframes = value;
        }

        #endregion


        #region CONSTRUCTORS

        public Timeline(ref Gtk.TreeView treeView)
        {
            this.treeView = treeView;

            this.framesListStore = new ListStore(
                typeof(int),
                typeof(string),
                typeof(float),
                typeof(float),
                typeof(float),
                typeof(float),
                typeof(float),
                typeof(float),
                typeof(float),
                typeof(UInt32)
            );

            this.treeView.Model = this.framesListStore;

            var cellView = new CellRendererText();

            this.treeView.AppendColumn("ID", cellView, "text", 0);
            this.treeView.AppendColumn("Name", cellView, "text", 1);
            this.treeView.AppendColumn("Actuator 0", cellView, "text", 2);
            this.treeView.AppendColumn("Actuator 1", cellView, "text", 3);
            this.treeView.AppendColumn("Actuator 2", cellView, "text", 4);
            this.treeView.AppendColumn("Actuator 3", cellView, "text", 5);
            this.treeView.AppendColumn("Actuator 4", cellView, "text", 6);
            this.treeView.AppendColumn("Actuator 5", cellView, "text", 7);
            this.treeView.AppendColumn("Actuator 6", cellView, "text", 8);
            this.treeView.AppendColumn("Time (Milliseconds)", cellView, "text", 9);
        }

        #endregion


        #region METHODS

        private void AppendListStore(ref Keyframe frame)
        {
            this.framesListStore.AppendValues(
                this.Keyframes.Count,
                frame.name,
                frame.actuatorValues[0],
                frame.actuatorValues[1],
                frame.actuatorValues[2],
                frame.actuatorValues[3],
                frame.actuatorValues[4],
                frame.actuatorValues[5],
                frame.actuatorValues[6],
                frame.time
                );

            this.SetSelectedKeyframe(this.Keyframes.Count - 1);
        }

        private void SortKeyframes()
        {
            this.Keyframes.OrderBy(x => x.time);
        }

        private int GetKeyframeIndex(string name)
        { 
            for(int i = 0; i < this.keyframes.Count; i++)
            {
                if (this.keyframes[i].name == name) return i;
            }

            return -1;
        }

        public void UpdateKeyframe(int index, Keyframe keyframe)
        {
            //Does time change???
            bool timeChanges = (this.keyframes[index].time != keyframe.time);

            //Update keyframe.
            this.Keyframes[index] = keyframe;

            //If time does not change return.
            if (!timeChanges) return;

            //Sort keyframes.
            this.SortKeyframes();

            //Get keyframe index.
            this.GetKeyframeIndex(keyframe.name);
        }

        public void SetSelectedKeyframe(int index)
        {
            this.treeView.ActivateRow(new TreePath(new int[1] { index }), this.treeView.Columns[0]);
        }

        public int AddKeyframe(string name, Nullable<UInt32> time, params float[] actuatorValues)
        {
            //Get num of frames.
            int numOfFrames = this.Keyframes.Count;

            //Create new keyframe.
            Keyframe keyframe;

            //Set name.
            if (name == null)
            {
                keyframe.name = $"Frame({this.Keyframes.Count})";
            }
            else
            {
                keyframe.name = name;
            }

            //Set time.
            if (time == null)
            {
                keyframe.time = (numOfFrames > 0) ? this.Keyframes[numOfFrames - 1].time + 1 : 0;
            }
            else
            {
                keyframe.time = time.Value;
            }

            //Set actuators.
            keyframe.actuatorValues = new float[actuatorValues.Length];
            for (int i = 0; i < actuatorValues.Length; i++)
            {
                keyframe.actuatorValues[i] = actuatorValues[i];
            }

            //Add keyframe.
            this.Keyframes.Add(keyframe);

            //Update list store.
            this.AppendListStore(ref keyframe);

            //Return index where frame is inserted.
            return numOfFrames;
        }

        #endregion

    }
}
