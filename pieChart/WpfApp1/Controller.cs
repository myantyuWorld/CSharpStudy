using NetMQ;
using NetMQ.Sockets;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpfApp1.pojo;
using System.Text.Json;
using System.Collections.Concurrent;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    class Controller
    {
        public delegate void CameraViewerDrawEvent(CameraMbps cameraMbps);
        public event CameraViewerDrawEvent CameraViewEvent;

        public delegate void WorkLabelInfoDrawEvent(WorkLabelInfo workLabelInfo);
        public event WorkLabelInfoDrawEvent WorkLabelEvent;

        public SubscriberSocket subscriber;
        public SubscriberSocket subscriberWorkLabel;

        public ConcurrentQueue<CameraMbps> CameraQueue;
        public ConcurrentQueue<WorkLabelInfo> WorkLabelInfoQueue;

        public Controller()
        {
            // zmq subscribe(購読開始)
            subscriber = new SubscriberSocket();
            subscriber.Connect("tcp://127.0.0.1:12345");
            subscriber.Subscribe("A");

            subscriberWorkLabel = new SubscriberSocket();
            subscriberWorkLabel.Connect("tcp://127.0.0.1:12000");
            subscriberWorkLabel.Subscribe("B");

            this.CameraQueue = new ConcurrentQueue<CameraMbps>();
            this.WorkLabelInfoQueue = new ConcurrentQueue<WorkLabelInfo>();
        }

        public void Start()
        {
            Debug.WriteLine("controller#Start!");
            // 別スレッドで処理開始
            Task.Run(() =>
            {
                while (true)
                {
                    var topic = subscriber.ReceiveFrameString();
                    var msg = subscriber.ReceiveFrameString();
                    var _mbps = JsonSerializer.Deserialize<CameraMbps>(msg);

                    this.CameraQueue.Enqueue(_mbps);

                    var topic2 = subscriberWorkLabel.ReceiveFrameString();
                    var msg2 = subscriberWorkLabel.ReceiveFrameString();
                    var _workLabelInfo = JsonSerializer.Deserialize<WorkLabelInfo>(msg2);

                    this.WorkLabelInfoQueue.Enqueue(_workLabelInfo);

                    Debug.WriteLine("From Publisher: {0} {1}" , topic, msg);
                    Debug.WriteLine("From Publisher: {0} {1}", topic2, msg2);
                    Thread.Sleep(100);
                }
            });
        }

        /// <summary>
        /// ZMQ[CameraMbps]の情報を取り出し、Mbpsビューワーを描画する
        /// </summary>
        public void DrawMbpsViewer()
        {
            Debug.WriteLine("controller#DrawMbpsViewer!");
            // 別スレッドで処理開始
            Task.Run(() =>
            {
                while (true)
                {
                    if (this.CameraQueue.TryDequeue(out CameraMbps _item))
                    {
                        // MainWindows#イベントをキックする
                        this.CameraViewEvent(cameraMbps: _item);
                    }
                    else
                    {
                        Debug.WriteLine("[NOTICE] nothing queue data...");
                    }
                    Thread.Sleep(1000);
                }
            });
        }

        /// <summary>
        /// ZMQ[workLabel]の情報を取り出し、Mbpsビューワーを描画する
        /// </summary>
        public void DrawWorkLabelInfo()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (this.WorkLabelInfoQueue.TryDequeue(out WorkLabelInfo _item))
                    {
                        // MainWindows#イベントをキックする
                        _item.Background = _item.Alert ? "Red" : "";

                        this.WorkLabelEvent(workLabelInfo: _item);
                    }
                    else
                    {
                        Debug.WriteLine("[NOTICE] nothing workinfo queue data...");
                    }
                    Thread.Sleep(100);
                }
            });
        }


    }
}
