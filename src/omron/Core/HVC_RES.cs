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

using System.Collections.Generic;
namespace omron.HVC
{
    /// <summary>
    /// HVC execution result 
    /// </summary>
    public class HVC_RES
    {
        /// <summary>
        /// Detection result
        /// </summary>
        public class DetectionResult
        {
            private readonly HVC_RES Parent;

            /// <summary>
            /// Center x-coordinate
            /// </summary>
            public int PosX;
            /// <summary>
            /// Center y-coordinate
            /// </summary>
            public int PosY;
            /// <summary>
            /// Size
            /// </summary>
            public int Size;
            /// <summary>
            /// Degree of confidence
            /// </summary>
            public int Confidence;

            /// <summary>
            /// Constructor<br>
            /// [Description]<br>
            /// none<br>
            /// [Notes]<br>
            /// </summary>
            public DetectionResult(HVC_RES parent)
            {
                this.Parent = parent;
                PosX = -1;
                PosY = -1;
                Size = -1;
                Confidence = -1;
            }
        }

        /// <summary>
        /// Face Detection & Estimations results
        /// </summary>
        public class FaceResult : DetectionResult
        {
            private readonly HVC_RES Parent;

            /// <summary>
            /// Face direction
            /// </summary>
            public class DirResult
            {
                private readonly HVC_RES.FaceResult Parent;

                /// <summary>
                /// Yaw angle
                /// </summary>
                public int Yaw;
                /// <summary>
                /// Pitch angle
                /// </summary>
                public int Pitch;
                /// <summary>
                /// Roll angle
                /// </summary>
                public int Roll;
                /// <summary>
                /// Degree of confidence
                /// </summary>
                public int Confidence;

                /// <summary>
                /// Constructor<br>
                /// [Description]<br>
                /// none<br>
                /// [Notes]<br>
                /// </summary>
                public DirResult(HVC_RES.FaceResult parent)
                {
                    this.Parent = parent;
                    Yaw = -1;
                    Pitch = -1;
                    Roll = -1;
                    Confidence = -1;
                }
            }

            /// <summary>
            /// Age
            /// </summary>
            public class AgeResult
            {
                private readonly HVC_RES.FaceResult Parent;

                /// <summary>
                /// Age
                /// </summary>
                public int Age;
                /// <summary>
                /// Degree of confidence
                /// </summary>
                public int Confidence;

                /// <summary>
                /// Constructor<br>
                /// [Description]<br>
                /// none<br>
                /// [Notes]<br>
                /// </summary>
                public AgeResult(HVC_RES.FaceResult parent)
                {
                    this.Parent = parent;
                    Age = -1;
                    Confidence = -1;
                }
            }

            /// <summary>
            /// Gender
            /// </summary>
            public class GenResult
            {
                private readonly HVC_RES.FaceResult Parent;

                /// <summary>
                /// Gender
                /// </summary>
                public int Gender;
                /// <summary>
                /// Degree of confidence
                /// </summary>
                public int Confidence;

                /// <summary>
                /// Constructor<br>
                /// [Description]<br>
                /// none<br>
                /// [Notes]<br>
                /// </summary>
                public GenResult(HVC_RES.FaceResult parent)
                {
                    this.Parent = parent;
                    Gender = -1;
                    Confidence = -1;
                }
            }

            /// <summary>
            /// Gaze
            /// </summary>
            public class GazeResult
            {
                private readonly HVC_RES.FaceResult Parent;

                /// <summary>
                /// Yaw angle
                /// </summary>
                public int GazeLR;
                /// <summary>
                /// Pitch angle
                /// </summary>
                public int GazeUD;

                /// <summary>
                /// Constructor<br>
                /// [Description]<br>
                /// none<br>
                /// [Notes]<br>
                /// </summary>
                public GazeResult(HVC_RES.FaceResult parent)
                {
                    this.Parent = parent;
                    GazeLR = -1;
                    GazeUD = -1;
                }
            }

            /// <summary>
            /// Blink
            /// </summary>
            public class BlinkResult
            {
                private readonly HVC_RES.FaceResult Parent;

                /// <summary>
                /// Left eye blink result
                /// </summary>
                public int RatioL;
                /// <summary>
                /// Right eye blink result
                /// </summary>
                public int RatioR;

                /// <summary>
                /// Constructor<br>
                /// [Description]<br>
                /// none<br>
                /// [Notes]<br>
                /// </summary>
                public BlinkResult(HVC_RES.FaceResult parent)
                {
                    this.Parent = parent;
                    RatioL = -1;
                    RatioR = -1;
                }
            }

            /// <summary>
            /// Expression
            /// </summary>
            public class ExpResult
            {
                private readonly HVC_RES.FaceResult Parent;

                /// <summary>
                /// Expression
                /// </summary>
                public int Expression;
                /// <summary>
                /// Score
                /// </summary>
                public int Score;
                /// <summary>
                /// Negative-positive degree
                /// </summary>
                public int Degree;

                /// <summary>
                /// Constructor<br>
                /// [Description]<br>
                /// none<br>
                /// [Notes]<br>
                /// </summary>
                public ExpResult(HVC_RES.FaceResult parent)
                {
                    this.Parent = parent;
                    Expression = -1;
                    Score = -1;
                    Degree = -1;
                }
            }

            /// <summary>
            /// Face direction estimation result
            /// </summary>
            public DirResult Dir;
            /// <summary>
            /// Age Estimation result
            /// </summary>
            public AgeResult Age;
            /// <summary>
            /// Gender Estimation result
            /// </summary>
            public GenResult Gen;
            /// <summary>
            /// Gaze Estimation result
            /// </summary>
            public GazeResult Gaze;
            /// <summary>
            /// Blink Estimation result
            /// </summary>
            public BlinkResult Blink;
            /// <summary>
            /// Expression Estimation result
            /// </summary>
            public ExpResult Exp;

            /// <summary>
            /// Constructor<br>
            /// [Description]<br>
            /// none<br>
            /// [Notes]<br>
            /// </summary>
            public FaceResult(HVC_RES parent)
                : base(parent)
            {
                this.Parent = parent;
                Dir = new DirResult(this);
                Age = new AgeResult(this);
                Gen = new GenResult(this);
                Gaze = new GazeResult(this);
                Blink = new BlinkResult(this);
                Exp = new ExpResult(this);
            }
        }

        /// <summary>
        /// Execution flag
        /// </summary>
        public int ExecutedFunc;
        /// <summary>
        /// Human Body Detection results
        /// </summary>
        public List<DetectionResult> Body;
        /// <summary>
        /// Hand Detection results
        /// </summary>
        public List<DetectionResult> Hand;
        /// <summary>
        /// Face Detection, Estimations results
        /// </summary>
        public List<FaceResult> Face;

        /// <summary>
        /// Constructor<br>
        /// [Description]<br>
        /// none<br>
        /// [Notes]<br>
        /// </summary>
        public HVC_RES()
        {
            Body = new List<DetectionResult>();
            Hand = new List<DetectionResult>();
            Face = new List<FaceResult>();
        }
    }

}