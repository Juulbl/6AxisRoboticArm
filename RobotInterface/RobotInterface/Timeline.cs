using System;
using System.Linq;
using System.Collections.Generic;
using Gtk;

namespace RobotInterface
{
    public class Timeline
    {
        #region FIELDS

        //GUI components.
        private Robot robot;
        private Gtk.VBox framePropertiesPanel;
        private Gtk.VBox framesPanel;
        private Gtk.TreeView treeView;

        //Frames and frames list store.
        private List<Keyframe> keyframes = new List<Keyframe>();
        private Gtk.ListStore framesListStore;
        private int selectKeyframeIndex = -1;

        //Playback.
        private bool isPlaying = false;
        private bool repeat = false;
        private double currentTime = 0;

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
            set
            {
                //If value is higher than the number of keyframes, set value to last keyframe.
                if (value >= this.Keyframes.Count) value = this.Keyframes.Count - 1;

                if (value >= 0)
                {

                    //Set selected keyframe.
                    this.selectKeyframeIndex = value;

                    //Set activated row.
                    this.treeView.ActivateRow(new TreePath(new int[1] { this.SelectKeyframeIndex }), this.treeView.Columns[0]);

                    //Update all servos.
                    this.UpdateAllRobotServos();

                }

                //Set frame properties sensitivity.
                this.framePropertiesPanel.Sensitive = (!this.IsPlaying && value >= 0);
            }
        }

        public bool IsPlaying
        {
            get => this.isPlaying;
            private set
            {
                //If no keyframes, return.
                if (this.keyframes.Count == 0) return;

                //Set sensitivity.
                this.framesPanel.Sensitive = !value;

                //Set frame properties sensitivity.
                this.framePropertiesPanel.Sensitive = (!value && this.SelectKeyframeIndex >= 0);

                //Set is playing.
                this.isPlaying = value;
            }
        }

        public bool Repeat
        {
            get => this.repeat;
            set => this.repeat = value;
        }

        public double CurrentTime
        {
            get => this.currentTime;
            set => this.currentTime = value;
        }

        #endregion


        #region CONSTRUCTORS

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RobotInterface.Timeline"/> class.
        /// </summary>
        /// <param name="treeView">Tree view.</param>
        /// <param name="framesPanel">Frames panel.</param>
        /// <param name="framePropertiesPanel">Frame properties panel.</param>
        /// <param name="robot">Robot.</param>
        public Timeline(ref Gtk.TreeView treeView, ref Gtk.VBox framesPanel, ref Gtk.VBox framePropertiesPanel, ref Robot robot)
        {
            //Set frames panel.
            this.framesPanel = framesPanel;

            //Set frame properties panel.
            this.framePropertiesPanel = framePropertiesPanel;

            //Set robot.
            this.robot = robot;

            //Set tree view.
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

            //Set frame properties panel non-sensitive.
            this.framePropertiesPanel.Sensitive = false;

            //Set tree view sensitive.
            this.framesPanel.Sensitive = true;
        }

        #endregion


        #region METHODS

        /// <summary>
        /// Play timeline.
        /// </summary>
        public void Play()
        {
            this.IsPlaying = true;
        }

        /// <summary>
        /// Pause timeline.
        /// </summary>
        public void Pause()
        {
            this.IsPlaying = false;
        }

        /// <summary>
        /// Stop timeline.
        /// </summary>
        public void Stop()
        {
            this.IsPlaying = false;
            this.CurrentTime = 0; 
        }

        /// <summary>
        /// Update timeline.
        /// </summary>
        /// <param name="deltaTime">Delta time (milliseconds).</param>
        public void Update(ref double deltaTime)
        {
            //If if not playing, return.
            if (!this.IsPlaying) return;

            //Update current time.
            this.CurrentTime += deltaTime;

            //If time is higher than the last keyframe.
            if (this.CurrentTime > this.Keyframes[this.Keyframes.Count - 1].time)
            { 
                //If not repeating, stop playing
                if(!this.Repeat)
                {
                    this.Stop();
                }
                else
                {
                    this.CurrentTime = 0;
                }

                return;
            }

            //Get keyframes the current time is between.
            Nullable<Keyframe> startFrame = null;
            Nullable<Keyframe> endFrame = null;
            int keyframeIndex = 0;
            while(keyframeIndex < this.Keyframes.Count - 1)
            {
                //If next keyframe has a time lower than the current time.
                if(this.CurrentTime <= this.Keyframes[keyframeIndex + 1].time)
                {
                    //Set start and end keyframes.
                    startFrame = this.Keyframes[keyframeIndex];
                    endFrame = this.Keyframes[keyframeIndex + 1];

                    //Break.
                    break;
                }

                //Increment keyframes.
                keyframeIndex++;
            }

            //If one of the selected keyframes does not have a value.
            if (!startFrame.HasValue || !endFrame.HasValue) return;

            //Get normalized position between the selected keyframes.
            double normalizedPos = (this.CurrentTime - startFrame.Value.time) / (endFrame.Value.time - startFrame.Value.time);

            //Update all servos.
            for(int i = 0; i < this.robot.Servos.Length; i++)
            {
                this.robot.SetServoAngle(
                        i, 
                        this.GetValueBetweenAngles(
                            startFrame.Value.actuatorValues[i], 
                            endFrame.Value.actuatorValues[i],
                            normalizedPos
                        )
                    );
            }
        }

