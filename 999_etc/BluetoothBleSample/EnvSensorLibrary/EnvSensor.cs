using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;

//#if WINDOWS_UWP
//namespace EnvSensorLibraryUWP
//#else
//namespace EnvSensorLibrary
//#endif
namespace EnvSensorLibrary
{
    /// <summary>
    /// Omronの環境センサEnvSensor BL01の制御クラス
    /// </summary>
    /// <remarks>
    /// BL01の通信仕様はOMRONのページを参照。
    /// https://omronfs.omron.com/ja_JP/ecb/products/pdf/CDSC-015.pdf
    /// </remarks>
    public class EnvSensor : IDisposable
    {
        private Guid SeosorServiceUuid { get; set; } = new Guid("3000".toEnvSensorUUID());
        private Guid LatestDataCharacteristicUuid { get; set; } = new Guid("3001".toEnvSensorUUID());

        private DeviceWatcher DeviceWatcher { get; set; }

        DeviceInformation DeviceInformation { get; set; } = null;
        /// <summary>
        /// Bluetoothデバイス
        /// </summary>
        private BluetoothLEDevice Device { get; set; }
        private GattDeviceServicesResult Services { get; set; }
        private GattDeviceService LatestDataService { get; set; }
        private GattCharacteristicsResult Characteristics { get; set; }
        private GattCharacteristic LatestDataCharacteristic { get; set; }

        /// <summary>
        /// 最新センサ情報変化時のハンドラ
        /// (本クラス使用者側に登録してもらう)
        /// 引数は順に、温度・湿度・照度・騒音
        /// </summary>
        public Action<double, double, double, double> LatestDataChanged { get; set; } = null;

        public void Dispose()
        {

        }

        public EnvSensor()
        {

        }

        /// <summary>
        /// Bluetooth v4機器の監視を開始し、機器リストを作成する
        /// </summary>
        public void StartCommunicationWithEnvSensorBL01()
        {
            StartWatcher();
        }

        private void StartWatcher()
        {
            // Bluetooth v4（BLE）を指定
            //string selector = "(" + "System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\"" + ")";// BLEプロトコルを持つ機器全部
            string selector = "(" + GattDeviceService.GetDeviceSelectorFromUuid(SeosorServiceUuid) + ")";// Omronのやつだけ。(これだとペアリングしてないとでてこなくなる？)

            // 第一引数：何の機器をwatchするか。指定したものだけAddedなどのハンドラで引っ掛ける。
            // 第二引数：ほしい情報を指定する(指定したものだけ、Addedなどのハンドラの引数に乗ってくる？)
            //DeviceWatcher = DeviceInformation.CreateWatcher(selector, null, DeviceInformationKind.AssociationEndpoint);
            DeviceWatcher = DeviceInformation.CreateWatcher(selector);

            // デバイス情報更新時のハンドラを登録
            DeviceWatcher.Added += Watcher_DeviceAdded;
            DeviceWatcher.Updated += Watcher_DeviceUpdated;
            DeviceWatcher.Removed += Watcher_DeviceRemoved;
            DeviceWatcher.EnumerationCompleted += Watcher_EnumerationCompleted;
            DeviceWatcher.Stopped += Watcher_Stopped;

            // watcherスタート
            DeviceWatcher.Start();
        }

        private void StopWatcher()
        {
            // デバイス情報更新時のハンドラを登録
            DeviceWatcher.Added -= Watcher_DeviceAdded;
            DeviceWatcher.Updated -= Watcher_DeviceUpdated;
            DeviceWatcher.Removed -= Watcher_DeviceRemoved;
            DeviceWatcher.EnumerationCompleted -= Watcher_EnumerationCompleted;
            DeviceWatcher.Stopped -= Watcher_Stopped;

            // watcherスタート
            if (DeviceWatcherStatus.Started == DeviceWatcher.Status ||
                DeviceWatcherStatus.EnumerationCompleted == DeviceWatcher.Status)
            {
                DeviceWatcher.Stop();
            }
        }

