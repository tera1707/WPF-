using System;
using System.Collections.Generic;
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
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;


using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Media;

namespace WpfApp58
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            if (devices.Count == 0)
            {
                Console.WriteLine("カメラデバイスなし");
                return;
            }
            var devicelist = devices.ToArray();
            list1.ItemsSource = devicelist;

            list1.SelectedIndex = 0;


        }
        MediaCapture mediaCapture = new MediaCapture();
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var di = (DeviceInformation)list1.SelectedItem;

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

                ////調整しないと暗い場合があるので
                //var vcon = mediaCapture.VideoDeviceController;
                //double max = vcon.Brightness.Capabilities.Max;
                //Console.WriteLine(max);


                //bool brightnessAutoResult = vcon.Brightness.TrySetAuto(true);
                //if (!brightnessAutoResult)
                //{
                //    Console.WriteLine("Brightness is manually set.");
                //    Console.WriteLine(vcon.Brightness.TrySetValue(max));
                //}
                //Console.WriteLine(vcon.Contrast.TrySetAuto(true));

                //await mediaCapture.StartPreviewAsync();
            }
        }

            

            //MediaCapture _mediaCapture = new MediaCapture();
            //private async Task GetPreviewFrameAsSoftwareBitmapAsync()
            //{
            //    // Get information about the preview
            //    var previewProperties = _mediaCapture.VideoDeviceController.GetMediaStreamProperties(MediaStreamType.VideoPreview) as VideoEncodingProperties;

            //    // Create the video frame to request a SoftwareBitmap preview frame
            //    var videoFrame = new VideoFrame(BitmapPixelFormat.Bgra8, (int)previewProperties.Width, (int)previewProperties.Height);

            //    // Capture the preview frame
            //    using (var currentFrame = await _mediaCapture.GetPreviewFrameAsync(videoFrame))
            //    {
            //        // Collect the resulting frame
            //        SoftwareBitmap previewFrame = currentFrame.SoftwareBitmap;

            //        // Copy the SoftwareBitmap to a WriteableBitmap to display it to the user
            //        var wb = new Windows.UI.Xaml.Media.Imaging.WriteableBitmap(previewFrame.PixelWidth, previewFrame.PixelHeight);
            //        previewFrame.CopyToBuffer(wb.PixelBuffer);

            //        // Display it in the Image control
            //        //PreviewFrameImage.Source = wb;
            //    }
            //}

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (var captureStream = new InMemoryRandomAccessStream())
            {
                var pngProperties = ImageEncodingProperties.CreatePng();
                //pngProperties.Width = (uint)pictureBox1.Width;
                //pngProperties.Height = (uint)pictureBox1.Height;
                pngProperties.Width = (uint)PreviewFrameImage.ActualWidth;
                pngProperties.Height = (uint)PreviewFrameImage.ActualHeight;

                await mediaCapture.CapturePhotoToStreamAsync(pngProperties, captureStream);

                Windows.Storage.StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
                Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync("sample.bmp", Windows.Storage.CreationCollisionOption.ReplaceExisting);
                await mediaCapture.CapturePhotoToStorageFileAsync(ImageEncodingProperties.CreatePng(), sampleFile);

                captureStream.Seek(0);

                //ビットマップにして表示
                System.IO.Stream stream = System.IO.WindowsRuntimeStreamExtensions.AsStream(captureStream );
                var img = System.Drawing.Bitmap.FromStream(stream);
                //this.pictureBox1.Image = img;
                //img.Save(Environment.SpecialFolder.Desktop + @"\aaa.bmp");

                // 画面に表示
                var a = System.Windows.Media.Imaging.BitmapFrame.Create(stream, System.Windows.Media.Imaging.BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                PreviewFrameImage.Source = a;



                //var lowLagCapture = await mediaCapture.PrepareLowLagPhotoCaptureAsync(ImageEncodingProperties.CreateUncompressed(MediaPixelFormat.Bgra8));

                //var capturedPhoto = await lowLagCapture.CaptureAsync();
                //var softwareBitmap = capturedPhoto.Frame.SoftwareBitmap;

                //await lowLagCapture.FinishAsync();

                //SoftwareBitmap frameBitmap = softwareBitmap;





                //System.Windows.Media.Imaging.WriteableBitmap bitmap = new WriteableBitmap(frameBitmap.PixelWidth, frameBitmap.PixelHeight);

                //frameBitmap.CopyToBuffer(bitmap.PixelBuffer);

                //Debug.WriteLine("done");



                //Windows.Graphics.Imaging.BitmapEncoder encoder = await Windows.Graphics.Imaging.BitmapEncoder.CreateAsync(Windows.Graphics.Imaging.BitmapEncoder.JpegEncoderId, captureStream);
                ////encoder.SetPixelData(BitmapPixelFormat.Rgba8, BitmapAlphaMode.Ignore, (uint)MyImage.ActualWidth, (uint)MyImage.ActualHeight, 96.0, 96.0, data);

                //await encoder.FlushAsync();
                //var bitmapImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                //bitmapImage.SetSource(captureStream);
                //MyImage.Source = bitmapImage;



                //using (var stream = new Windows.Storage.Streams.InMemoryRandomAccessStream())
                //{
                //    var encoder = await Windows.Graphics.Imaging.BitmapEncoder.CreateAsync(Windows.Graphics.Imaging.BitmapEncoder.PngEncoderId, stream);
                //    encoder.SetSoftwareBitmap(softwareBitmap);
                //    await encoder.FlushAsync();
                //    var bmp = new System.Drawing.Bitmap(stream.AsStream());
                //}
            }
            
        }
    }
}
