using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TurntableHelper.Model
{
    [Serializable]
    public class AwardsModel
    {
        public int ID { get; set; }
        public Brush Color { get; set; }
        public int Probability { get; set; }
        public string Name { get; set; }

        public AwardsModel clone()
        {
            return new AwardsModel { ID = ID, Color = Color, Probability = Probability, Name = Name };
        }
    }
}