        private async void Watcher_DeviceAdded(DeviceWatcher sender, DeviceInformation deviceInfo)
        {

            if (deviceInfo.Name == "EnvSensor-BL01" || deviceInfo.Name.Contains(SeosorServiceUuid.ToString()))
            {
                DeviceInformation = deviceInfo;
                //Console.WriteLine($"[{/*MethodBase.GetCurrentMethod().Name*/0}] デバイスを追加しました(Name:{deviceInfo.Name}, Kind:{deviceInfo.Kind}, IsPaired{deviceInfo.Pairing.IsPaired})");

                StopWatcher();

                //DoPairing(DeviceInformation);

                var ret = await ConnectToServiceForLatestData();
                Console.WriteLine($"[{/*MethodBase.GetCurrentMethod().Name*/0}] 接続結果：{ret}");
            }
        }

        private void Watcher_DeviceUpdated(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            if (DeviceInformation != null)
            {
                if (deviceInfoUpdate.Id == DeviceInformation.Id)
                {
                    DeviceInformation.Update(deviceInfoUpdate);
                    Console.WriteLine($"[{/*MethodBase.GetCurrentMethod().Name*/0}] デバイスをアップデートしました(Name:{DeviceInformation.Name}, Kind:{DeviceInformation.Kind}, IsPaired{DeviceInformation.Pairing.IsPaired})");
                }
            }
        }

        private void Watcher_DeviceRemoved(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            //Console.WriteLine("Watcher_DeviceUpdated");
            if (deviceInfoUpdate.Id == DeviceInformation.Id)
            {
                DeviceInformation.Update(deviceInfoUpdate);
                Console.WriteLine($"[{/*MethodBase.GetCurrentMethod().Name*/0}] デバイスを削除しました(Name:{DeviceInformation.Name}, Kind:{DeviceInformation.Kind}, IsPaired{DeviceInformation.Pairing.IsPaired})");
            }
        }

        private void Watcher_EnumerationCompleted(DeviceWatcher sender, object obj)
        {
            Console.WriteLine("Watcher_EnumerationCompleted");
        }

        private void Watcher_Stopped(DeviceWatcher sender, object obj)
        {
            Console.WriteLine("Watcher_Stopped");
        }

        private async Task<bool> ConnectToServiceForLatestData()
        {
            var ret = false;

            if (DeviceInformation == null) return false;

            // デバイスを、ペアリングしている対象の機器のIDからとってくる
            Device = await BluetoothLEDevice.FromIdAsync(DeviceInformation.Id);
            
            // その機器のサービスをとる
            var services = await Device.GetGattServicesForUuidAsync(SeosorServiceUuid);

            if (services != null)
            {
                // そのサービスから、目的のキャラクタリスティックのコレクションをとる
                var characteristics = await services.Services[0].GetCharacteristicsForUuidAsync(LatestDataCharacteristicUuid);
                // コレクションには(UUID指定してるから)1個しかないはずなので、それを使う
                LatestDataCharacteristic = characteristics.Characteristics[0];

                if (LatestDataCharacteristic != null)
                {

                    //if (status == GattCommunicationStatus.Success)
                    {
#if true
                        var cur = await LatestDataCharacteristic.ReadClientCharacteristicConfigurationDescriptorAsync();
                        Debug.WriteLine("Current Value : " + cur.ClientCharacteristicConfigurationDescriptor + " : " + cur.Status);

                        var status = GattCommunicationStatus.Unreachable;
                        while (status != GattCommunicationStatus.Success)
                        {
                            status = await LatestDataCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
                            Debug.WriteLine("result : " + status);
                        }

                        LatestDataCharacteristic.ValueChanged += ((s, a) =>
                        {
                            ShapeResponses(a.CharacteristicValue);
                        });
#else
                        //var status = await LatestDataCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);

                        GattCharacteristicProperties properties = LatestDataCharacteristic.CharacteristicProperties;
                        // 読み込みがサポートされてるか判定
                        if (properties.HasFlag(GattCharacteristicProperties.Read))
                        {
                            var t = Task.Run(async () =>
                            {
                                while (true)
                                {
                                    GattReadResult r = await LatestDataCharacteristic.ReadValueAsync();
                                    if (r.Status == GattCommunicationStatus.Success)
                                    {
                                        ShapeResponses(r.Value);
                                    }
                                    await Task.Delay(5000);
                                }
                            });
                        }
#endif

                        ret = true;
                    }
                    //else
                    //{
                    //    Debug.WriteLine($"[{/*MethodBase.GetCurrentMethod().Name*/0}] ConfigurationDescriptor設定に失敗しました(status = {status})");
                    //}
                }
                else
                {
                    Debug.WriteLine($"[{/*MethodBase.GetCurrentMethod().Name*/0}] キャラクタリスティック取得に失敗しました({LatestDataCharacteristicUuid})");
                }
            }
            else
            {
                Debug.WriteLine($"[{/*MethodBase.GetCurrentMethod().Name*/0}] サービス取得に失敗しました({SeosorServiceUuid})");
            }

            return ret;
        }

