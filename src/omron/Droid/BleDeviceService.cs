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
using Java.Util;
using System;
using System.Diagnostics;

namespace omron.HVC
{
    /// <summary>
    /// Service for managing connection and data communication with a GATT server hosted on a
    /// given Bluetooth LE device.
    /// </summary>
    public class BleDeviceService
    {
        public const int STATE_DISCONNECTED = 0;
        public const int STATE_CONNECTING = 1;
        public const int STATE_CONNECTED = 2;

        public const string ACTION_GATT_CONNECTED = "ACTION_GATT_CONNECTED";
        public const string ACTION_GATT_DISCONNECTED = "ACTION_GATT_DISCONNECTED";
        public const string ACTION_GATT_SERVICES_DISCOVERED = "ACTION_GATT_SERVICES_DISCOVERED";
        public const string ACTION_DATA_AVAILABLE = "ACTION_DATA_AVAILABLE";
        public const string NAME_DATA = ":NAME_DATA";
        public const string EXTRA_DATA = ":EXTRA_DATA";
        public const string DEVICE_DOES_NOT_SUPPORT_UART = "DEVICE_DOES_NOT_SUPPORT_UART";

        public static readonly UUID CCCD = UUID.FromString("00002902-0000-1000-8000-00805f9b34fb");

        public static readonly UUID RX_SERVICE_UUID2 = UUID.FromString("35100001-d13a-4f39-8ab3-bf64d4fbb4b4");
        public static readonly UUID RX_CHAR_UUID2 = UUID.FromString("35100002-d13a-4f39-8ab3-bf64d4fbb4b4");
        public static readonly UUID TX_CHAR_UUID2 = UUID.FromString("35100003-d13a-4f39-8ab3-bf64d4fbb4b4");

        public static readonly UUID NAME_CHAR_UUID = UUID.FromString("35100004-d13a-4f39-8ab3-bf64d4fbb4b4");

        private BleCallback CallBack = null;

        private BluetoothGatt Gatt;
        private string DeviceAddress;
        private int ConnectionState = STATE_DISCONNECTED;

        private const string TAG = "BleDeviceService";

        // Implements callback methods for GATT events that the app cares about.  For example,
        // connection change and services discovered.
        private BluetoothGattCallback mGattCallback;

        public BleDeviceService(BleCallback gattCallback)
        {
            CallBack = gattCallback;
            mGattCallback = new GattCallback(this);
        }

        private class GattCallback : BluetoothGattCallback
        {
            private readonly BleDeviceService Service;

            public GattCallback(BleDeviceService parent)
            {
                this.Service = parent;
            }

            public override void OnConnectionStateChange(BluetoothGatt gatt, GattStatus status, ProfileState newState)
            {
                string intentAction;

                if (newState == ProfileState.Connected)
                {
                    intentAction = ACTION_GATT_CONNECTED;
                    Service.SetmConnectionState(STATE_CONNECTED);
                    Service.BroadcastUpdate(intentAction);
                    System.Console.WriteLine(TAG, "Connected to GATT server.");
                    // Attempts to discover services after successful connection.
                    System.Console.WriteLine(TAG, "Attempting to start service discovery:" + Service.Gatt.DiscoverServices());
                }
                else if (newState == ProfileState.Disconnected)
                {
                    intentAction = ACTION_GATT_DISCONNECTED;
                    Service.SetmConnectionState(STATE_DISCONNECTED);
                    System.Console.WriteLine(TAG, "Disconnected from GATT server.");
                    Service.BroadcastUpdate(intentAction);
                }
            }

            public override void OnServicesDiscovered(BluetoothGatt gatt, GattStatus status)
            {
                if (status == GattStatus.Success)
                {
                    Debug.WriteLine(TAG, "mBluetoothGatt = " + Service.Gatt);
                    BluetoothGattCharacteristic TxChar;
                    BluetoothGattService RxService = Service.Gatt.GetService(RX_SERVICE_UUID2);
                    if (RxService == null)
                    {
                        Service.ShowMessage("mBluetoothGatt null" + Service.Gatt);
                        Service.ShowMessage("Rx service not found!");
                        Service.BroadcastUpdate(DEVICE_DOES_NOT_SUPPORT_UART);
                        return;
                    }
                    TxChar = RxService.GetCharacteristic(TX_CHAR_UUID2);
                    if (TxChar == null)
                    {
                        Service.ShowMessage("Rx charateristic not found!");
                        Service.BroadcastUpdate(DEVICE_DOES_NOT_SUPPORT_UART);
                        return;
                    }
                    Debug.WriteLine(TAG, "RxChar = " + TX_CHAR_UUID2.ToString());
                    Service.Gatt.SetCharacteristicNotification(TxChar, true);

                    BluetoothGattDescriptor descriptor = TxChar.GetDescriptor(CCCD);
                    var ilistValue = BluetoothGattDescriptor.EnableNotificationValue;
                    var arrayValue = new byte[ilistValue.Count];
                    for (int index = 0; index < arrayValue.Length; index++)
                    {
                        arrayValue[index] = ilistValue[index];
                    }
                    descriptor.SetValue(arrayValue);
                    Service.Gatt.WriteDescriptor(descriptor);

                    Service.BroadcastUpdate(ACTION_GATT_SERVICES_DISCOVERED);
                }
                else
                {
                    Debug.WriteLine(TAG, "onServicesDiscovered received: " + status);
                }
            }

