# HVC-C1B Android-SDK by OMRON

### 1. コード内容  
 本コードではBluetooth接続から機能実行、切断処理までをJAVAのAPIクラスとして用意しています。

### 2. ディレクトリ構成  
      AndroidManifest.xml         マニュフェスト  
      bin/  
        hvc_c1b_sdk.jar             コードから生成したJARファイル  
      src/  
        omron/  
          HVC/                      HVCクラスパッケージ  
            HVC.java                  HVCの親クラス  
            HVC_BLE.java              HVC-Cクラス(HVCのサブクラス)  
            HVC_VER.java              HVCのバージョン番号を格納するクラス  
            HVC_PRM.java              各種パラメータの設定値を格納するクラス  
            HVC_RES.java              機能実行結果を格納するクラス  
            HVCCallback.java          コールバック関数の親クラス  
            HVCBleCallback.java       HVC_BLEからメインアクティビティにデバイスの状態を返すための
                                      コールバッククラス（HVCCallbackのサブクラス）  
            BleCallback.java          BleDeviceServiceからHVC_BLEにデバイスの状態を返すための
                                      コールバッククラス  
            BleDeviceSearch.java      Bluetoothデバイスを検索するクラス  
            BleDeviceService.java     Bluetoothデバイス管理クラス  

### 3. コードのビルド方法
 (1) コードはJAVAのバージョン1.7でコンパイルを確認しています。
     またAndroid-SDKは4.3以上を使用する必要があります。

 (2) Bluetoothパーミッション
     HVC-CはBluetooth接続であるため、アプリケーションにはBluetoothパーミッションの
     許可が必要です。アプリケーションマニュフェストに以下の記述を追加してください。

    <uses-permission android:name="android.permission.BLUETOOTH" />
    <uses-permission android:name="android.permission.BLUETOOTH_ADMIN" />

### 4. アプリへのリンクについて
 (1) libsフォルダ  
     作成したアプリケーションプロジェクトにlibsフォルダを作成し、hvc_c1w_sdk.jarをコピーしてください。

 (2) インポート  
     アプリケーションソースに次のようにHVCクラスをインポートすればHVCクラスの使用が可能になります。

     import omron.HVC.HVC;  
     import omron.HVC.HVCBleCallback;  
     import omron.HVC.HVC_BLE;  
     import omron.HVC.HVC_PRM;  
     import omron.HVC.HVC_RES;  
     import omron.HVC.HVC_RES.FaceResult;  

 (3) プログラミングの手引き  

* 主なクラスの概要  

        クラス名           説明
        HVC               HVCのsuperクラス
        HVC_BLE           HVC-Cに対応するクラス
                          ・HVC-Cの操作はこのクラスで行います。
        HVC_PRM           HVCの設定パラメータ
                          ・HVC.setParam()の引数として渡されます。
        HVC_RES           検出結果を格納するクラス
        HVC_BleCallback   処理完了後に呼び出されるコールバックをまとめたクラス
                          ・HVCクラスのメソッドは非同期で実行され、
                            処理完了後にコールバック関数が呼び出されます。

    詳細なクラス図は[HVC-C_Androidクラス図](./HVC-C_Android_Class.png)をご参照下さい。


* HVCクラスの使い方  

        <メイン処理フロー抜粋>
        // クラスの作成
        hvcBle = new HVC_BLE();
        hvcPrm = new HVC_PRM();
        hvcRes = new HVC_RES();

        // BLEデバイス取得
        BluetoothDevice device = SelectHVCDevice("OMRON_HVC.*|omron_hvc.*");  

        // コールバックを登録
        hvcBle.setCallBack(hvcCallback);

        // HVCとBLE接続
        hvcBle.connect(getApplicationContext(), device);

        // パラメータ設定
        hvcPrm.face.MinSize = 60;
        hvcBle.setParam(hvcPrm);
        // 検出実行
        hvcBle.execute(HVC.HVC_ACTIV_FACE_DETECTION |
                       HVC.HVC_ACTIV_FACE_DIRECTION, hvcRes);

        // BLE接続を切断
        hvcBle.disconnect();

        <検出結果の取得>
        HVCBleCallback hvcCallback
        {
            @Override
            public void onConnected() {
                // 接続完了
            }

            @Override
            public void onDisconnected() {
                // 切断しました
            }

            @Override
            public void onPostSetParam(int nRet, byte outStatus) {
                // 設定完了
            }

            @Override
            public void onPostGetParam(int nRet, byte outStatus) {
                // 設定値取得
            }

            @Override
            public void onPostExecute(int nRet, byte outStatus) {
                // 検出結果取得
                for (FaceResult faceResult : hvcRes.face) {
                    int size = faceResult.size;
                    int posX = faceResult.posX;
                    int posY = faceResult.posY;
                    int conf = faceResult.confidence;
                }
            }
        }

* 注意事項  
     HVC_BLEクラスはnewした後、connect()メソッドで引数に設定されたデバイスと
     接続処理に入ります。接続完了までにはある程度の時間がかかります。コールバックメソッド
     onConnected()を使用して接続完了を確認後に機能実行する必要があります。
     （接続中に機能実行を呼び出すとエラーが返ってきます。）


###[ご使用にあたって]
* 本サンプルコードおよびドキュメントの著作権はオムロンに帰属します。  
* 本サンプルコードは動作を保証するものではありません。

----
オムロン株式会社  
Copyright(C) 2014-2015 OMRON Corporation, All Rights Reserved.

