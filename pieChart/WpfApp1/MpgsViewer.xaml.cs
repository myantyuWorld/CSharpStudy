using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
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

namespace WpfApp1
{
    /// <summary>
    /// MpgsViewer.xaml の相互作用ロジック
    /// </summary>
    public partial class MpgsViewer : UserControl
    {
        public MpgsViewer()
        {

            InitializeComponent();

            this.TmpCam1.Values = new ChartValues<ObservableValue> { new ObservableValue(1) };
            this.TmpCam2.Values = new ChartValues<ObservableValue> { new ObservableValue(1) };
            this.TmpCam3.Values = new ChartValues<ObservableValue> { new ObservableValue(1) };
            this.TmpCam4.Values = new ChartValues<ObservableValue> { new ObservableValue(1) };
        }

        public int Cam1MbpsValue
        {
            get => (int)GetValue(Cam1MbpsProperty);
            set {
                Console.WriteLine(" - UserControl DoubleValue1 = {0} ", value);
                SetValue(Cam1MbpsProperty, value);
                this.UpdaterChart();
            }
        }
        public static readonly DependencyProperty Cam1MbpsProperty = DependencyProperty.Register(nameof(Cam1MbpsValue), typeof(int), typeof(MpgsViewer));
        //---------------------------------------------------------------------------
        public int Cam2MbpsValue
        {
            get => (int)GetValue(Cam2MbpsProperty);
            set {
                Console.WriteLine(" - UserControl DoubleValue2 = {0} ", value);
                SetValue(Cam2MbpsProperty, value);
                this.UpdaterChart();
            }
        }
        public static readonly DependencyProperty Cam2MbpsProperty = DependencyProperty.Register(nameof(Cam2MbpsValue), typeof(int), typeof(MpgsViewer));
        //---------------------------------------------------------------------------
        public int Cam3MbpsValue
        {
            get => (int)GetValue(Cam3MbpsProperty);
            set {
                Console.WriteLine(" - UserControl DoubleValue3 = {0} ", value);
                SetValue(Cam3MbpsProperty, value);
                this.UpdaterChart();
            }
        }
        public static readonly DependencyProperty Cam3MbpsProperty = DependencyProperty.Register(nameof(Cam3MbpsValue), typeof(int), typeof(MpgsViewer));

        public int Cam4MbpsValue
        {
            get => (int)GetValue(Cam4MbpsProperty);
            set {
                Console.WriteLine(" - UserControl DoubleValue3 = {0} ", value);
                SetValue(Cam4MbpsProperty, value);
                this.UpdaterChart();
            }
        }
        public static readonly DependencyProperty Cam4MbpsProperty = DependencyProperty.Register(nameof(Cam4MbpsValue), typeof(int), typeof(MpgsViewer));

        public int QueueCount
        {
            get => (int)GetValue(QueueCountProperty);
            set { Console.WriteLine(" - UserControl DoubleValue3 = {0} ", value); SetValue(QueueCountProperty, value); }
        }
        public static readonly DependencyProperty QueueCountProperty = DependencyProperty.Register(nameof(QueueCount), typeof(int), typeof(MpgsViewer));

        public void UpdaterChart()
        {
            this.TmpCam1.Values = new ChartValues<ObservableValue> { new ObservableValue(this.Cam1MbpsValue) };
            this.TmpCam2.Values = new ChartValues<ObservableValue> { new ObservableValue(this.Cam2MbpsValue) };
            this.TmpCam3.Values = new ChartValues<ObservableValue> { new ObservableValue(this.Cam3MbpsValue) };
            this.TmpCam4.Values = new ChartValues<ObservableValue> { new ObservableValue(this.Cam4MbpsValue) };
        }
    }
}
