/*
 * Copyright (C) 2014-2015 HATSUNE, Akira (Original OMRON Corporation)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace omron.HVC
{

    /// <summary>
    /// HVC parameters
    /// </summary>
    public class HVC_PRM
    {
        /// <summary>
        /// Detection parameters
        /// </summary>
        public class DetectionParam
        {
            private readonly HVC_PRM Parent;

            /// <summary>
            /// Minimum detection size
            /// </summary>
            public int MinSize;
            /// <summary>
            /// Maximum detection size
            /// </summary>
            public int MaxSize;
            /// <summary>
            /// Degree of confidence
            /// </summary>
            public int Threshold;

            /// <summary>
            /// Constructor<br>
            /// [Description]<br>
            /// none<br>
            /// [Notes]<br>
            /// </summary>
            public DetectionParam(HVC_PRM parent)
            {
                this.Parent = parent;
                this.MinSize = 40;
                this.MaxSize = 480;
                this.Threshold = 500;
            }
        }

        /// <summary>
        /// Face Detection parameters
        /// </summary>
        public class FaceParam : DetectionParam
        {
            private readonly HVC_PRM Parent;

            /// <summary>
            /// Facial pose
            /// </summary>
            public int Pose;
            /// <summary>
            /// Roll angle
            /// </summary>
            public int Angle;

            /// <summary>
            /// Constructor<br>
            /// [Description]<br>
            /// none<br>
            /// [Notes]<br>
            /// </summary>
            public FaceParam(HVC_PRM parent)
                : base(parent)
            {
                this.Parent = parent;
                this.Pose = 40;
                this.Angle = 480;
            }
        }

        /// <summary>
        /// Camera angle
        /// </summary>
        public int CameraAngle;
        /// <summary>
        /// Human Body Detection parameters
        /// </summary>
        public DetectionParam Body;
        /// <summary>
        /// Hand Detection parameters
        /// </summary>
        public DetectionParam Hand;
        /// <summary>
        /// Face Detection parameters
        /// </summary>
        public FaceParam Face;

        /// <summary>
        /// Constructor<br>
        /// [Description]<br>
        /// none<br>
        /// [Notes]<br>
        /// </summary>
        public HVC_PRM()
        {
            CameraAngle = 0;
            Body = new DetectionParam(this);
            Hand = new DetectionParam(this);
            Face = new FaceParam(this);
        }
    }

}