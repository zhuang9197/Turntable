using System.Windows;
using TurntableHelper.Model;

namespace Turntable
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //public static ObservableCollection<AwardsModel> Awards {  get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //string programPath = AppDomain.CurrentDomain.BaseDirectory;
            //int levelsToGoUp = 5;
            //string parentDirectory = Enumerable.Range(0, levelsToGoUp)
            //                                   .Aggregate(programPath, (current, _) => Path.GetDirectoryName(current));
            //string awardsFilePath = Path.Combine(parentDirectory, "File", "Awards.json");


            //if( Awards == null)
            //{
            //    Awards = new ObservableCollection<AwardsModel>();
            //}

            //实例化全局类
            AwardsModel.Initialize();
        }
    }
}