            public override void OnCharacteristicRead(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, GattStatus status)
            {
                Debug.WriteLine(TAG, string.Format("onCharacteristicRead: {0}", characteristic.Uuid));
                if (status == GattStatus.Success)
                {
                    Service.BroadcastUpdate(ACTION_DATA_AVAILABLE, characteristic);
                }
            }

            public override void OnCharacteristicChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic)
            {
                Debug.WriteLine(TAG, string.Format("onCharacteristicChanged: {0}", characteristic.Uuid));
                Service.BroadcastUpdate(ACTION_DATA_AVAILABLE, characteristic);
            }
        }

        private void BroadcastUpdate(string action)
        {
            CallBack.CallbackMethod(action);
        }

        private void BroadcastUpdate(string action, BluetoothGattCharacteristic characteristic)
        {
            // This is special handling for the Heart Rate Measurement profile.  Data parsing is
            // carried out as per profile specifications:
            // http://developer.bluetooth.org/gatt/characteristics/Pages/CharacteristicViewer.aspx?u=org.bluetooth.characteristic.heart_rate_measurement.xml
            if (TX_CHAR_UUID2.Equals(characteristic.Uuid))
            {
                Debug.WriteLine(TAG, string.Format("Received Text: {0:D}", characteristic.GetValue().Length));
                CallBack.CallbackMethod(action + EXTRA_DATA, characteristic.GetValue());
            }
            else
            {
                if (NAME_CHAR_UUID.Equals(characteristic.Uuid))
                {
                    CallBack.CallbackMethod(action + NAME_DATA, characteristic.GetValue());
                }
            }
        }

        private bool RefreshDeviceCache(BluetoothGatt gatt)
        {
            try
            {
                BluetoothGatt localBluetoothGatt = gatt;
                var localMethod = localBluetoothGatt.GetType().GetMethod("refresh", new Type[0]);
                if (localMethod != null)
                {
                    bool @bool = (bool)((bool?)localMethod.Invoke(localBluetoothGatt, new object[0]));
                    Debug.WriteLine(TAG, string.Format("refreshDeviceCache : {0:D}", @bool));
                    return @bool;
                }
            }
            catch
            {
                System.Console.WriteLine(TAG, "An exception occured while refreshing device");
            }
            Debug.WriteLine(TAG, string.Format("refreshDeviceCache : false"));
            return false;
        }

