using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turntable.Helper
{
    public static class CommonHelper
    {
        //string programPath = AppDomain.CurrentDomain.BaseDirectory;
        //int levelsToGoUp = 5;
        //string parentDirectory = Enumerable.Range(0, levelsToGoUp)
        //                                   .Aggregate(programPath, (current, _) => Path.GetDirectoryName(current));
        public static string AwardsFilePath = Path.Combine(Enumerable.Range(0, 5)
                                           .Aggregate(AppDomain.CurrentDomain.BaseDirectory, (current, _) => Path.GetDirectoryName(current)), "File", "Awards.json");
    }
}
