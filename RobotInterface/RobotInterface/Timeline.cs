using System;
using System.Linq;
using System.Collections.Generic;
using Gtk;

namespace RobotInterface
{
    public class Timeline
    {
        #region FIELDS

        private Robot robot;
        private List<Keyframe> keyframes = new List<Keyframe>();
        private Gtk.ListStore framesListStore;
        private Gtk.TreeView treeView;
        private int selectKeyframeIndex = -1;

        #endregion


        #region PROPERTIES

        public List<Keyframe> Keyframes
        {
            get => this.keyframes;
            private set => this.keyframes = value;
        }

        public int SelectKeyframeIndex
        {
            get => this.selectKeyframeIndex;
            private set
            {
                //If value is higher than the number of keyframes, set value to last keyframe.
                if (value >= this.Keyframes.Count) value = this.Keyframes.Count - 1;

                //Set selected keyframe.
                this.selectKeyframeIndex = value;
            }
        }

        #endregion


        #region CONSTRUCTORS

        public Timeline(ref Gtk.TreeView treeView, ref Robot robot)
        {
            this.robot = robot;

            this.treeView = treeView;

            //Create list store.
            this.framesListStore = new ListStore(
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

            //Set sorting.
            this.framesListStore.SetSortColumnId(8, SortType.Ascending);

            //Set model.
            this.treeView.Model = this.framesListStore;

            //Create renderer.
            var cellView = new CellRendererText();

            //Add columns.
            this.treeView.AppendColumn("Name", cellView, "text", 0);
            this.treeView.AppendColumn("Actuator 0", cellView, "text", 1);
            this.treeView.AppendColumn("Actuator 1", cellView, "text", 2);
            this.treeView.AppendColumn("Actuator 2", cellView, "text", 3);
            this.treeView.AppendColumn("Actuator 3", cellView, "text", 4);
            this.treeView.AppendColumn("Actuator 4", cellView, "text", 5);
            this.treeView.AppendColumn("Actuator 5", cellView, "text", 6);
            this.treeView.AppendColumn("Actuator 6", cellView, "text", 7);
            this.treeView.AppendColumn("Time (Milliseconds)", cellView, "text", 8);
        }

        #endregion


        #region METHODS

        private void AppendListStore(ref Keyframe frame)
        {
            this.framesListStore.AppendValues(
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

            this.SetSelectedKeyframeIndex(this.Keyframes.Count - 1);
        }

        private void SortKeyframes()
        {
            this.Keyframes = this.Keyframes.OrderBy(f => f.time).ToList<Keyframe>();
        }

        private int GetKeyframeIndex(UInt32 time)
        {
            for (int i = 0; i < this.keyframes.Count; i++)
            {
                if (this.keyframes[i].time == time) return i;
            }

            return -1;
        }

        public bool UpdateKeyframe(int index, Keyframe keyframe)
        {
            //Does time change???
            bool timeChanges = (this.keyframes[index].time != keyframe.time);

            //If (new)time does match another item's time, set time to old time.
            if (this.GetKeyframeIndex(keyframe.time) >= 0)
            {
                keyframe.time = this.Keyframes[index].time;
            }

            //Update keyframe.
            this.Keyframes[index] = keyframe;

            //Set visible keyframe.
            TreeIter iter;
            this.framesListStore.GetIter(out iter, new TreePath(new int[1] { this.SelectKeyframeIndex }));

            this.framesListStore.SetValues(
                iter,
                keyframe.name,
                keyframe.actuatorValues[0],
                keyframe.actuatorValues[1],
                keyframe.actuatorValues[2],
                keyframe.actuatorValues[3],
                keyframe.actuatorValues[4],
                keyframe.actuatorValues[5],
                keyframe.actuatorValues[6],
                keyframe.time
                );

            //If time has changed.
            if (timeChanges)
            {
                //Sort keyframes.
                this.SortKeyframes();

                //Get item.
                this.SetSelectedKeyframeIndex(this.GetKeyframeIndex(keyframe.time));
            }

            return true;
        }

        public void SetSelectedKeyframeIndex(int index)
        {
            this.SelectKeyframeIndex = index;
            this.treeView.ActivateRow(new TreePath(new int[1] { this.SelectKeyframeIndex }), this.treeView.Columns[0]);
            this.UpdateAllRobotServos();
        }

        public Nullable<Keyframe> GetSelectedKeyframe()
        {
            if (this.SelectKeyframeIndex < 0) return null;
            return this.Keyframes[this.SelectKeyframeIndex];
        }

        public bool UpdateSelectedKeyframe(Keyframe keyframe)
        {
            if (this.SelectKeyframeIndex < 0) return false;
            return this.UpdateKeyframe(this.SelectKeyframeIndex, keyframe);
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
                keyframe.name = "Frame";
            }
            else
            {
                keyframe.name = name;
            }

            //Set time.
            if (!time.HasValue)
            {
                keyframe.time = (numOfFrames > 0) ? this.Keyframes[numOfFrames - 1].time + 1000 : 0;
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

        public void UpdateAllRobotServos()
        {
            Nullable<Keyframe> keyframe = this.GetSelectedKeyframe();
            if (!keyframe.HasValue) return;

            for(int i = 0; i < this.robot.Servos.Length; i++)
            {
                this.robot.SetServoAngle(i, keyframe.Value.actuatorValues[i]);
            }
        }

        #endregion

    }
}
