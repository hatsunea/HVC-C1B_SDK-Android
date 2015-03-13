# HVC-C1B Xamarin.Android-SDK by HATSUNE, Akira. Original SDK by OMRON

### 1. Code contents  
This code provides the .NET Framework API classes from Bluetooth connection to function execution and disconnection process.

### 2. Directory structure  
      bin/  
        omron.HVC.Droid.dll			dll file created from code
      src/  
        omron/  
          HVC/				    HVC class package
            Core/                     Common Core Class
              HVC.java			   HVC parent class  
              HVC_VER.java		   Class storing the HVC version number
              HVC_PRM.java		   Class storing the set values for each parameter
              HVC_RES.java		   Class storing the function execution results
              HVCCallback.java	   Parent class for callback function  
              HVCBleCallback.java	   Callback class to return the device status from HVC_BLE to main activity(HVCCallback sub-class)  
              BleCallback.java	   Callback class to return the device status from BleDeviceService to HVC_BLE
            Droid/                    Android Class
              BleDeviceSearch.java   Class for Bluetooth device search  
              BleDeviceService.java  Class for Bluetooth device control  
              HVC_BLE.java		   HVC-C class (HVC sub-class)

### 3. Method for building code
(1) Use V4.3 or higher for Android-SDK (required).

(2) Bluetooth permission  
Since HVC-C is connected with Bluetooth, the application will require Bluetooth permission. Add the following to the application manifest:

    <uses-permission android:name="android.permission.BLUETOOTH" />
    <uses-permission android:name="android.permission.BLUETOOTH_ADMIN" />

### 4. Links to the application
(1) Reference
Reference omron.HVC.Droid.dll into it.

(2) using  
The HVC classes will be useable if they are imported as follows in the application source:

     using omron.HVC;

 (3) Guidance for programming

* Description of main classes  

        Name		      Description
        HVC               HVC super class
        HVC_BLE           Class for HVC-C
                          ・Used for HVC-C operations
        HVC_PRM           HVC parameters
                          ・Passed as argument of HVC.setParam()
        HVC_RES           Class storing detection results
        HVC_BleCallback   Class for callbacks called after process completion
                          ・The method for HVC class is executed out-of-sync.
                            The callback function is called after process completion.

    Refer to the class diagram in [HVC-C_Android class diagram](./HVC-C_Android_Class.png) for details.


* Usage of HVC class  

        <Extract main process flow>
        // Create class
            HvcBle = new HVC_BLE();
            HvcPrm = new HVC_PRM();
            HvcRes = new HVC_RES();

        // Obtain BLE device
        BluetoothDevice device = await this.SelectHVCDevice("OMRON_HVC.*|omron_hvc.*");

        // Register callback
        HvcBle.SetCallBack(new hvcCallback(this));

        // Connect HVC and BLE
        this.HvcBle.Connect(global::Android.App.Application.Context, device);

        // Set parameters
        this.HvcPrm.Face.MinSize = 60;
        await this.HvcBle.SetParam(this.HvcPrm);

        // Execute detection
        await this.HvcBle.execute(HVC.HVC_ACTIV_FACE_DETECTION |
                                  HVC.HVC_ACTIV_FACE_DIRECTION, this.HvcRes);

        // Disconnect BLE
        this.HvcBle.Disconnect();

        <Get detection results>
        private class hvcCallback : HVCBleCallback
        { 
            private readonly MainActivity outerInstance;

            public hvcCallback(MainActivity outerInstance)
            {
                this.outerInstance = outerInstance;
            }
            public override void OnConnected()
            {
                // Connection completed
            }

            public override void OnDisconnected()
            {
                // Disconnected
            }

            public override void OnPostSetParam(int nRet, byte outStatus)
            {
                // Settings completed
            }

            public override void OnPostGetParam(int nRet, byte outStatus)
            {
                // Get settings
            }

            public override void OnPostExecute(int nRet, byte outStatus)
            {
                // Get detection result
                foreach (HVC_RES.DetectionResult bodyResult in outerInstance.HvcRes.Body)
                {
                    int size = bodyResult.Size;
                    int posX = bodyResult.PosX;
                    int posY = bodyResult.PosY;
                    int conf = bodyResult.Confidence;
                }
            }
        }

* CAUTION  
After creating the HVC_BLE classes, the connection process to the device set in the parameters will start with the connect() method.
Some time will be required for the connection to be completed.
The functions must be executed after confirming the connection completion with the callback method onConnected().
(An error will be returned if the functions are executed during the connection process)


###[NOTES ON USAGE]
* This sample code and documentation are copyrighted property of HATSUNE, Akira
* Under Apache License 2.0.
* This sample code does not guarantee proper operation
* Original sample code and documentation are copyrighted property of OMRON Corporation  

----
HATSUNE, Akira
Copyright(C) 2014-2015 HATSUNE, Akira, All Rights Reserved.
