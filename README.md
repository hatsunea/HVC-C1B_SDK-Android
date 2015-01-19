# HVC-C1B Android-SDK

### 1. Code contents  
This code provides the JAVA API classes from Bluetooth connection to function execution and disconnection process.

### 2. Directory structure  
      AndroidManifest.xml		Manifest  
      bin/  
        hvc_c1w_sdk.jar			JAR file created from code
      src/  
        omron/  
          HVC/				   HVC class package
            HVC.java			   HVC parent class  
            HVC_BLE.java		   HVC-C class (HVC sub-class)
            HVC_VER.java		   Class storing the HVC version number
            HVC_PRM.java		   Class storing the set values for each parameter
            HVC_RES.java		   Class storing the function execution results
            HVCCallback.java	   Parent class for callback function  
            HVCBleCallback.java	Callback class to return the device status from HVC_BLE to main activity(HVCCallback sub-class)  
            BleCallback.java	   Callback class to return the device status from BleDeviceService to HVC_BLE
            BleDeviceSearch.java   Class for Bluetooth device search  
            BleDeviceService.java  Class for Bluetooth device control  

### 3. Method for building code
(1) Make sure that the code is compiled with JAVA version 1.7.  
Use V4.3 or higher for Android-SDK (required).

(2) Bluetooth permission  
Since HVC-C is connected with Bluetooth, the application will require Bluetooth permission. Add the following to the application manifest:

    <uses-permission android:name="android.permission.BLUETOOTH" />
    <uses-permission android:name="android.permission.BLUETOOTH_ADMIN" />

### 4. Links to the application
(1) libs folder  
Create a libs folder in the application project and copy hvc_c1w_sdk.jar into it.

(2) Import  
The HVC classes will be useable if they are imported as follows in the application source:

     import omron.HVC.HVC;  
     import omron.HVC.HVCBleCallback;  
     import omron.HVC.HVC_BLE;  
     import omron.HVC.HVC_PRM;  
     import omron.HVC.HVC_RES;  
     import omron.HVC.HVC_RES.FaceResult;  

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
                        ・The method for HVC class is executed out-of-synch. The callback function is called after process completion

    Refer to the class diagram in [HVC-C_Android class diagram](./HVC-C_Android_Class.png) for details.


* Usage of HVC class  

        <Extract main process flow>
        // Create class
        hvcBle = new HVC_BLE();
        hvcPrm = new HVC_PRM();
        hvcRes = new HVC_RES();

        // Obtain BLE device
        BluetoothDevice device = SelectHVCDevice("OMRON_HVC.*|omron_hvc.*");  

        // Register callback
        hvcBle.setCallBack(hvcCallback);

        // Connect HVC and BLE
        hvcBle.connect(getApplicationContext(), device);

        // Set parameters
        hvcPrm.face.MinSize = 60;
        hvcBle.setParam(hvcPrm);

        // Execute detection
        hvcBle.execute(HVC.HVC_ACTIV_FACE_DETECTION |
                       HVC.HVC_ACTIV_FACE_DIRECTION, hvcRes);

        // Disconnect BLE
        hvcBle.disconnect();

        <Get detection results>
        HVCBleCallback hvcCallback
        {
            @Override
            public void onConnected() {
                // Connection completed
            }

            @Override
            public void onDisconnected() {
                // Disconnected
            }

            @Override
            public void onPostSetParam(int nRet, byte outStatus) {
                // Settings completed
            }

            @Override
            public void onPostGetParam(int nRet, byte outStatus) {
                // Get settings
            }

            @Override
            public void onPostExecute(int nRet, byte outStatus) {
                // Get detection result
                for (FaceResult faceResult : hvcRes.face) {
                    int size = faceResult.size;
                    int posX = faceResult.posX;
                    int posY = faceResult.posY;
                    int conf = faceResult.confidence;
                }
            }
        }

* CAUTION  
After creating the HVC_BLE classes, the connection process to the device set in the parameters will start with the connect() method.
Some time will be required for the connection to be completed.
The functions must be executed after confirming the connection completion with the callback method onConnected().
(An error will be returned if the functions are executed during the connection process)


###[NOTES ON USAGE]
* This sample code and documentation are copyrighted property of OMRON Corporation  
* This sample code does not guarantee proper operation

----
OMRON Corporation
Copyright(C) 2014-2015 OMRON Corporation, All Rights Reserved.
