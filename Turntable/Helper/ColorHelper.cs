using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TurntableHelper.Model;

namespace Turntable.Helper
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
    }
}
