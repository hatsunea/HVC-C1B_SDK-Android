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

using Android.Bluetooth;
using Android.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace omron.HVC
{
    internal interface BleInterface
    {
        void Connect(Context context, BluetoothDevice device);
        void Disconnect();
        int SetDeviceName(byte[] value);
        int GetDeviceName(byte[] value);
    }

    /// <summary>
    /// HVC-C BLE Model<br>
    /// [Description]<br>
    /// HVC subclass, connects HVC to Bluetooth<br>
    /// 
    /// </summary>
    public class HVC_BLE : HVC, BleInterface
    {
        public const int STATE_DISCONNECTED = 0;
        public const int STATE_CONNECTING = 1;
        public const int STATE_CONNECTED = 2;
        public const int STATE_BUSY = 3;

        private int Status = STATE_DISCONNECTED;
        private byte[] TxName = null;
        private List<byte?> TxValue = null;

        private HVCBleCallback Callback = null;
        private BluetoothDevice BtDevice = null;
        private BleDeviceService Service = null;

        private const string TAG = "HVC_BLE";

        private readonly object SyncObject = new object();

        /// <summary>
        /// HVC_BLE constructor<br>
        /// [Description]<br>
        /// Set HVC_BLE to new to automatically connect to Bluetooth device specified with btDevice<br> </summary>
        /// <param name="mainAct"> Activity object<br> </param>
        public HVC_BLE()
            : base()
        {
            GattCallback = new BleCallbackAnonymousInnerClassHelper(this);
            Status = STATE_DISCONNECTED;
            TxValue = new List<byte?>();
        }

        /// <summary>
        /// HVC_BLE finalizer<br>
        /// [Description]<br>
        /// MUST be called when ending<br> </summary>
        /// <exception cref="Throwable">  </exception>
        public new void Dispose()
        {
            Status = STATE_DISCONNECTED;
            if (Service != null)
            {
                Service.Close();
            }
            Service = null;
        }

        /// <summary>
        /// HVC_BLE verify status<br>
        /// [Description]<br>
        /// Verify HVC_BLE device status<br> </summary>
        /// <returns> boolean true:function executable, false:function non-executable<br> </returns>
        public override bool IsBusy()
        {
            // TODO Auto-generated method stub
            if (Status != STATE_CONNECTED)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Execute HVC functions<br>
        /// [Description]<br>
        /// Execute each HVC function. Store results in HVC_BLE.Result<br> </summary>
        /// <param name="inExec"> execution flag<br> </param>
        /// <param name="outStatus"> HVC execution result status<br> </param>
        /// <returns> int execution result error code <br> </returns>
        public async override Task<int> Execute(int inExec, HVC_RES res)
        {
            if (BtDevice == null)
            {
                Debug.WriteLine(TAG, "execute() : HVC_ERROR_NODEVICES");
                return HVC_ERROR_NODEVICES;
            }
            if (Service == null || Service.GetmConnectionState() != BleDeviceService.STATE_CONNECTED)
            {
                Debug.WriteLine(TAG, "execute() : HVC_ERROR_DISCONNECTED");
                return HVC_ERROR_DISCONNECTED;
            }
            if (Status > STATE_CONNECTED)
            {
                Debug.WriteLine(TAG, "execute() : HVC_ERROR_BUSY");
                return HVC_ERROR_BUSY;
            }

            Status = STATE_BUSY;

            await Task.Run(() =>
            {
                int nRet = HVC_NORMAL;
                byte[] outStatus = new byte[1];
                nRet = Execute(30000, inExec, outStatus, res);
                if (Callback != null)
                {
                    Callback.OnPostExecute(nRet, outStatus[0]);
                }

                if (Status == STATE_BUSY)
                {
                    Status = STATE_CONNECTED;
                }
            });

            //Thread t = new Thread(new ThreadStart(() => {
            //    int nRet = HVC_NORMAL;
            //    byte[] outStatus = new byte[1];
            //    nRet = Execute(10000, inExec, outStatus, res);
            //    if (mCallback != null)
            //    {
            //        mCallback.onPostExecute(nRet, outStatus[0]);
            //    }

            //    if (mStatus == STATE_BUSY)
            //    {
            //        mStatus = STATE_CONNECTED;
            //    }
            //}));

            //t.Start();
            Debug.WriteLine(TAG, "execute() : HVC_NORMAL");
            return HVC_NORMAL;
        }

        public async override Task<int> SetParam(HVC_PRM prm)
        {
            if (BtDevice == null)
            {
                Debug.WriteLine(TAG, "setParam() : HVC_ERROR_NODEVICES");
                return HVC_ERROR_NODEVICES;
            }
            if (Service == null || Service.GetmConnectionState() != BleDeviceService.STATE_CONNECTED)
            {
                Debug.WriteLine(TAG, "setParam() : HVC_ERROR_DISCONNECTED");
                return HVC_ERROR_DISCONNECTED;
            }
            if (Status > STATE_CONNECTED)
            {
                Debug.WriteLine(TAG, "setParam() : HVC_ERROR_BUSY");
                return HVC_ERROR_BUSY;
            }

            Status = STATE_BUSY;

            await Task.Run(() =>
            {
                int nRet = HVC_NORMAL;
                byte[] outStatus = new byte[1];
                nRet = SetCameraAngle(10000, outStatus, prm);
                if (nRet == HVC_NORMAL && outStatus[0] == 0)
                {
                    nRet = SetThreshold(10000, outStatus, prm);
                }
                if (nRet == HVC_NORMAL && outStatus[0] == 0)
                {
                    nRet = SetSizeRange(10000, outStatus, prm);
                }
                if (nRet == HVC_NORMAL && outStatus[0] == 0)
                {
                    nRet = SetFaceDetectionAngle(10000, outStatus, prm);
                }
                if (Callback != null)
                {
                    Callback.OnPostSetParam(nRet, outStatus[0]);
                }

                if (Status == STATE_BUSY)
                {
                    Status = STATE_CONNECTED;
                }
            });

            Debug.WriteLine(TAG, "setParam() : HVC_NORMAL");
            return HVC_NORMAL;
        }

        public async override Task<int> GetParam(HVC_PRM prm)
        {
            if (BtDevice == null)
            {
                Debug.WriteLine(TAG, "getParam() : HVC_ERROR_NODEVICES");
                return HVC_ERROR_NODEVICES;
            }
            if (Service == null || Service.GetmConnectionState() != BleDeviceService.STATE_CONNECTED)
            {
                Debug.WriteLine(TAG, "getParam() : HVC_ERROR_DISCONNECTED");
                return HVC_ERROR_DISCONNECTED;
            }
            if (Status > STATE_CONNECTED)
            {
                Debug.WriteLine(TAG, "getParam() : HVC_ERROR_BUSY");
                return HVC_ERROR_BUSY;
            }

            Status = STATE_BUSY;

            await Task.Run(() =>
            {
                int nRet = HVC_NORMAL;
                byte[] outStatus = new byte[1];
                if (nRet == HVC_NORMAL)
                {
                    nRet = GetCameraAngle(10000, outStatus, prm);
                }
                if (nRet == HVC_NORMAL)
                {
                    nRet = GetThreshold(10000, outStatus, prm);
                }
                if (nRet == HVC_NORMAL)
                {
                    nRet = GetSizeRange(10000, outStatus, prm);
                }
                if (nRet == HVC_NORMAL)
                {
                    nRet = GetFaceDetectionAngle(10000, outStatus, prm);
                }
                if (Callback != null)
                {
                    Callback.OnPostGetParam(nRet, outStatus[0]);
                }

                if (Status == STATE_BUSY)
                {
                    Status = STATE_CONNECTED;
                }
            });

            Debug.WriteLine(TAG, "getParam() : HVC_NORMAL");
            return HVC_NORMAL;
        }

        //ORIGINAL LINE: @Override public int getVersion(final HVC_VER ver)
        public async override Task<int> GetVersion(HVC_VER ver)
        {
            if (BtDevice == null)
            {
                Debug.WriteLine(TAG, "getParam() : HVC_ERROR_NODEVICES");
                return HVC_ERROR_NODEVICES;
            }
            if (Service == null || Service.GetmConnectionState() != BleDeviceService.STATE_CONNECTED)
            {
                Debug.WriteLine(TAG, "getParam() : HVC_ERROR_DISCONNECTED");
                return HVC_ERROR_DISCONNECTED;
            }
            if (Status > STATE_CONNECTED)
            {
                Debug.WriteLine(TAG, "getParam() : HVC_ERROR_BUSY");
                return HVC_ERROR_BUSY;
            }

            Status = STATE_BUSY;

            await Task.Run(() =>
            {
                int nRet = HVC_NORMAL;
                byte[] outStatus = new byte[1];
                if (nRet == HVC_NORMAL)
                {
                    nRet = GetVersion(10000, outStatus, ver);
                }
                if (Callback != null)
                {
                    Callback.OnPostGetVersion(nRet, outStatus[0]);
                }

                if (Status == STATE_BUSY)
                {
                    Status = STATE_CONNECTED;
                }
            });

            Debug.WriteLine(TAG, "getVersion() : HVC_NORMAL");
            return HVC_NORMAL;
        }

        private BleCallback GattCallback;

        private class BleCallbackAnonymousInnerClassHelper : BleCallback
        {
            private readonly HVC_BLE outerInstance;

            public BleCallbackAnonymousInnerClassHelper(HVC_BLE outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public override void CallbackMethod(string action)
            {
                //*********************//
                if (action.Equals(BleDeviceService.ACTION_GATT_CONNECTED))
                {
                    Debug.WriteLine(TAG, "UART_CONNECT_MSG");
                    outerInstance.Status = STATE_CONNECTING;
                }

                //*********************//
                if (action.Equals(BleDeviceService.ACTION_GATT_DISCONNECTED))
                {
                    Debug.WriteLine(TAG, "UART_DISCONNECT_MSG");
                    outerInstance.Service.Close();
                    outerInstance.Status = STATE_DISCONNECTED;
                    if (outerInstance.Callback != null)
                    {
                        outerInstance.Callback.OnDisconnected();
                    }
                }

                //*********************//
                if (action.Equals(BleDeviceService.ACTION_GATT_SERVICES_DISCOVERED))
                {
                    Debug.WriteLine(TAG, "UART_DISCOVERED_MSG");
                    outerInstance.Status = STATE_CONNECTED;
                    if (outerInstance.Callback != null)
                    {
                        outerInstance.Callback.OnConnected();
                    }
                }

                //*********************//
                if (action.Equals(BleDeviceService.DEVICE_DOES_NOT_SUPPORT_UART))
                {
                    Debug.WriteLine(TAG, "DEVICE_DOES_NOT_SUPPORT_UART");
                    outerInstance.Service.Disconnect();
                    outerInstance.Status = STATE_DISCONNECTED;
                }
            }

            public override void CallbackMethod(string action, byte[] byText)
            {
                //*********************//
                if (action.Equals(BleDeviceService.ACTION_DATA_AVAILABLE + BleDeviceService.EXTRA_DATA))
                {
                    if (byText != null)
                    {
                        string deviceInfo = "DATA_AVAILABLE: " + Convert.ToString(byText.Length) + " byte";
                        lock (outerInstance.SyncObject)
                        {
                            for (int i = 0; i < byText.Length; i++)
                            {
                                outerInstance.TxValue.Add(byText[i]);
                                //deviceInfo += String.valueOf(byText[i]) + " ";
                            }
                        }
                        Debug.WriteLine(TAG, deviceInfo);
                        //System.Threading.Thread.Sleep(1);
                        outerInstance.Sleep(1);
                    }
                }

                //*********************//
                if (action.Equals(BleDeviceService.ACTION_DATA_AVAILABLE + BleDeviceService.NAME_DATA))
                {
                    if (byText != null && outerInstance.TxName != null)
                    {
                        string deviceInfo = "NAME_AVAILABLE: " + Convert.ToString(byText.Length) + " byte";
                        lock (outerInstance.SyncObject)
                        {
                            for (int i = 0; i < byText.Length; i++)
                            {
                                outerInstance.TxName[i] = byText[i];
                                //deviceInfo += String.valueOf(mtxName[i]) + " ";
                            }
                        }
                        Debug.WriteLine(TAG, deviceInfo);
                        if (outerInstance.Callback != null)
                        {
                            outerInstance.Callback.OnPostGetDeviceName(outerInstance.TxName);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Bt send signal<br>
        /// [Description]<br>
        /// none<br>
        /// [Notes]<br> </summary>
        /// <param name="inDataSize"> send signal data size<br> </param>
        /// <param name="inData"> send signal data<br> </param>
        /// <returns> int send signal complete data number<br> </returns>
        protected internal override int Send(byte[] inData)
        {
            do
            {
                if (Status < STATE_CONNECTED)
                {
                    return 0;
                }
                lock (SyncObject)
                {
                    int readLength = TxValue.Count;
                    if (readLength <= 0)
                    {
                        break;
                    }
                    TxValue.Clear();
                }
            } while (true);
            Service.WriteTXCharacteristic(inData);

            string deviceInfo = "Send: " + inData.Length + " byte";
            Debug.WriteLine(TAG, deviceInfo);
            return inData.Length;
        }

        /// <summary>
        /// Bt receive signalg<br>
        /// [Description]<br>
        /// none<br>
        /// [Notes]<br> </summary>
        /// <param name="inDataSize"> receive signal data size<br> </param>
        /// <param name="outResult"> receive signal data<br> </param>
        /// <returns> int receive signal complete data number<br> </returns>
        protected internal override int Receive(int inTimeOutTime, int inDataSize, byte[] outResult)
        {

            long maxTimeMillis = DateTime.Now.Ticks + inTimeOutTime * 1000;
            int readLength = 0;
            while (DateTime.Now.Ticks < maxTimeMillis)
            {
                if (Status < STATE_CONNECTED)
                {
                    return 0;
                }
                readLength = TxValue.Count;
                if (readLength >= inDataSize)
                {
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(1);
                    //sleep(1);
                }
            }
            readLength = 0;
            lock (SyncObject)
            {
                readLength = Math.Min(TxValue.Count, inDataSize);
                for (int i = 0; i < readLength; i++)
                {
                    outResult[i] = TxValue[i].Value;
                }
                for (int i = 0; i < readLength; i++)
                {
                    TxValue.RemoveAt(0);
                }
            }
            string deviceInfo = "Receive: " + Convert.ToString(readLength) + " byte";
            Debug.WriteLine(TAG, deviceInfo);
            return readLength;
        }

        public virtual void Sleep(long msec)
        {
            lock (SyncObject)
            {
                try
                {
                    Monitor.Wait(this, TimeSpan.FromMilliseconds(msec));
                    //System.Threading.Thread.Sleep(1);
                }
                catch
                {
                }
            }
        }

        public virtual void SetCallBack(HVCBleCallback hvcCallback)
        {
            // TODO Auto-generated method stub
            Callback = hvcCallback;
            Debug.WriteLine(TAG, "Set CallBack");
        }

        /// <summary>
        /// HVC_BLE connect<br>
        /// [Description]<br>
        /// Connect with HVC_BLE device<br>
        /// </summary>
        public virtual void Connect(Context context, BluetoothDevice device)
        {
            // TODO Auto-generated method stub
            Status = STATE_DISCONNECTED;
            if (Service != null)
            {
                Debug.WriteLine(TAG, "DisConnect Device = " + BtDevice.Name + " (" + BtDevice.Address + ")");
                Service.Close();
            }

            BtDevice = device;
            if (BtDevice == null)
            {
                return;
            }

            Service = new BleDeviceService(GattCallback);

            Service.Connect(context, BtDevice);
            Debug.WriteLine(TAG, "Connect Device = " + BtDevice.Name + " (" + BtDevice.Address + ")");
        }

        /// <summary>
        /// HVC_BLE disconnect<br>
        /// [Description]<br>
        /// Disconnect HVC_BLE device<br>
        /// </summary>
        public virtual void Disconnect()
        {
            // TODO Auto-generated method stub
            Status = STATE_DISCONNECTED;
            if (Service != null)
            {
                Debug.WriteLine(TAG, "DisConnect Device = " + BtDevice.Name + " (" + BtDevice.Address + ")");
                Service.Close();
            }
            Service = null;
            if (Callback != null)
            {
                Callback.OnDisconnected();
            }
        }

        public virtual int SetDeviceName(byte[] value)
        {
            // TODO Auto-generated method stub
            if (BtDevice == null)
            {
                Debug.WriteLine(TAG, "setDeviceName() : HVC_ERROR_NODEVICES");
                return HVC_ERROR_NODEVICES;
            }
            if (Service == null || Service.GetmConnectionState() != BleDeviceService.STATE_CONNECTED)
            {
                Debug.WriteLine(TAG, "setDeviceName() : HVC_ERROR_DISCONNECTED");
                return HVC_ERROR_DISCONNECTED;
            }
            if (Status > STATE_CONNECTED)
            {
                Debug.WriteLine(TAG, "setDeviceName() : HVC_ERROR_BUSY");
                return HVC_ERROR_BUSY;
            }

            Service.WriteNameCharacteristic(value);
            Debug.WriteLine(TAG, "getDeviceName() : HVC_NORMAL");
            return HVC_NORMAL;
        }

        public virtual int GetDeviceName(byte[] value)
        {
            // TODO Auto-generated method stub
            if (BtDevice == null)
            {
                Debug.WriteLine(TAG, "getDeviceName() : HVC_ERROR_NODEVICES");
                return HVC_ERROR_NODEVICES;
            }
            if (Service == null || Service.GetmConnectionState() != BleDeviceService.STATE_CONNECTED)
            {
                Debug.WriteLine(TAG, "getDeviceName() : HVC_ERROR_DISCONNECTED");
                return HVC_ERROR_DISCONNECTED;
            }
            if (Status > STATE_CONNECTED)
            {
                Debug.WriteLine(TAG, "getDeviceName() : HVC_ERROR_BUSY");
                return HVC_ERROR_BUSY;
            }

            TxName = value;
            Service.ReadNameCharacteristic();
            Debug.WriteLine(TAG, "getDeviceName() : HVC_NORMAL");
            return HVC_NORMAL;
        }
    }

}