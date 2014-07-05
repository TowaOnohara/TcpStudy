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

using System.Threading;

using System.Net;
using System.Net.Sockets;
using System.IO;


namespace TcpStudy_Client
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewMode();
        }
    }


    public class MainViewMode : ViewModelBase
    {
        private TcpClient client = null;
        private StreamWriter writer = null;
        public int port { get; set; }
        public string TargetIP { get; set; }
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
                _connectCommand = new DelegateCommand(this.Connect, m => { return (this.writer == null); });
                return _connectCommand;
            }
        }

        private ICommand _sendCommand = null;
        public ICommand SendCommand
        {
            get
            {
                if (_sendCommand != null) { return _sendCommand; }
                _sendCommand = new DelegateCommand(this.Send, m => { return (this.writer != null); });
                return _sendCommand;
            }
        }

        private ICommand _disConnectCommand = null;
        public ICommand DisConnectCommand
        {
            get
            {
                if (_disConnectCommand != null) { return _disConnectCommand; }
                _disConnectCommand = new DelegateCommand(this.DisConnect, m => { return (this.writer != null); });
                return _disConnectCommand;
            }
        }

        private void DisConnect(object obj)
        {
            if (writer != null) 
            {
                writer.WriteLine("__CMD_ENDCONNECTION__");
                writer.Flush();
                writer.BaseStream.Close();
                writer.Close();
                writer = null;
            }
            if (client != null) 
            {
                client.Close();
                client = null;
            }
        }

        private void Send(object obj)
        {
            this.writer.WriteLine(Sendtext);
            this.writer.Flush();                    // これが必要
            Sendedtext += (DateTime.Now.ToString("[HH:mm:ss] ") + Sendtext);
            Sendedtext += "\n";
            Sendtext = "";
        }

        public MainViewMode()
        {
            port = 9999;                // Serverが待機しているIP
            TargetIP = "127.0.0.1";     // ServerのIPアドレス
        }


        private void Connect(object obj)
        {
            try
            {
                client = new TcpClient(this.TargetIP, this.port);
                writer = new StreamWriter(client.GetStream());
            }
            catch (Exception e) 
            {
                
            }
        }

        
    }
}
