# HVC-C1B Xamarin.Android-SDK by HATSUNE,Akira Original SDK by OMRON

### 1. コード内容  
 本コードではBluetooth接続から機能実行、切断処理までを.NET FrameworkのAPIクラスとして用意しています。

### 2. ディレクトリ構成  
      bin/  
        omron.HVC.Droid.dll             コードから生成したdllファイル  
      src/  
        omron/  
          HVC/                      HVCクラスパッケージ  
            Core/                     共通コアクラス
              HVC.cs                  HVCの親クラス  
              HVC_VER.cs              HVCのバージョン番号を格納するクラス  
              HVC_PRM.cs              各種パラメータの設定値を格納するクラス 
              HVC_RES.cs              機能実行結果を格納するクラス  
              HVCCallback.cs          コールバック関数の親クラス  
              HVCBleCallback.cs       HVC_BLEからメインアクティビティにデバイスの状態を返すための
                                        コールバッククラス（HVCCallbackのサブクラス）  
              BleCallback.cs          BleDeviceServiceからHVC_BLEにデバイスの状態を返すための
                                        コールバッククラス  
            Droid/                    Android固有クラス
              BleDeviceSearch.cs      Bluetoothデバイスを検索するクラス  
              BleDeviceService.cs     Bluetoothデバイス管理クラス  
              HVC_BLE.cs              HVC-Cクラス(HVCのサブクラス)  

### 3. コードのビルド方法
 (1) Android-SDKは4.3以上を使用する必要があります。

 (2) Bluetoothパーミッション
     HVC-CはBluetooth接続であるため、アプリケーションにはBluetoothパーミッションの
     許可が必要です。アプリケーションマニュフェストに以下の記述を追加してください。

    <uses-permission android:name="android.permission.BLUETOOTH" />
    <uses-permission android:name="android.permission.BLUETOOTH_ADMIN" />

### 4. アプリへのリンクについて
 (1) 参照設定
     作成したアプリケーションプロジェクトでomron.HVC.Droid.dllを参照設定してください。
または、nugetを使ってアプリケーションプロジェクトへomron.HVC.Droid.dllをインストールしてください。
PM> Install-Package omron.HVC.Droid
nugetによる方法をお勧めします。

 (2) using  
     アプリケーションソースに次のようにHVCクラスをusingすればHVCクラスの使用が可能になります。

     using omron.HVC;

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
            HvcBle = new HVC_BLE();
            HvcPrm = new HVC_PRM();
            HvcRes = new HVC_RES();

        // BLEデバイス取得
        BluetoothDevice device = await this.SelectHVCDevice("OMRON_HVC.*|omron_hvc.*");

        // コールバックを登録
        HvcBle.SetCallBack(new hvcCallback(this));

        // HVCとBLE接続
        this.HvcBle.Connect(global::Android.App.Application.Context, device);

        // パラメータ設定
        this.HvcPrm.Face.MinSize = 60;
        await this.HvcBle.SetParam(this.HvcPrm);
        // 検出実行
        await this.HvcBle.execute(HVC.HVC_ACTIV_FACE_DETECTION |
                                  HVC.HVC_ACTIV_FACE_DIRECTION, this.HvcRes);

        // BLE接続を切断
        this.HvcBle.Disconnect();

        <検出結果の取得>
        private class hvcCallback : HVCBleCallback
        { 
            private readonly MainActivity outerInstance;

            public hvcCallback(MainActivity outerInstance)
            {
                this.outerInstance = outerInstance;
            }
            public override void OnConnected()
            {
                // 接続完了
            }

            public override void OnDisconnected()
            {
                // 切断しました
            }

            public override void OnPostSetParam(int nRet, byte outStatus)
            {
                // 設定完了
            }

            public override void OnPostGetParam(int nRet, byte outStatus)
            {
                // 設定値取得
            }

            public override void OnPostExecute(int nRet, byte outStatus)
            {
                // 検出結果取得
                foreach (HVC_RES.DetectionResult bodyResult in outerInstance.HvcRes.Body)
                {
                    int size = bodyResult.Size;
                    int posX = bodyResult.PosX;
                    int posY = bodyResult.PosY;
                    int conf = bodyResult.Confidence;
                }
            }
        }

* 注意事項  
     HVC_BLEクラスはnewした後、connect()メソッドで引数に設定されたデバイスと
     接続処理に入ります。接続完了までにはある程度の時間がかかります。コールバックメソッド
     onConnected()を使用して接続完了を確認後に機能実行する必要があります。
     （接続中に機能実行を呼び出すとエラーが返ってきます。）


###[ご使用にあたって]
* 本サンプルコードおよびドキュメントの著作権は初音玲に帰属します。
* ライセンスはApache License 2.0となります。
* 本サンプルコードは動作を保証するものではありません。
* オリジナルサンプルコードおよびドキュメントの著作権はオムロンに帰属します。  

----
初音玲
Copyright(C) 2014-2015 HATSUNE, Akira, All Rights Reserved.

