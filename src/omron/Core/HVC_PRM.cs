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
        /**
         * Pose enum
         */
        public enum HVC_FACE_POSE
        {
            HVC_FACE_POSE_FRONT = 0,
            HVC_FACE_POSE_HALF_PROFIL = 1,
            HVC_FACE_POSE_PROFILE = 2
        }

        /**
         * Angle enum
         */
        public enum HVC_FACE_ANGLE
        {
            HVC_FACE_ANGLE_15 = 0,
            HVC_FACE_ANGLE_45 = 1
        }

        /**
         * Camera angle enum
         */
        public enum HVC_CAMERA_ANGLE
        {
            HVC_CAMERA_ANGLE_0 = 0,
            HVC_CAMERA_ANGLE_90 = 1,
            HVC_CAMERA_ANGLE_180 = 2,
            HVC_CAMERA_ANGLE_270 = 3
        }

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
            public HVC_FACE_POSE Pose;
            /// <summary>
            /// Roll angle
            /// </summary>
            public HVC_FACE_ANGLE Angle;

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
                this.Pose = HVC_FACE_POSE.HVC_FACE_POSE_FRONT;
                this.Angle = HVC_FACE_ANGLE.HVC_FACE_ANGLE_15;
            }
        }

        /// <summary>
        /// Camera angle
        /// </summary>
        public HVC_CAMERA_ANGLE CameraAngle;
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
            CameraAngle = HVC_CAMERA_ANGLE.HVC_CAMERA_ANGLE_0;
            Body = new DetectionParam(this);
            Hand = new DetectionParam(this);
            Face = new FaceParam(this);
        }
    }

}