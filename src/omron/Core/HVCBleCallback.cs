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

    internal interface BleCallbackInterface
    {
        void OnConnected();
        void OnDisconnected();
        void OnPostGetDeviceName(byte[] value);
    }

    public class HVCBleCallback : HVCCallback, BleCallbackInterface
    {
        public override void OnPostSetParam(int nRet, byte outStatus)
        {
            // TODO Auto-generated method stub
        }

        public override void OnPostGetParam(int nRet, byte outStatus)
        {
            // TODO Auto-generated method stub
        }

        public override void OnPostGetVersion(int nRet, byte outStatus)
        {
            // TODO Auto-generated method stub
        }

        public override void OnPostExecute(int nRet, byte outStatus)
        {
            // TODO Auto-generated method stub
        }

        public virtual void OnConnected()
        {
            // TODO Auto-generated method stub
        }

        public virtual void OnDisconnected()
        {
            // TODO Auto-generated method stub
        }

        public virtual void OnPostGetDeviceName(byte[] value)
        {
            // TODO Auto-generated method stub
        }
    }

}