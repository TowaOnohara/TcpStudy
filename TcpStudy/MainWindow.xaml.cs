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
using MVVMinfrastructure;
using System.Diagnostics;

using System.Threading;

using System.Net;
using System.Net.Sockets;
using System.IO;


namespace TcpStudy
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // 起動モードの取得
            if (null != Environment.GetCommandLineArgs().FirstOrDefault(s => { return (s.Substring(0, 3) == "udp"); }))
            {
                this.DataContext = new UdpViewModel();
            }
            else
            {
                this.DataContext = new MainViewMode();
            }
        }
    }


    public class MainViewMode : ViewModelBase 
    {
		private TcpListener listener = null;
        private Thread ServerThread = null;
        public int LocalPort { get; set; }
        public string LocalIP { get; set; }
        private string _recvText;
        public string Recvtext 
        {
            get { return _recvText; }
            set 
            {
                if (_recvText == value) { return; }
                _recvText = value;
                OnPropertyChanged("Recvtext");
            }
        }

        private string _clientInfo;
        public string ClientInfo 
        {
            get { return _clientInfo; }
            set 
            {
                if (_clientInfo == value) { return; }
                _clientInfo = value;
                OnPropertyChanged("ClientInfo");
            }
        }
        private ICommand _startServerCommand = null;
        public ICommand StartServerCommand
        {
            get 
            {
                if (_startServerCommand != null) { return _startServerCommand; }
                _startServerCommand = new DelegateCommand(this.StartServer, m => { return (listener == null); });
                return _startServerCommand;
            }
        }

        private ICommand _stopServerCommand = null;
        public ICommand StopServerCommand
        {
            get 
            {
                if (_stopServerCommand != null) { return _stopServerCommand; }
                _stopServerCommand = new DelegateCommand(this.StopServer, m => { return (listener != null); });
                return _stopServerCommand;
            }
        }

        private void StopServer(object obj)
        {
            if (listener != null)
            {
                listener.Stop();
                listener = null;
            }
            if (ServerThread != null) 
            {
                ServerThread.Abort();
                ServerThread = null;
            }
        }

        public MainViewMode() 
        {
            LocalPort = 9999;
            LocalIP = "127.0.0.1";
        }


        private void StartServer(object obj)
        {
            // 待機処理開始
            listener = new TcpListener(IPAddress.Parse(LocalIP), LocalPort);    // 自分のIP、自分のPort
            listener.Start();

            // サーバスレッドで待機する。
            ServerThread = new Thread(new ParameterizedThreadStart(this.ServerListener));
            ServerThread.Start();
        }

        private void ServerListener(object obj)
        {
            try
            {
                while (true)
                {
                    TcpClient server = listener.AcceptTcpClient();
                    Trace.WriteLine("accept");
                    ClientInfo = string.Format("Connect to [{0}:{1}]",
                        ((System.Net.IPEndPoint)server.Client.RemoteEndPoint).Address.ToString(),
                        ((System.Net.IPEndPoint)server.Client.RemoteEndPoint).Port.ToString());

                    NetworkStream sterm = server.GetStream();
                    StreamReader reader = new StreamReader(sterm);
                    Trace.WriteLine("wait...");

                    while (true)
                    {
                        string sr = reader.ReadLine();
                        if (sr == "__CMD_ENDCONNECTION__") { break; }
                        Recvtext += (DateTime.Now.ToString("[HH:mm:ss] ") + sr);
                        Recvtext += "\n";
                        Trace.WriteLine("get string:" + sr);
                    }

                    ClientInfo = "";
                }
            }
            catch (Exception ex) 
            {
                Trace.WriteLine(this.ToString() + " : " + ex.Message);
                ClientInfo = "";
            }

        }
    }


}
