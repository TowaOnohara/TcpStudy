using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMinfrastructure;
using System.Threading;
using System.Diagnostics;
using System.Windows.Input;

using System.Net;
using System.Net.Sockets;
using System.IO;

namespace TcpStudy
{
    public class UdpViewModel : ViewModelBase
    {
        private Thread ServerThread = null;
        public int LocalPort { get; set; }
        public string LocalIP { get; set; }
        System.Text.Encoding enc = System.Text.Encoding.UTF8; //文字コードを指定する
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
                _startServerCommand = new DelegateCommand(this.StartServer, m => { return (ServerThread == null); });
                return _startServerCommand;
            }
        }

        private ICommand _stopServerCommand = null;
        public ICommand StopServerCommand
        {
            get 
            {
                if (_stopServerCommand != null) { return _stopServerCommand; }
                _stopServerCommand = new DelegateCommand(this.StopServer, m => { return (ServerThread != null); });
                return _stopServerCommand;
            }
        }

        private void StopServer(object obj)
        {
            if (ServerThread != null) 
            {
                ServerThread.Abort();
                ServerThread = null;
            }
        }

        public UdpViewModel() 
        {
            LocalPort = 9999;
            LocalIP = "";           // 使わない
        }


        private void StartServer(object obj)
        {
            // サーバスレッドで待機する。
            ServerThread = new Thread(new ParameterizedThreadStart(this.ServerListener));
            ServerThread.Start();
        }

        private void ServerListener(object obj)
        {
            try
            {
                // 待機処理開始
                UdpClient server = new UdpClient(LocalPort);  // 自分のポート番号

                while (true)
                {
                    System.Net.IPEndPoint remoteEP = null;
                    byte[] rcvBytes = server.Receive(ref remoteEP);
                    string sr = enc.GetString(rcvBytes);
                    Recvtext += (DateTime.Now.ToString("[HH:mm:ss] ") + sr);
                    Recvtext += "\n";
                    Trace.WriteLine("get string:" + sr);

                    ClientInfo = string.Format("Connect to [{0}:{1}]",
                       remoteEP.Address.ToString(),
                       remoteEP.Port.ToString());
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
