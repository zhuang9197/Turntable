using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TurntableHelper.Model;

namespace TurntableHelper.ViewModel
{
    public class AwardsVModel
    {
        
        public List<List<AwardsViewModel>> Rows { get; set; }

        public AwardsVModel()
        {
            // Initialize the Rows with different colors
            Rows = new List<List<AwardsViewModel>> {
                new List<AwardsViewModel>
            {
                new AwardsViewModel { Color = Brushes.Red, Text = "Text 1" },
                new AwardsViewModel { Color = Brushes.Green, Text = "Text 2" },
                new AwardsViewModel { Color = Brushes.Blue, Text = "Text 3" }
            },
            new List<AwardsViewModel>
            {
                new AwardsViewModel { Color = Brushes.Yellow, Text = "Text 4" },
                new AwardsViewModel { Color = Brushes.Orange, Text = "Text 5" },
                new AwardsViewModel { Color = Brushes.Purple, Text = "Text 6" },
                new AwardsViewModel { Color = Brushes.Aqua, Text = "Text 7" }
            },
            };
        }
        
    }
}
