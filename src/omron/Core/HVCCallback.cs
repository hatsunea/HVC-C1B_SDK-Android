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

    public abstract class HVCCallback
    {
        public abstract void OnPostSetParam(int nRet, byte outStatus);
        public abstract void OnPostGetParam(int nRet, byte outStatus);
        public abstract void OnPostGetVersion(int nRet, byte outStatus);
        public abstract void OnPostExecute(int nRet, byte outStatus);
    }

}