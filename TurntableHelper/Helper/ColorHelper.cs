using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TurntableHelper.Model;

namespace TurntableHelper.Helper
{
    public class ColorHelper
    {
        private static readonly Random random = new Random();

        public static Brush SolidColorBrush(ObservableCollection<AwardsModel> awards)
        {
            while (true)
            {
                // 生成一个随机颜色
                byte[] colorBytes = new byte[3];
                random.NextBytes(colorBytes);

                Color randomColor = Color.FromRgb(colorBytes[0], colorBytes[1], colorBytes[2]);

                // 检查颜色是否已经存在于 awards 列表中
                if (!awards.Any(a => ((SolidColorBrush)a.Color).Color == randomColor))
                {
                    return new SolidColorBrush(randomColor);
                }
            }
        }

        public static List<List<double>> CoordinateGeneratorList(ObservableCollection<AwardsModel> awards,double radius) 
        {
            

            var coordinates = new List<Tuple<Point, Point>>();
            
            Point Center = new Point(210, 210);
            Point start = new Point(210+radius, 210);

            List<List<double>> resultList = new List<List<double>>();
            
            double currentAngle = 0;


            foreach (var item in awards)
            {
                // 计算角度对应的弧度
                currentAngle += item.Probability;
                double radians = currentAngle * Math.PI / 180;
                
                // 计算结束坐标
                Point end = new Point(Center.X + radius * Math.Cos(radians), Center.Y + radius * Math.Sin(radians));
                
                // 添加到坐标集合
                coordinates.Add(new Tuple<Point, Point>(start, end));

                resultList.Add(new List<double> { start.X,start.Y,end.X,end.Y,currentAngle - item.Probability,currentAngle });

                // 下一个圆弧的起始坐标是当前圆弧的结束坐标
                start = end;
            }

            

            return resultList;
        }

        public static Size CalculateSize(double startAngle, double endAngle, double radius)
        {
            // 角度转弧度
            double startAngleRad = startAngle * (Math.PI / 180);
            double endAngleRad = endAngle * (Math.PI / 180);

            // 计算起始点和结束点的坐标
            double startX = Math.Cos(startAngleRad) * radius;
            double startY = Math.Sin(startAngleRad) * radius;

            double endX = Math.Cos(endAngleRad) * radius;
            double endY = Math.Sin(endAngleRad) * radius;

            // 找到最小和最大的 x 和 y 坐标
            double minX = Math.Min(startX, endX);
            double minY = Math.Min(startY, endY);
            double maxX = Math.Max(startX, endX);
            double maxY = Math.Max(startY, endY);

            // 计算 Size
            double sizeWidth = Math.Abs(maxX - minX);
            double sizeHeight = Math.Abs(maxY - minY);

            return new Size(sizeWidth, sizeHeight);
        }
    }

    
}
