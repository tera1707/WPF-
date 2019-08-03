using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;
using OxyPlot;

namespace BluetoothBleSample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<DataPoint> DataList { get; }


        public MainWindow()
        {
            InitializeComponent();


            DataList = new List<DataPoint>();
            //{
            //    {new DataPoint(0, 0)},
            //    {new DataPoint(2, 4)},
            //    {new DataPoint(5, 8)},
            //    {new DataPoint(8, 3)},
            //    {new DataPoint(12, 5)},
            //};

            StartWatcher();
        }

        private async void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            await Task.Run(() =>
            {
                var md = args.Advertisement.ManufacturerData.FirstOrDefault();
                if (md != null)
                {
                    // ManufactureDataをもとにCompanyIDとったりできる

                }

                this.Dispatcher.Invoke(()=>
                {
                    this.TextBlockRSSI.Text = $"{args.Timestamp:HH\\:mm\\:ss}, RSSI: {args.RawSignalStrengthInDBm}, Address: {args.BluetoothAddress.ToString("X")}, Type: {args.AdvertisementType}";
                });

            });

        }
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

        // データ取得
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //var t = Task.Run(async ()=>
            {
                //while (true)
                {

                    if (EnvSensorDeviceInformation == null || EnvSensorDeviceInformation.Pairing.IsPaired == false) return;

                    // デバイスを、ペアリングしている対象の機器のIDからとってくる
                    device = await BluetoothLEDevice.FromIdAsync(EnvSensorDeviceInformation.Id);

                    // その機器のサービスをとる(これいらんかも)
                    GattDeviceServicesResult result = await device.GetGattServicesAsync();

                    if (result.Status == GattCommunicationStatus.Success)
                    {
                        // その機器のサービスをとる
                        services = await device.GetGattServicesForUuidAsync(new Guid("0C4C3000-7700-46F4-AA96-D5E974E32A54"));
                        // そのサービスから、目的のキャラクタリスティックのコレクションをとる
                        characteristics = await services.Services[0].GetCharacteristicsForUuidAsync(new Guid("0C4C3001-7700-46F4-AA96-D5E974E32A54"));
                        // コレクションには(UUID指定してるから)1個しかないはずなので、それを使う
                        characteristic = characteristics.Characteristics.FirstOrDefault();

                        if (characteristic != null)
                        {

                            GattCommunicationStatus status = GattCommunicationStatus.Unreachable;
                            var cccdValue = GattClientCharacteristicConfigurationDescriptorValue.Notify;
                            status = await characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(cccdValue);
                            if (status == GattCommunicationStatus.Success)
                            {
                                characteristic.ValueChanged += ((s, a) =>
                                {
                                    //GattReadResult r = await c.ReadValueAsync();
                                    //if (r.Status == GattCommunicationStatus.Success)
                                    {
                                        var reader = DataReader.FromBuffer(a.CharacteristicValue);
                                        byte[] input = new byte[reader.UnconsumedBufferLength];
                                        reader.ReadBytes(input);
                                        // Utilize the data as needed

                                        // データを整形
                                        double temparature = (double)(input[1] + 0x0100 * input[2]) / 100;
                                        //var str = System.Text.Encoding.ASCII.GetString(data);
                                        Debug.WriteLine("温度：" + temparature);
                                        this.Dispatcher.Invoke(() =>
                                        {
                                            TextBlockRSSI.Text = "温度：" + temparature;

                                            DataList.Add(new DataPoint(count++, temparature));
                                            if (DataList.Count() > 1000) DataList.RemoveAt(0);
                                            graph.InvalidatePlot();
                                        });
                                    }
                                });
                            }
                            else
                            {
                                Console.WriteLine("設定sipoai:" + status);
                            }


                            // 読み込みがサポートされてるか判定
                            //if (properties.HasFlag(GattCharacteristicProperties.Read))
                            //{
                            //}
                        }
                    }
                }
            }//);
        }

        int count = 0;

        // 温度間隔書き込み
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {

            // デバイスを、ペアリングしている対象の機器のIDからとってくる
            device = await BluetoothLEDevice.FromIdAsync(EnvSensorDeviceInformation.Id);

            // その機器のサービスをとる(これいらんかも)
            GattDeviceServicesResult result = await device.GetGattServicesAsync();

            if (result.Status == GattCommunicationStatus.Success)
            {
                // その機器のサービスをとる->3010:SetingService
                services = await device.GetGattServicesForUuidAsync(new Guid("0C4C3010-7700-46F4-AA96-D5E974E32A54"));
                // そのサービスから、目的のキャラクタリスティックのコレクションをとる（測定間隔）
                characteristics = await services.Services[0].GetCharacteristicsForUuidAsync(new Guid("0C4C3011-7700-46F4-AA96-D5E974E32A54"));
                // コレクションには(UUID指定してるから)1個しかないはずなので、それを使う
                characteristic = characteristics.Characteristics.FirstOrDefault();

                if (characteristic != null)
                {
                    GattCharacteristicProperties properties = characteristic.CharacteristicProperties;

                    // 書き込みがサポートされてるか判定
                    if (properties.HasFlag(GattCharacteristicProperties.Write))
                    {
                        var writer = new DataWriter();
                        // WriteByte used for simplicity. Other common functions - WriteInt16 and WriteSingle
                        writer.ByteOrder = ByteOrder.LittleEndian;
                        writer.WriteInt16(5);
                        GattCommunicationStatus r = await characteristic.WriteValueAsync(writer.DetachBuffer());
                        if (r == GattCommunicationStatus.Success)
                        {
                            // Successfully wrote to device
                            Console.WriteLine("設定成功");
                        }
                    }

                    // 読み込みがサポートされてるか判定
                    if (properties.HasFlag(GattCharacteristicProperties.Read))
                    {
                        GattReadResult r = await characteristic.ReadValueAsync();
                        if (r.Status == GattCommunicationStatus.Success)
                        {
                            var reader = DataReader.FromBuffer(r.Value);
                            byte[] input = new byte[reader.UnconsumedBufferLength];
                            reader.ReadBytes(input);
                            // Utilize the data as needed

                            // データを整形
                            var interval = (int)(input[0] + 0x0100 * input[1]);
                            //var str = System.Text.Encoding.ASCII.GetString(data);
                            Debug.WriteLine("温度監視間隔：" + interval + " 秒");
                        }
                    }
                }
            }
        }

        private BluetoothLEDevice device;
        private GattDeviceServicesResult services;
        private GattCharacteristicsResult characteristics;
        private GattCharacteristic characteristic;

        void characteristicChanged(GattCharacteristic sender, GattValueChangedEventArgs eventArgs)
        {
            byte[] data = new byte[eventArgs.CharacteristicValue.Length];

            Windows.Storage.Streams.DataReader.FromBuffer(eventArgs.CharacteristicValue).ReadBytes(data);
            double temparature = (double)(data[1] + 0x0100 * data[2]) / 100;
            //var str = System.Text.Encoding.ASCII.GetString(data);
            Debug.WriteLine("温度：" + temparature);
            this.Dispatcher.Invoke(() =>
            {
                TextBlockRSSI.Text = "温度：" + temparature;
            });
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (EnvSensorDeviceInformation == null) return;

            DevicePairingResult result1;
            DeviceUnpairingResult result2;

            if (EnvSensorDeviceInformation.Pairing.IsPaired == false)
            {
                DeviceInformationCustomPairing customPairing = EnvSensorDeviceInformation.Pairing.Custom;
                customPairing.PairingRequested += PairingRequestedHandler;
                DevicePairingResult result = await customPairing.PairAsync(DevicePairingKinds.ConfirmOnly, DevicePairingProtectionLevel.Default);
                customPairing.PairingRequested -= PairingRequestedHandler;
                Console.WriteLine("result is : " + result.Status);
            }
            else
            {
                result2 = await EnvSensorDeviceInformation.Pairing.UnpairAsync();
                Console.WriteLine("result is : " + result2.Status);
            }



            //// センサーの有効化?
            ////var configCharacteristic = this.GattDeviceService.GetCharacteristics(new Guid(SensorTagUuid.UuidIrtConf)).First();
            ////var status = await configCharacteristic.WriteValueAsync(new byte[] { 1 }.AsBuffer());
            ////if (status == GattCommunicationStatus.Unreachable)
            ////{
            ////    MessageBox.Show("Initialize failed");
            ////    return;
            ////}


        }
        private DeviceWatcherHelper deviceWatcherHelper;

        private DeviceWatcher deviceWatcher;
        private void StartWatcher()
        {
            //        public static DeviceSelectorInfo BluetoothLE =>
            //new DeviceSelectorInfo() { DisplayName = "Bluetooth LE", Selector = "System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\"", Kind = DeviceInformationKind.AssociationEndpoint };

            // Get the device selector chosen by the UI then add additional constraints for devices that
            // can be paired or are already paired.
            // DeviceSelectorInfo deviceSelectorInfo = (DeviceSelectorInfo)selectorComboBox.SelectedItem;
            //string selector = "(" + "System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\"" + ")" + " AND (System.Devices.Aep.CanPair:=System.StructuredQueryType.Boolean#True OR System.Devices.Aep.IsPaired:=System.StructuredQueryType.Boolean#True)";
            string selector = "(" + "System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\"" + ")";
            string[] requestedProperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected", "System.Devices.Aep.IsPaired" };

            // Kind is specified in the selector info
            deviceWatcher = DeviceInformation.CreateWatcher(
                selector,
                null,
                DeviceInformationKind.AssociationEndpoint);

            //deviceWatcher = DeviceInformation.CreateWatcher();

            // Connect events to update our collection as the watcher report results.
            deviceWatcher.Added += Watcher_DeviceAdded;
            deviceWatcher.Updated += Watcher_DeviceUpdated;
            deviceWatcher.Removed += Watcher_DeviceRemoved;
            deviceWatcher.EnumerationCompleted += Watcher_EnumerationCompleted;
            deviceWatcher.Stopped += Watcher_Stopped;


            deviceWatcher.Start();
        }

        DeviceInformation EnvSensorDeviceInformation = null;

        private async void Watcher_DeviceAdded(DeviceWatcher sender, DeviceInformation deviceInfo)
        {

            if (deviceInfo.Name == "EnvSensor-BL01")
            {
                EnvSensorDeviceInformation = deviceInfo;
                Console.WriteLine("Watcher_DeviceAdded : " + deviceInfo.Name + "  " + deviceInfo.Kind + "  " + deviceInfo.Pairing.IsPaired);
            }
            if (deviceInfo.Name == "Env")
            {
                EnvSensorDeviceInformation = deviceInfo;
                Console.WriteLine("Watcher_DeviceAdded : " + deviceInfo.Name + "  " + deviceInfo.Kind + "  " + deviceInfo.Pairing.IsPaired);
            }
        }

        private async void Watcher_DeviceUpdated(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            //Console.WriteLine("Watcher_DeviceUpdated");
            if (deviceInfoUpdate.Id == EnvSensorDeviceInformation.Id)
            {
                EnvSensorDeviceInformation.Update(deviceInfoUpdate);
                Console.WriteLine("Watcher_DeviceUpdated : " + EnvSensorDeviceInformation.Name + "  " + EnvSensorDeviceInformation.Kind + "  " + EnvSensorDeviceInformation.Pairing.IsPaired);
            }
        }

        private async void Watcher_DeviceRemoved(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            //Console.WriteLine("Watcher_DeviceUpdated");
            if (deviceInfoUpdate.Id == EnvSensorDeviceInformation.Id)
            {
                EnvSensorDeviceInformation.Update(deviceInfoUpdate);
                Console.WriteLine("Watcher_DeviceRemoved : " + EnvSensorDeviceInformation.Name + "  " + EnvSensorDeviceInformation.Kind + "  " + EnvSensorDeviceInformation.Pairing.IsPaired);
            }
        }

        private async void Watcher_EnumerationCompleted(DeviceWatcher sender, object obj)
        {
            Console.WriteLine("Watcher_EnumerationCompleted");
        }

        private async void Watcher_Stopped(DeviceWatcher sender, object obj)
        {
            Console.WriteLine("Watcher_Stopped");
        }
    }
}