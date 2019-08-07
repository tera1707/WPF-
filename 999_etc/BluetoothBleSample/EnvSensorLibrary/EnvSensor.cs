using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;

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
            // Bluetooth v4（BLE）を指定
            string selector = "(" + "System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\"" + ")";

            DeviceWatcher = DeviceInformation.CreateWatcher(selector, null, DeviceInformationKind.AssociationEndpoint);

            // デバイス情報更新時のハンドラを登録
            DeviceWatcher.Added += Watcher_DeviceAdded;
            DeviceWatcher.Updated += Watcher_DeviceUpdated;
            DeviceWatcher.Removed += Watcher_DeviceRemoved;
            DeviceWatcher.EnumerationCompleted += Watcher_EnumerationCompleted;
            DeviceWatcher.Stopped += Watcher_Stopped;

            // watcherスタート
            DeviceWatcher.Start();
        }

        private async void Watcher_DeviceAdded(DeviceWatcher sender, DeviceInformation deviceInfo)
        {

            if (deviceInfo.Name == "EnvSensor-BL01")
            {
                DeviceInformation = deviceInfo;
                Console.WriteLine("Watcher_DeviceAdded : " + deviceInfo.Name + "  " + deviceInfo.Kind + "  " + deviceInfo.Pairing.IsPaired);

                var ret = await ConnectToServiceForLatestData();
                Debug.WriteLine("Connect To Service : " + ret);
            }
        }

        private void Watcher_DeviceUpdated(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            if (deviceInfoUpdate.Id == DeviceInformation.Id)
            {
                DeviceInformation.Update(deviceInfoUpdate);
                Console.WriteLine("Watcher_DeviceUpdated : " + DeviceInformation.Name + "  " + DeviceInformation.Kind + "  " + DeviceInformation.Pairing.IsPaired);
            }
        }

        private void Watcher_DeviceRemoved(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            //Console.WriteLine("Watcher_DeviceUpdated");
            if (deviceInfoUpdate.Id == DeviceInformation.Id)
            {
                DeviceInformation.Update(deviceInfoUpdate);
                Console.WriteLine("Watcher_DeviceRemoved : " + DeviceInformation.Name + "  " + DeviceInformation.Kind + "  " + DeviceInformation.Pairing.IsPaired);
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
            // そのサービスから、目的のキャラクタリスティックのコレクションをとる
            var characteristics = await services.Services[0].GetCharacteristicsForUuidAsync(LatestDataCharacteristicUuid);
            // コレクションには(UUID指定してるから)1個しかないはずなので、それを使う
            LatestDataCharacteristic = characteristics.Characteristics.FirstOrDefault();

            if (LatestDataCharacteristic != null)
            {
                var status = GattCommunicationStatus.Unreachable;
                status = await LatestDataCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);

                if (status == GattCommunicationStatus.Success)
                {
                    LatestDataCharacteristic.ValueChanged += ((s, a) =>
                    {
                        var reader = DataReader.FromBuffer(a.CharacteristicValue);
                        byte[] input = new byte[reader.UnconsumedBufferLength];
                        reader.ReadBytes(input);
                        // Utilize the data as needed

                        // データを整形
                        double t = (double)(input[1] + 0x0100 * input[2]) / 100;    // 温度
                        double h = (double)(input[3] + 0x0100 * input[4]) / 100;    // 湿度
                        double i = (double)(input[5] + 0x0100 * input[6]) / 100;    // 照度
                        double n = (double)(input[11] + 0x0100 * input[12]) / 100;    // 騒音
                        Debug.WriteLine("温度：" + t + " 湿度：" + h + " 照度：" + i + " 騒音：" + n);

                        // ユーザーが登録したハンドラ実行
                        LatestDataChanged?.Invoke(t, h, i, n);
                    });

                    ret = true;
                }
            }

            return ret;
        }
    }
}