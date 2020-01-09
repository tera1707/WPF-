using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApp39
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// マウス押下中フラグ
        /// </summary>
        bool isMouseLeftButtonDown = false;

        /// <summary>
        /// マウスを押下した点を保存
        /// </summary>
        Point MouseDonwStartPoint = new Point(0, 0);

        /// <summary>
        /// マウスの現在地
        /// </summary>
        Point MouseCurrentPoint = new Point(0, 0);

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 【指】
        /// 移動時のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            var delta = e.DeltaManipulation;
            Matrix matrix = (MyTarget.RenderTransform as MatrixTransform).Matrix;
            matrix.Translate(delta.Translation.X, delta.Translation.Y);

            var scaleDelta = delta.Scale.X;
            var orgX = e.ManipulationOrigin.X;
            var orgY = e.ManipulationOrigin.Y;
            matrix.ScaleAt(scaleDelta, scaleDelta, orgX, orgY);
            MyTarget.RenderTransform = new MatrixTransform(matrix);
        }

        /// <summary>
        /// 【マウス】
        /// マウス押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // クリックした位置を保存
            // 位置の基準にするControlはなんでもいいが、MouseMoveのほうの基準Controlと合わせること。
            MouseDonwStartPoint = e.GetPosition(MyScrollViewer);

            isMouseLeftButtonDown = true;
        }

        /// <summary>
        /// 【マウス】
        /// マウス離す
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isMouseLeftButtonDown = false;
        }

        /// <summary>
        /// 【マウス】
        /// マウスがコントロールの上から外れた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseLeftButtonDown = false;
        }

        /// <summary>
        /// 【マウス】
        /// マウスがコントロールの上を移動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseLeftButtonDown == false) return;

            // マウスの現在位置座標を取得（ScrollViewerからの相対位置）
            // ここは、位置の基準にするControl(GetPositionの引数)はScrollViewrでもthis(Window自体)でもなんでもいい。
            // Start時とマウス移動時の差分がわかりさえすればよし。
            MouseCurrentPoint = e.GetPosition(MyScrollViewer);

            // 移動開始点と現在位置の差から、MouseMoveイベント1回分の移動量を算出
            double offsetX = MouseCurrentPoint.X - MouseDonwStartPoint.X;
            double offsetY = MouseCurrentPoint.Y - MouseDonwStartPoint.Y;

            // 動かす対象の図形からMatrixオブジェクトを取得
            // このMatrixオブジェクトを用いて図形を描画上移動させる
            Matrix matrix = ((MatrixTransform)MyTarget.RenderTransform).Matrix;

            // TranslateメソッドにX方向とY方向の移動量を渡し、移動後の状態を計算
            matrix.Translate(offsetX, offsetY);

            // 移動後の状態を計算したMatrixオブジェクトを描画に反映する
            MyTarget.RenderTransform = new MatrixTransform(matrix);
            MyTarget2.RenderTransform = new MatrixTransform(matrix);

            // 移動開始点を現在位置で更新する
            // （今回の現在位置が次回のMouseMoveイベントハンドラで使われる移動開始点となる）
            MouseDonwStartPoint = MouseCurrentPoint;
        }

        /// <summary>
        /// 【マウス】
        /// ホイールくるくる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyGrid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scale = 1.0;
            Matrix matrix = ((MatrixTransform)MyTarget.RenderTransform).Matrix;

            // ScaleAt()の拡大中心点(引数3,4個目)に渡すための座標をとるときの基準Controlは、拡大縮小をしたいものの一つ上のControlにすること。
            // ここでは拡大縮小するGridを包んでいるScrollViewerを基準にした。
            MouseCurrentPoint = e.GetPosition(MyScrollViewer);

            // ホイール上に回す→拡大 / 下に回す→縮小
            if (e.Delta > 0) scale = 1.25;
            else scale = 1 / 1.25;

            Debug.WriteLine($"倍率：{scale} 中心点：{MouseCurrentPoint} 大きさ：({MyTarget.ActualWidth},{MyTarget.ActualHeight})");

            // 拡大実施
            matrix.ScaleAt(scale, scale, MouseCurrentPoint.X, MouseCurrentPoint.Y);
            MyTarget.RenderTransform = new MatrixTransform(matrix);
            MyTarget2.RenderTransform = new MatrixTransform(matrix);
        }
    }
}