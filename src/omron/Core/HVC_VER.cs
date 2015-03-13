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

using System;

namespace omron.HVC
{

    /// <summary>
    /// Version (GetVersion result)
    /// </summary>
    public class HVC_VER
    {
        /// <summary>
        /// Version string
        /// </summary>
        public char[] Version;
        /// <summary>
        /// Major version number
        /// </summary>
        public byte Major;
        /// <summary>
        /// Minor version number
        /// </summary>
        public byte Minor;
        /// <summary>
        /// Release version number
        /// </summary>
        public byte Relese;
        /// <summary>
        /// Revision number
        /// </summary>
        public int Rev;

        /// <summary>
        /// Constructor<br>
        /// [Description]<br>
        /// none<br>
        /// [Notes]<br>
        /// </summary>
        public HVC_VER()
        {
            Version = new char[12];
            for (int i = 0; i < Version.Length; i++)
            {
                Version[i] = (char)0;
            }
            Major = 0;
            Minor = 0;
            Relese = 0;
            Rev = 0;
        }

        /// <summary>
        /// Get version struct size<br>
        /// [Description]<br>
        /// Return struct size when obtaining HVC version number<br> </summary>
        /// <returns> int version struct size<br> </returns>
        public virtual int GetSize()
        {
            return Version.Length + 3 + 4;
        }

        /// <summary>
        /// Get version string<br>
        /// [Description]<br>
        /// Return HVC version number as a string<br> </summary>
        /// <returns> String version number string<br> </returns>
        public virtual string GetString()
        {
            return Convert.ToString(Major) + '.' + Convert.ToString(Minor) + '.' + Convert.ToString(Relese) + '.' + Convert.ToString(Rev) + '[' + Convert.ToString(Version) + ']';
        }
    }

}