using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using WpfApp1.pojo;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        Controller controller;
        List<WorkLabelInfo> items;
        public MainWindow()
        {
            InitializeComponent();

            items = new List<WorkLabelInfo>();
            BindingOperations.EnableCollectionSynchronization(this.items, new object());

            controller = new Controller();
            controller.CameraViewEvent += Controller_CameraViewEvent;// CameraMbps event
            controller.WorkLabelEvent += Controller_WorkLabelEvent;// WorkLabel event

            controller.Start(); // cameraNum, Mbpsをキューにためるスレッド
            controller.DrawMbpsViewer(); // キューから取り出し、描画を行うスレッド
            controller.DrawWorkLabelInfo();
        }

        /// <summary>
        /// WorkLabelInfoリストボックス描画
        /// </summary>
        private void Controller_WorkLabelEvent(WorkLabelInfo workLabelInfo)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                // base64 -> BitMapImage
                // ※ UI更新のスレッドと同じスレッドで操作する必要があるため、ここで更新する
                byte[] binaryData = Convert.FromBase64String(workLabelInfo.ImageStr);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = new MemoryStream(binaryData);
                bi.EndInit();
                workLabelInfo.Image = bi;

                this.lbTodoList.Items.Insert(0, workLabelInfo);
                this.lbWorkLabelList.Items.Insert(0, workLabelInfo);
            }));
        }

        /// <summary>
        /// CameraMbps プロパティを変更するイベント
        /// ※ プロパティ変更時、setterで、pieChart再描画処理を走らせる
        /// </summary>
        /// <param name="cameraMbps"></param>
        private void Controller_CameraViewEvent(CameraMbps cameraMbps)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                this.mbpsViewer.QueueCount = this.controller.CameraQueue.Count();
                switch(cameraMbps.CamNum)
                {
                    case 1:
                        this.mbpsViewer.Cam1MbpsValue = cameraMbps.Mbps;
                        break;
                    case 2:
                        this.mbpsViewer.Cam2MbpsValue = cameraMbps.Mbps;
                        break;
                    case 3:
                        this.mbpsViewer.Cam3MbpsValue = cameraMbps.Mbps;
                        break;
                    case 4:
                        this.mbpsViewer.Cam4MbpsValue = cameraMbps.Mbps;
                        break;
                }
            }));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
