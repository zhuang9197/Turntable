using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TurntableHelper.Helper;

namespace TurntableHelper.Model
{
    /// <summary>
    /// 全局类，以单例模式呈现
    /// </summary>
    [Serializable]
    public class AwardsModel
    {
        
        public int ID { get; set; } //编号
        public Brush Color { get; set; }    //颜色
        public int Probability { get; set; }    //概率
        public string Name { get; set; }    //名称

        /// <summary>
        /// 单例
        /// </summary>
        private static ObservableCollection<AwardsModel> SigngleAward;

        /// <summary>
        /// 私有化构造函数
        /// </summary>
        private AwardsModel()
        {

        }

        /// <summary>
        /// 定义实例化方法，只允许一次调用
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void Initialize()
        {
            if(SigngleAward == null)
            {
                SigngleAward =  JsonSerializationHelper.DeserializeFromFile<ObservableCollection<AwardsModel>>(StaticCommonHelper.AwardsFilePath);
            }
            else
            {
                throw new Exception("Repeated instantiation");
            }
        }

        /// <summary>
        /// 单例
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<AwardsModel> Instance()
        {
            {
                if (SigngleAward == null)
                {
                    return null;
                }

                return SigngleAward;
            }
        }


        

        /// <summary>
        /// 定义克隆方法，后续考虑深拷贝问题
        /// </summary>
        /// <returns></returns>
        public AwardsModel clone()
        {
            return new AwardsModel { ID = ID, Color = Color, Probability = Probability, Name = Name };
        }

        /// <summary>
        /// 放出单个的new方法，用于add操作
        /// </summary>
        /// <param name="id"></param>
        /// <param name="color"></param>
        /// <param name="probability"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static AwardsModel GetModel(int id, Brush color,int probability,string name)
        {
            return new AwardsModel { ID = id, Color = color, Probability = probability, Name = name };
        }
    }
}
