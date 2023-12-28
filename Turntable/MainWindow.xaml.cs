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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Turntable.Helper;
using TurntableHelper.Helper;
using TurntableHelper.Model;
using TurntableHelper.ViewModel;

namespace Turntable
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isDragging = false;
        private Point lastMousePosition;
        private bool isPop = false;
        private ViewModel viewModel;
        private static ObservableCollection<AwardsModel> _awards = new ObservableCollection<AwardsModel>();
        private bool flag = false;

        public MainWindow()
        {
            InitializeComponent();

            if(App.Awards != null)
            {
                foreach(var item in App.Awards)
                {
                    _awards.Add(item.clone());
                }
            }
           

            viewModel = new ViewModel(_awards);
            DataContext = viewModel;

        }

        public class ViewModel: INotifyPropertyChanged
        {
            private ObservableCollection<ObservableCollection<AwardsViewModel>> rows { get; set; } = new ObservableCollection<ObservableCollection<AwardsViewModel>>();
            private ObservableCollection<ObservableCollection<AwardsViewModel>> _rows { get; set; }
            public ObservableCollection<ObservableCollection<AwardsViewModel>> Rows { get {  return _rows; } set { 
                    rows = value;
                    GetReverse();
                    OnPropertyChanged(nameof(Rows));
                } }

            public ViewModel()
            {
        
            }

            public event PropertyChangedEventHandler? PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public ViewModel(ObservableCollection<AwardsModel> award)
            {
                if (award != null)
                {
                    // 使用 Select 把每个 AwardsModel 转换为 AwardsViewModel
                    ObservableCollection<AwardsViewModel> viewModelList = new ObservableCollection<AwardsViewModel>(
                        award.Select(item => new AwardsViewModel { Color = item.Color, Text = item.Name + '\n' + item.Probability })
                    );

                    // 直接设置 Rows 为 viewModelList
                    Rows = new ObservableCollection<ObservableCollection<AwardsViewModel>> { viewModelList };
                    //_rows = new ObservableCollection<ObservableCollection<AwardsViewModel>>(Rows.Reverse());
                }
                else
                {
                    // award 为 null 时，保持默认的空列表状态
                    Rows = new ObservableCollection<ObservableCollection<AwardsViewModel>>();
                }
               
            }

            public void Add(AwardsModel award)
            {
                // 获取 Rows 中的最后一个内部列表
                ObservableCollection<AwardsViewModel> lastInnerList = rows.LastOrDefault();
                //ObservableCollection<AwardsViewModel> lastInnerList1 = Rows.LastOrDefault();

                // 如果 lastInnerList 为 null，创建一个新的内部列表
                if (lastInnerList == null)
                {
                    lastInnerList = new ObservableCollection<AwardsViewModel>();
                    rows.Add(lastInnerList);
                }

                // 向内部列表添加新的 AwardsViewModel
                lastInnerList.Add(new AwardsViewModel { Color = award.Color, Text = award.Name + '\n' + award.Probability });
                //lastInnerList1.Add(new AwardsViewModel { Color = award.Color, Text = award.Name + '\n' + award.Probability });
                GetReverse();
            }

            public void Update(ObservableCollection<AwardsModel> award)
            {
                if (award == null)
                {
                    Rows.Clear();
                    return;
                }

                foreach (var item in award)
                {
                    var existingList = Rows.FirstOrDefault(innerList => innerList.Any(vm => vm.Text == item.Name + '\n' + item.Probability));

                    if (existingList != null)
                    {
                        // 如果列表已存在，则更新现有项
                        var existingItem = existingList.FirstOrDefault(vm => vm.Text == item.Name + '\n' + item.Probability);
                        if (existingItem != null)
                        {
                            existingItem.Color = item.Color;
                        }
                        else
                        {
                            // 如果项不存在，则添加新项
                            existingList.Add(new AwardsViewModel { Color = item.Color, Text = item.Name + '\n' + item.Probability });
                        }
                    }
                    else
                    {
                        // 如果列表不存在，则创建新列表并添加新项
                        Rows.Add(new ObservableCollection<AwardsViewModel> { new AwardsViewModel { Color = item.Color, Text = item.Name + '\n' + item.Probability } });
                    }
                }

                // 删除任何过时的列表或项
                var obsoleteLists = Rows.Where(innerList => !award.Any(model => innerList.Any(vm => vm.Text == model.Name + '\n' + model.Probability))).ToList();
                foreach (var list in obsoleteLists)
                {
                    Rows.Remove(list);
                }
            }

            private void GetReverse()
            {
                if(rows.Count == 0 || rows == null)
                {
                    _rows = new ObservableCollection<ObservableCollection<AwardsViewModel>>();
                }
                else{
                    _rows = new ObservableCollection<ObservableCollection<AwardsViewModel>>(
                        rows.Select(innerList => new ObservableCollection<AwardsViewModel>(innerList.Reverse()))
                    );
                }

                OnPropertyChanged(nameof(Rows));

            }
        }

       

        /// <summary>
        /// 打开弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenAddPopup_Click(object sender, RoutedEventArgs e)
        {
            AddPopup.IsOpen = true;
            isPop = true;
            IsHitTestVisible = false;
        }


        /// <summary>
        /// 弹窗保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            flag = true;

            #region 新增判断
            if (ValidateInput(out int IDResult,out int ProbabilityResult))
            {
                
                if (_awards.Any() && _awards.Any(_ => _.ID == IDResult))
                {
                    flag = false;
                    //重复
                    MessageBox.Show("编号重复捏.");
                    
                }
                
                if(_awards.Any() && (_awards.Sum(_ => _.Probability) + ProbabilityResult) > 360)
                {
                    flag = false;
                    //概率不合法
                    MessageBox.Show("概率和大于360了捏.");
                }

                if (flag)
                {
                    var tempColor = ColorHelper.SolidColorBrush(_awards);
                    _awards.Add(new AwardsModel { ID = IDResult, Color = tempColor, Probability = ProbabilityResult, Name = TextBoxName.Text });
                    viewModel.Add(new AwardsModel { ID = IDResult, Color = tempColor, Probability = ProbabilityResult, Name = TextBoxName.Text });

                    TextClear();
                    AddPopup.IsOpen = false;
                    isPop = false;
                    IsHitTestVisible = true;

                    MessageBox.Show("成功！");
                }
                else
                {
                    MessageBox.Show("请检查输入是否正确！");
                }
                
            }
            else
            {
                MessageBox.Show("请检查输入是否正确！");
            }
            #endregion

            
        }

        /// <summary>
        /// 弹窗取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelAddPopup_Click(object sender, RoutedEventArgs e)
        {
            TextClear();
            AddPopup.IsOpen = false;
            isPop = false;
            IsHitTestVisible = true;
        }

        private void AddPopup_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        /// <summary>
        /// 鼠标左键按下时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Popup_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                isDragging = true;
                lastMousePosition = e.GetPosition(AddPopup);
            }
        }

        /// <summary>
        /// 鼠标左键按下且移动时重画弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Popup_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point newMousePosition = e.GetPosition(AddPopup);
                double deltaX = newMousePosition.X - lastMousePosition.X;
                double deltaY = newMousePosition.Y - lastMousePosition.Y;

                AddPopup.HorizontalOffset += deltaX;
                AddPopup.VerticalOffset += deltaY;

                lastMousePosition = newMousePosition;
            }
        }

        /// <summary>
        /// 鼠标左键弹起时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Popup_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                isDragging = false;
            }
        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                isDragging = true;
                Mouse.Capture((UIElement)sender);
            }
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point newMousePosition = e.GetPosition(AddPopup);
                double deltaX = newMousePosition.X - lastMousePosition.X;
                double deltaY = newMousePosition.Y - lastMousePosition.Y;

                AddPopup.HorizontalOffset += deltaX;
                AddPopup.VerticalOffset += deltaY;

                lastMousePosition = newMousePosition;
            }
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                isDragging = false;
                Mouse.Capture(null);
            }
        }

        private void MainWindow_Deactivated(object sender, EventArgs e)
        {
            if (isPop)
            {
                AddPopup.IsOpen = false;
                IsHitTestVisible = true;
            }
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            if (isPop)
            {
                AddPopup.IsOpen = true;
                IsHitTestVisible = false;
            }
        }

        /// <summary>
        /// 输入框条件判断
        /// </summary>
        /// <returns></returns>
        private bool ValidateInput(out int _,out int __)
        {
            _ = -1;
            __ = -1;
            // 检查所有 TextBox 是否为空
            if (string.IsNullOrEmpty(TextBoxNumber.Text) || string.IsNullOrEmpty(TextBoxProbability.Text) || string.IsNullOrEmpty(TextBoxName.Text))
            {

                return false;
            }

            // 检查 TextBoxNumber 是否可以成功转换为整数
            if (!int.TryParse(TextBoxNumber.Text, out _))
            {
                return false;
            }

            // 检查 TextBoxProbability 是否可以成功转换为整数，并且在 0 到 360 之间
            if (!int.TryParse(TextBoxProbability.Text, out __) || __ <= 0 || __ > 360)
            {
                return false;
            }

            // 所有条件都满足
            return true;
        }

        private void TextClear()
        {
            TextBoxNumber.Text = string.Empty;
            TextBoxNumber.Text = string.Empty;
            TextBoxProbability.Text = string.Empty;
            TextBoxName.Text = string.Empty;
        }

        private void Serialize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_awards.Count > 0)
                {
                    if (_awards.Sum(_ => _.Probability) != 360)
                    {
                        MessageBox.Show("生成转盘请确保所有奖项概率和等于360,现在不等于捏!");
                    }
                    else
                    {
                        App.Awards.Clear();
                        foreach (var item in _awards)
                        {
                            App.Awards.Add(item.clone());
                        }

                        JsonSerializationHelper.SerializeToFile(App.Awards, CommonHelper.AwardsFilePath);
                        MessageBox.Show("生成成功捏！");
                    }
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            
        }
    }
}