        /// <summary>
        /// Connects to the GATT server hosted on the Bluetooth LE device.
        /// </summary>
        /// <param name="address"> The device address of the destination device.
        /// </param>
        /// <returns> Return true if the connection is initiated successfully. The connection result
        ///         is reported asynchronously through the
        ///         {@code BluetoothGattCallback#onConnectionStateChange(android.bluetooth.BluetoothGatt, int, int)}
        ///         callback. </returns>
        public virtual bool Connect(Context context, BluetoothDevice device)
        {
            if (device == null)
            {
                Debug.WriteLine(TAG, "Device not found. Unable to connect.");
                return false;
            }

            // Previously connected device.  Try to reconnect.
            if (DeviceAddress != null && device.Address.Equals(DeviceAddress) && Gatt != null)
            {
                Debug.WriteLine(TAG, "Trying to use an existing mBluetoothGatt for connection.");
                if (Gatt.Connect())
                {
                    SetmConnectionState(STATE_CONNECTING);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // We want to directly connect to the device, so we are setting the autoConnect
            // parameter to false.
            Gatt = device.ConnectGatt(context, false, mGattCallback);
            RefreshDeviceCache(Gatt);
            Debug.WriteLine(TAG, "Trying to create a new connection.");
            DeviceAddress = device.Address;
            SetmConnectionState(STATE_CONNECTING);
            return true;
        }

        /// <summary>
        /// Disconnects an existing connection or cancel a pending connection. The disconnection result
        /// is reported asynchronously through the
        /// {@code BluetoothGattCallback#onConnectionStateChange(android.bluetooth.BluetoothGatt, int, int)}
        /// callback.
        /// </summary>
        public virtual void Disconnect()
        {
            if (Gatt == null)
            {
                Debug.WriteLine(TAG, "mBluetoothGatt not initialized");
                return;
            }
            Gatt.Disconnect();
        }

        /// <summary>
        /// After using a given BLE device, the app must call this method to ensure resources are
        /// released properly.
        /// </summary>
        public virtual void Close()
        {
            if (Gatt == null)
            {
                return;
            }
            Debug.WriteLine(TAG, "mBluetoothGatt closed");
            DeviceAddress = null;
            Gatt.Close();
            Gatt = null;
        }

        /// <summary>
        /// Request a read on a given {@code BluetoothGattCharacteristic}. The read result is reported
        /// asynchronously through the {@code BluetoothGattCallback#onCharacteristicRead(android.bluetooth.BluetoothGatt, android.bluetooth.BluetoothGattCharacteristic, int)}
        /// callback.
        /// </summary>
        /// <param name="characteristic"> The characteristic to read from. </param>
        public virtual void ReadCharacteristic(BluetoothGattCharacteristic characteristic)
        {
            if (Gatt == null)
            {
                Debug.WriteLine(TAG, "mBluetoothGatt not initialized");
                return;
            }
            Gatt.ReadCharacteristic(characteristic);
        }

        public virtual void WriteTXCharacteristic(byte[] value)
        {
            BluetoothGattCharacteristic rxChar;
            var rxService = Gatt.GetService(RX_SERVICE_UUID2);
            if (rxService == null)
            {
                ShowMessage("mBluetoothGatt null" + Gatt);
                ShowMessage("Tx service not found!");
                BroadcastUpdate(DEVICE_DOES_NOT_SUPPORT_UART);
                return;
            }
            rxChar = rxService.GetCharacteristic(RX_CHAR_UUID2);
            if (rxChar == null)
            {
                ShowMessage("Tx charateristic not found!");
                BroadcastUpdate(DEVICE_DOES_NOT_SUPPORT_UART);
                return;
            }
            Debug.WriteLine(TAG, "TxChar = " + RX_CHAR_UUID2.ToString());
            rxChar.SetValue(value);
            bool status = Gatt.WriteCharacteristic(rxChar);
            Debug.WriteLine(TAG, "write TXchar - status=" + status);
        }

        public virtual void ReadNameCharacteristic()
        {
            var rxService = Gatt.GetService(RX_SERVICE_UUID2);
            if (rxService == null)
            {
                ShowMessage("Rx service not found!");
                BroadcastUpdate(DEVICE_DOES_NOT_SUPPORT_UART);
                return;
            }
            var nameChar = rxService.GetCharacteristic(NAME_CHAR_UUID);
            if (nameChar == null)
            {
                ShowMessage("Name charateristic not found!");
                BroadcastUpdate(DEVICE_DOES_NOT_SUPPORT_UART);
                return;
            }
            var status = Gatt.ReadCharacteristic(nameChar);
            Debug.WriteLine(TAG, "read NAMEchar - status=" + status);
        }

        public virtual void WriteNameCharacteristic(byte[] value)
        {
            var rxService = Gatt.GetService(RX_SERVICE_UUID2);
            if (rxService == null)
            {
                ShowMessage("Rx service not found!");
                BroadcastUpdate(DEVICE_DOES_NOT_SUPPORT_UART);
                return;
            }
            var nameChar = rxService.GetCharacteristic(NAME_CHAR_UUID);
            if (nameChar == null)
            {
                ShowMessage("Rx charateristic not found!");
                BroadcastUpdate(DEVICE_DOES_NOT_SUPPORT_UART);
                return;
            }
            nameChar.SetValue(value);
            var status = Gatt.WriteCharacteristic(nameChar);
            Debug.WriteLine(TAG, "write NAMEchar - status=" + status);
        }

        private void ShowMessage(string msg)
        {
            System.Console.WriteLine(TAG, msg);
        }

        public int GetmConnectionState()
        {
            return ConnectionState;
        }

        public void SetmConnectionState(int State)
        {
            this.ConnectionState = State;
        }
    }

}