using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfApp1.pojo
{
    class WorkLabelInfo
    {
        public string WorkLabel { get; set; }
        public Boolean Alert { get; set; }
        public string Background { get; set; }
        public string TimeStamp { get; set; }
        public string ImageStr { get; set; }
        public BitmapImage Image { get; set; }
    }
}
