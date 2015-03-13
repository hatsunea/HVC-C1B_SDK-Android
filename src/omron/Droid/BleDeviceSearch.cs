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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace omron.HVC
{

    /// <summary>
    /// Bluetooth Device Search
    /// </summary>
    public class BleDeviceSearch
    {
        private List<BluetoothDevice> DeviceList = null;

        private BluetoothAdapter Adapter = null;
        private BluetoothReceiver Receiver = null;

        public BleDeviceSearch(Context context) 
        {
            DeviceList = new List<BluetoothDevice>();

            // Step 1: Enable Bluetooth
            Adapter = BluetoothAdapter.DefaultAdapter;
            if (Adapter != null)
            {
                // Bluetooth supported
                if (Adapter.IsEnabled)
                {
                    DiscoverDevices(context);
                }
            }
        }

        private void DiscoverDevices(Context context)
        {
            // Step 4: Scan for Bluetooth device
            this.Receiver = new BluetoothReceiver(this);
            context.RegisterReceiver(this.Receiver, new IntentFilter(BluetoothDevice.ActionFound));
            this.Adapter.StartDiscovery();
        }

        internal class BluetoothReceiver : BroadcastReceiver
        {
            private readonly BleDeviceSearch Parent;

            public BluetoothReceiver(BleDeviceSearch parent)
            {
                this.Parent = parent;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                string action = intent.Action;
                if (BluetoothDevice.ActionFound.Equals(action))
                {
                    BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                    if (Parent.DeviceList.Where((p) => p.Address == device.Address).Count() == 0)
                    {
                        Parent.DeviceList.Add(device);
                    }
                }
            }
        }

        public void StopDeviceSearch(Context context)
        {
            if (this.Adapter != null &&
                    this.Receiver != null)
            {
                this.Adapter.CancelDiscovery();
                context.UnregisterReceiver(this.Receiver);
            }
        }

        public virtual List<BluetoothDevice> Devices
        {
            get
            {
                // TODO Auto-generated method stub
                return DeviceList;
            }
        }
    }

}