        private void ShapeResponses(IBuffer buf)
        {
            var reader = DataReader.FromBuffer(buf);
            byte[] input = new byte[reader.UnconsumedBufferLength];
            reader.ReadBytes(input);
            // Utilize the data as needed

            // データを整形
            double t = (double)(input[1] + 0x0100 * input[2]) / 100;    // 温度
            double h = (double)(input[3] + 0x0100 * input[4]) / 100;    // 湿度
            double i = (double)(input[5] + 0x0100 * input[6]);          // 照度
            double n = (double)(input[11] + 0x0100 * input[12]) / 100;  // 騒音
                                                                        //Debug.WriteLine("温度：" + t + " 湿度：" + h + " 照度：" + i + " 騒音：" + n);

            // ユーザーが登録したハンドラ実行
            LatestDataChanged?.Invoke(t, h, i, n);
        }

        /// <summary>
        /// ペアリング実施
        /// </summary>
        /// <param name="devInfo"></param>
        private async void DoPairing(DeviceInformation devInfo)
        {
            if (devInfo == null) return;

            if (devInfo.Pairing.IsPaired == false)
            {
                DeviceInformationCustomPairing customPairing = devInfo.Pairing.Custom;
                customPairing.PairingRequested += PairingRequestedHandler;
                DevicePairingResult result = await customPairing.PairAsync(DevicePairingKinds.ConfirmOnly, DevicePairingProtectionLevel.Default);
                customPairing.PairingRequested -= PairingRequestedHandler;
                Console.WriteLine("result is : " + result.Status);
                Debug.WriteLine($"[{/*MethodBase.GetCurrentMethod().Name*/0}] ペアリング結果：{result.Status}");
            }
            else
            {
                Debug.WriteLine($"[{/*MethodBase.GetCurrentMethod().Name*/0}] すでにペアリング済み");
            }
        }

        /// <summary>
        /// ペアリング解除実施
        /// </summary>
        /// <param name="devInfo"></param>
        private async void DoUnpairing(DeviceInformation devInfo)
        {
            if (devInfo == null) return;

            if (devInfo.Pairing.IsPaired == true)
            {
                var result = await devInfo.Pairing.UnpairAsync();
                Debug.WriteLine($"[{/*MethodBase.GetCurrentMethod().Name*/0}] ペアリング解除結果：{result.Status}");
            }
            else
            {
                Debug.WriteLine($"[{/*MethodBase.GetCurrentMethod().Name*/0}] すでにペアリング解除済み");
            }
        }

        /// <summary>
        /// ペアリング要求時のハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void PairingRequestedHandler(DeviceInformationCustomPairing sender, DevicePairingRequestedEventArgs args)
        {
            switch (args.PairingKind)
            {
                case DevicePairingKinds.ConfirmOnly:
                    // Windows itself will pop the confirmation dialog as part of "consent" if this is running on Desktop or Mobile
                    // If this is an App for 'Windows IoT Core' or a Desktop and Console application
                    // where there is no Windows Consent UX, you may want to provide your own confirmation.
                    args.Accept();
                    break;

                case DevicePairingKinds.ProvidePin:
                    // A PIN may be shown on the target device and the user needs to enter the matching PIN on 
                    // this Windows device. Get a deferral so we can perform the async request to the user.
                    var collectPinDeferral = args.GetDeferral();
                    string pinFromUser = "952693";
                    if (!string.IsNullOrEmpty(pinFromUser))
                    {
                        args.Accept(pinFromUser);
                    }
                    collectPinDeferral.Complete();
                    break;
            }
        }
    }
}