        /// <summary>
        /// Gets the value between angles.
        /// </summary>
        /// <returns>The angle between angles.</returns>
        /// <param name="angle1">Angle1.</param>
        /// <param name="angle2">Angle2.</param>
        /// <param name="normalizedPosition">Normalized position.</param>
        private float GetValueBetweenAngles(float angle1, float angle2, double normalizedPosition)
        {
            return (float)(angle1 + ((angle2 - angle1) * normalizedPosition));
        }

        /// <summary>
        /// Appends the list store.
        /// </summary>
        /// <param name="frame">Frame.</param>
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

            this.SelectKeyframeIndex = this.Keyframes.Count - 1;
        }

        /// <summary>
        /// Sorts the keyframes.
        /// </summary>
        private void SortKeyframes()
        {
            this.Keyframes = this.Keyframes.OrderBy(f => f.time).ToList<Keyframe>();
        }

        /// <summary>
        /// Gets the index of a keyframe by its time.
        /// </summary>
        /// <returns>The keyframe index.</returns>
        /// <param name="time">Time.</param>
        private int GetKeyframeIndex(UInt32 time)
        {
            for (int i = 0; i < this.keyframes.Count; i++)
            {
                if (this.keyframes[i].time == time) return i;
            }

            return -1;
        }

        /// <summary>
        /// Deletes the keyframe at index.
        /// </summary>
        /// <returns><c>true</c>, if keyframe was deleted, <c>false</c> otherwise.</returns>
        /// <param name="index">Index.</param>
        public bool DeleteKeyframe(int index)
        {
            //If out of range, return.
            if (index < 0 || index >= this.Keyframes.Count) return false;

            //Get keyframe  itterator.
            TreeIter iter;
            this.framesListStore.GetIter(out iter, new TreePath(new int[1] { index }));

            //Remove item.
            this.framesListStore.Remove(ref iter);

            //Remove item from list.
            this.Keyframes.RemoveAt(index);

            //Reset selected item.
            this.SelectKeyframeIndex = this.SelectKeyframeIndex;

            return true;
        }

        /// <summary>
        /// Updates the keyframe at index.
        /// </summary>
        /// <returns><c>true</c>, if keyframe was updated, <c>false</c> otherwise.</returns>
        /// <param name="index">Index.</param>
        /// <param name="keyframe">Keyframe.</param>
        public bool UpdateKeyframe(int index, Keyframe keyframe)
        {
            //If out of range, return.
            if (index < 0 && index >= this.Keyframes.Count) return false;

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
            this.framesListStore.GetIter(out iter, new TreePath(new int[1] { index }));

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
                this.SelectKeyframeIndex = this.GetKeyframeIndex(keyframe.time);
            }

            return true;
        }

        /// <summary>
        /// Gets the selected keyframe.
        /// </summary>
        /// <returns>The selected keyframe.</returns>
        public Nullable<Keyframe> GetSelectedKeyframe()
        {
            if (this.SelectKeyframeIndex < 0) return null;
            return this.Keyframes[this.SelectKeyframeIndex];
        }

        /// <summary>
        /// Updates the selected keyframe.
        /// </summary>
        /// <returns><c>true</c>, if selected keyframe was updated, <c>false</c> otherwise.</returns>
        /// <param name="keyframe">Keyframe.</param>
        public bool UpdateSelectedKeyframe(Keyframe keyframe)
        {
            if (this.SelectKeyframeIndex < 0) return false;
            return this.UpdateKeyframe(this.SelectKeyframeIndex, keyframe);
        }

        /// <summary>
        /// Adds a new keyframe.
        /// </summary>
        /// <returns>The keyframe.</returns>
        /// <param name="name">Name.</param>
        /// <param name="time">Time.</param>
        /// <param name="actuatorValues">Actuator values.</param>
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

        /// <summary>
        /// Updates all robot servos.
        /// </summary>
        public void UpdateAllRobotServos()
        {
            //If is playing, return.
            if (this.IsPlaying) return;

            //Get selected keyframe.
            Nullable<Keyframe> keyframe = this.GetSelectedKeyframe();
            if (!keyframe.HasValue) return;

            //Update all servos.
            for(int i = 0; i < this.robot.Servos.Length; i++)
            {
                this.robot.SetServoAngle(i, keyframe.Value.actuatorValues[i]);
            }
        }

        #endregion

    }
}
