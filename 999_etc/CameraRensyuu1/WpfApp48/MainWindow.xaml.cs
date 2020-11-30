using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
//using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Devices.Enumeration;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;

namespace WpfApp48
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


        #region LogFramework
        public ObservableCollection<string> Logs { get; set; } = new ObservableCollection<string>();

        public void AddLog(string log)
        {
            DateTime now = DateTime.Now;

            Logs.Add(now.ToString("hh:mm:ss.fff ") + log);
            OnPropertyChanged(nameof(Logs));
        }
        #endregion


        DeviceInformation di;
        MediaCapture mediaCapture = new MediaCapture();


        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            if (devices.Count == 0)
            {
                Console.WriteLine("カメラデバイスなし");
                return;
            }
            di = devices.FirstOrDefault();
            //list1.ItemsSource = devicelist;

            //list1.SelectedIndex = 0;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (di == null) return;

            {
                mediaCapture.Failed += (s, arg) =>
                {
                    Console.WriteLine("キャプチャ失敗");
                };

                MediaCaptureInitializationSettings setting = new MediaCaptureInitializationSettings();
                setting.VideoDeviceId = di.Id;//カメラ選択
                setting.StreamingCaptureMode = StreamingCaptureMode.Video;
                await mediaCapture.InitializeAsync(setting);

            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (var captureStream = new InMemoryRandomAccessStream())
            {
                var pngProperties = ImageEncodingProperties.CreatePng();
                //pngProperties.Width = (uint)pictureBox1.Width;
                //pngProperties.Height = (uint)pictureBox1.Height;
                pngProperties.Width = (uint)PreviewFrameImage.ActualWidth;
                pngProperties.Height = (uint)PreviewFrameImage.ActualHeight;

                AddLog("キャプチャ開始");
                //await mediaCapture.CapturePhotoToStreamAsync(pngProperties, captureStream);


                Windows.Storage.StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
                Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync("sample.bmp", Windows.Storage.CreationCollisionOption.ReplaceExisting);
                //await mediaCapture.CapturePhotoToStorageFileAsync(ImageEncodingProperties.CreatePng(), sampleFile);
                
                captureStream.Seek(0);

                AddLog("AsStream");
                //ビットマップにして表示
                System.IO.Stream stream = System.IO.WindowsRuntimeStreamExtensions.AsStream(captureStream);
                var img = System.Drawing.Bitmap.FromStream(stream);
                //this.pictureBox1.Image = img;
                //img.Save(Environment.SpecialFolder.Desktop + @"\aaa.bmp");

                AddLog("表示");
                // 画面に表示
                var a = System.Windows.Media.Imaging.BitmapFrame.Create(stream, System.Windows.Media.Imaging.BitmapCreateOptions.None, System.Windows.Media.Imaging.BitmapCacheOption.OnLoad);
                PreviewFrameImage.Source = a;

            }

        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            AddLog("開始");
            var lowLagCapture = await mediaCapture.PrepareLowLagPhotoCaptureAsync(ImageEncodingProperties.CreateUncompressed(MediaPixelFormat.Bgra8));

            var capturedPhoto = await lowLagCapture.CaptureAsync();
            var softwareBitmap = capturedPhoto.Frame.SoftwareBitmap;

            await lowLagCapture.FinishAsync();


            using (IRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                // Create the decoder from the stream
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

                // Get the SoftwareBitmap representation of the file
                softwareBitmap = await decoder.GetSoftwareBitmapAsync();


                // Create an encoder with the desired format
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);

                // Set the software bitmap
                encoder.SetSoftwareBitmap(softwareBitmap);

                await encoder.FlushAsync();






                //ビットマップにして表示
                System.IO.Stream stream2 = System.IO.WindowsRuntimeStreamExtensions.AsStream(stream);
                var img = System.Drawing.Bitmap.FromStream(stream2);
                //this.pictureBox1.Image = img;
                //img.Save(Environment.SpecialFolder.Desktop + @"\aaa.bmp");

                AddLog("表示");
                // 画面に表示
                var a = System.Windows.Media.Imaging.BitmapFrame.Create(stream2, System.Windows.Media.Imaging.BitmapCreateOptions.None, System.Windows.Media.Imaging.BitmapCacheOption.OnLoad);
                PreviewFrameImage.Source = a;
            }

            AddLog("修了");
        }
    }
}
