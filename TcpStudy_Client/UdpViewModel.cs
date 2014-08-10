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

namespace TcpStudy_Client
{
    public class UdpViewModel : ViewModelBase
    {
        private UdpClient client = null;
        public int ServerPort { get; set; }
        public string ServerIP { get; set; }
        System.Text.Encoding enc = System.Text.Encoding.UTF8; //文字コードを指定する
        private string _sendedText;
        public string Sendedtext
        {
            get { return _sendedText; }
            set
            {
                if (_sendedText == value) { return; }
                _sendedText = value;
                OnPropertyChanged("Sendedtext");
            }
        }
        private string _sendText;
        public string Sendtext
        {
            get { return _sendText; }
            set
            {
                if (_sendText == value) { return; }
                _sendText = value;
                OnPropertyChanged("Sendtext");
            }
        }

        private ICommand _connectCommand = null;
        public ICommand ConnectCommand
        {
            get
            {
                if (_connectCommand != null) { return _connectCommand; }
                _connectCommand = new DelegateCommand(m => { return; }, m => { return (false); });  // Connectはないので常に無効
                return _connectCommand;
            }
        }

        private ICommand _sendCommand = null;
        public ICommand SendCommand
        {
            get
            {
                if (_sendCommand != null) { return _sendCommand; }
                _sendCommand = new DelegateCommand(this.Send, m => { return ((this.ServerPort != 0) && this.ServerIP != ""); });
                return _sendCommand;
            }
        }

        private ICommand _disConnectCommand = null;
        public ICommand DisConnectCommand
        {
            get
            {
                if (_disConnectCommand != null) { return _disConnectCommand; }
                _disConnectCommand = new DelegateCommand(m => { return; }, m => { return (false); });    // Connectがないので常に無効
                return _disConnectCommand;
            }
        }


        private void Send(object obj)
        {
            byte[] sendBytes = enc.GetBytes(Sendtext);
            //リモートホストを指定してデータを送信する
            client.Send(sendBytes, sendBytes.Length, ServerIP, ServerPort);
            Sendedtext += (DateTime.Now.ToString("[HH:mm:ss] ") + Sendtext);
            Sendedtext += "\n";
            Sendtext = "";
        }

        public UdpViewModel()
        {
            ServerPort = 9999;                  // Serverが待機しているIP
            ServerIP = "127.0.0.1";             // ServerのIPアドレス

            //ローカルポート番号にバインドする
            client = new UdpClient(8808);
        }




    }
}
