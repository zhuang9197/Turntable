using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace Turntable
{
    /// <summary>
    /// TurntableWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TurntableWindow : Window
    {
        public TurntableWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillPathInCanvas();
        }


        private void StartLinearDecelerationRotationAnimation()
        {
            // 创建旋转动画
            DoubleAnimation rotationAnimation = new DoubleAnimation();

            // 设置动画属性
            rotationAnimation.From = 0;
            rotationAnimation.To = 360;
            rotationAnimation.Duration = TimeSpan.FromSeconds(10); // 设置持续时间为10秒
            rotationAnimation.AccelerationRatio = 0; // 开始时无加速
            rotationAnimation.DecelerationRatio = 1; // 结束时减速

            // 将动画应用于 RotateTransform
            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);
        }

        private void FillPathInCanvas()
        {

            //< Path Data = "M 150 250 A 50,50 0 1,0 450,250" Fill = "Red" />
            //< Path Data = "M 450,250 A50,50 0 1,0 150,250" Fill = "Blue" />
            string[] pathDataString = { "M 60 210 A 50,50 0 1,0 360,210", "M 360,210 A50,50 0 1,0 60,210" };
            Brush[] br = new Brush[pathDataString.Length];
            br[0] = Brushes.Blue;
            br[1] = Brushes.Red;
            for(var i = 0; i < 2; i++)
            {
                PathGeometry pathGeometry = PathGeometry.CreateFromGeometry(Geometry.Parse(pathDataString[i])) as PathGeometry;

                Path myPath = new Path
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 5,
                    Fill = br[i], // 设置填充颜色
                    Data = pathGeometry
                };

                // 添加到 Canvas 中
                myCanvas.Children.Add(myPath);
            }
            
        }
    }
}
