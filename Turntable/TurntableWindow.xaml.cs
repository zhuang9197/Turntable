using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using TurntableHelper.Helper;
using TurntableHelper.Model;

namespace Turntable
{
    /// <summary>
    /// TurntableWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TurntableWindow : Page
    {
        private static double angle = 0;
        public TurntableWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TextRadius.Text = 150.ToString();
            FillPathInCanvas();
        }


        private void StartLinearDecelerationRotationAnimation()
        {
            // 创建旋转动画
            Random random = new Random();

            // 随机生成旋转时间，大于5小于20秒
            double randomDuration = random.NextDouble() * (20 - 5) + 5;

            // 随机生成旋转角度，大于1小于3600度
            double randomAngle = random.NextDouble() * (3600 - 1) + 1;

            // 创建旋转动画
            DoubleAnimation rotationAnimation = new DoubleAnimation();

            // 设置动画属性
            rotationAnimation.From = angle;
            rotationAnimation.To = randomAngle;
            rotationAnimation.Duration = TimeSpan.FromSeconds(randomDuration);
            rotationAnimation.AccelerationRatio = 0; // 开始时无加速
            rotationAnimation.DecelerationRatio = 1; // 结束时减速

            angle = angle % 360;

            // 将动画应用于 RotateTransform
            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);
        }

        private void FillPathInCanvas()
        {
            myCanvas.Children.Clear();

            if (int.TryParse(TextRadius.Text, out int radius) && radius > 0)
            {
                SetPolygonMargin(radius);
                List<List<double>> pathDataList = ColorHelper.CoordinateGeneratorList(AwardsModel.Instance(), radius);

                for (var i = 0; i < AwardsModel.Instance().Count; i++)
                {
                    //PathGeometry pathGeometry = PathGeometry.CreateFromGeometry(Geometry.Parse(pathDataList[i])) as PathGeometry;
                    PathGeometry pathGeometry = new PathGeometry();
                    PathFigure pathFigure = new PathFigure { StartPoint = new Point(pathDataList[i][0], pathDataList[i][1]) };
                    var arcSegment = new ArcSegment
                    {
                        Point = new Point(pathDataList[i][2], pathDataList[i][3]),
                        //Size = ColorHelper.CalculateSize(pathDataList[i][4], pathDataList[i][5], 150),
                        Size = new Size(radius, radius),
                        SweepDirection = SweepDirection.Clockwise
                    };
                    var lineSegment = new LineSegment { Point = new Point(210, 210) };
                    pathFigure.Segments.Add(arcSegment);
                    pathFigure.Segments.Add(lineSegment);
                    pathGeometry.Figures.Add(pathFigure);
                    Path myPath = new Path
                    {
                        Stroke = Brushes.Black,
                        StrokeThickness = 0,
                        Fill = AwardsModel.Instance()[i].Color, // 设置填充颜色
                        Data = pathGeometry
                    };

                    // 添加到 Canvas 中
                    myCanvas.Children.Add(myPath);
                }
            }
            else
            {
                MessageBox.Show("半径不对捏");
            }


        }

        private void TurntableRotate_Click(object sender, RoutedEventArgs e)
        {
            StartLinearDecelerationRotationAnimation();
        }

        private void ConfirmRadius_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(TextRadius.Text, out int radius) && radius > 0)
            {
                FillPathInCanvas();
            }
            else
            {
                MessageBox.Show("半径不对捏");
            }
        }

        private void SetPolygonMargin(double value)
        {
            PointPolygon.Margin = new Thickness(PointPolygon.Margin.Left, 215 - value, PointPolygon.Margin.Right, PointPolygon.Margin.Bottom);
        }

        
    }
}
