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
using TurntableHelper.ViewModel;

namespace Turntable
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel();
        }

        public class ViewModel
        {
            public List<List<AwardsViewModel>> Rows { get; set; }

            public ViewModel()
            {
                // Initialize the Rows with different colors and text
                Rows = new List<List<AwardsViewModel>>
        {
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
                new AwardsViewModel { Color = Brushes.Purple, Text = "Text 6" }
            },
            // Add more rows with different colors and text
        };
            }
        }

    }
}
