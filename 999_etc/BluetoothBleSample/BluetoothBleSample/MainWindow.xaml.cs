using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

namespace BluetoothBleSample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private BluetoothLEAdvertisementWatcher watcher;

        private GattDeviceService GattDeviceService { get; set; }

        private GattCharacteristic GattCharacteristic { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            //this.watcher = new BluetoothLEAdvertisementWatcher();



            //// rssi >= -60のとき受信開始するっぽい
            //this.watcher.SignalStrengthFilter.InRangeThresholdInDBm = -60;
            //// rssi <= -65が2秒続いたら受信終わるっぽい
            //this.watcher.SignalStrengthFilter.OutOfRangeThresholdInDBm = -65;
            //this.watcher.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(2000);
            //this.watcher.Received += this.Watcher_Received;

            //this.watcher.Start();

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
            if (EnvSensorDeviceInformation == null || EnvSensorDeviceInformation.Pairing.IsPaired == false) return;

            //// SensorTagを取得
            //var selector = GattDeviceService.GetDeviceSelectorFromUuid(new Guid("00001800-0000-1000-8000-00805f9b34fb"));
            var selector = GattDeviceService.GetDeviceSelectorFromUuid(new Guid("0C4C3000-7700-46F4-AA96-D5E974E32A54"));
            var devices = await DeviceInformation.FindAllAsync(selector);
            var deviceInformation = devices.FirstOrDefault();
            if (deviceInformation == null)
            {
                MessageBox.Show("not found");
                return;
            }
            device = await BluetoothLEDevice.FromIdAsync(deviceInformation.Id);
            //MessageBox.Show($"found {deviceInformation.Id}");
            services = await device.GetGattServicesForUuidAsync(new Guid("0C4C3000-7700-46F4-AA96-D5E974E32A54"));
            characteristics = await services.Services[0].GetCharacteristicsForUuidAsync(new Guid("0C4C3001-7700-46F4-AA96-D5E974E32A54"));

            characteristics.Characteristics[0].ValueChanged += characteristicChanged;

            await characteristics.Characteristics[0].WriteClientCharacteristicConfigurationDescriptorAsync(
                GattClientCharacteristicConfigurationDescriptorValue.Notify
            );
        }

        private BluetoothLEDevice device;
        private GattDeviceServicesResult services;
        private GattCharacteristicsResult characteristics;

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
            Console.WriteLine("Watcher_DeviceAdded : " + deviceInfo.Name + "  " + deviceInfo.Kind + "  " + deviceInfo.Pairing.IsPaired);

            if (deviceInfo.Name == "EnvSensor-BL01")
            {
                EnvSensorDeviceInformation = deviceInfo;
            }
        }

        private async void Watcher_DeviceUpdated(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            Console.WriteLine("Watcher_DeviceUpdated");
            if (deviceInfoUpdate.Id == EnvSensorDeviceInformation.Id)
            {
                EnvSensorDeviceInformation.Update(deviceInfoUpdate);
            }
        }

        private async void Watcher_DeviceRemoved(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            Console.WriteLine("Watcher_DeviceRemoved");

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
