using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interactivity;
using System.Windows;

namespace MVVMinfrastructure
{
    //━━━━━━━━━━━━━━━━━━━━━
    /// <summary> 
    /// ViewModelとViewの間での情報の
    /// やり取りを行うメッセージ 
    /// </summary> 
    //━━━━━━━━━━━━━━━━━━━━━
    public class Message
    {
        //--------------------------------------------
        /// <summary> メッセージの本体  </summary> 
        //--------------------------------------------
        public object Body { get; private set; }

        //--------------------------------------------
        /// <summary> ViewからViewModelへのメッセージのレスポンス</summary> 
        //--------------------------------------------
        public object Response { get; set; }

        //=====================================
        /// <summary> 
        /// コンストラクタ
        /// Bodyを指定してMessageを作成する 
        /// </summary> 
        /// <param name="body"></param> 
        //=====================================
        public Message(object body)
        {
            this.Body = body;
        }
    }

    //━━━━━━━━━━━━━━━━━━━━━
    /// <summary> 
    /// Messageを送信するクラス。 
    /// </summary> 
    //━━━━━━━━━━━━━━━━━━━━━
    public class Messenger
    {
        //--------------------------------------------
        /// <summary> メッセージが送信されたことを通知するイベント </summary>
        //--------------------------------------------
        public event EventHandler<MessageEventArgs> Raised;

        //=====================================
        /// <summary> 
        /// 指定したメッセージとコールバックでメッセージを送信する 
        /// </summary> 
        /// <param name="message">メッセージ</param> 
        /// <param name="callback">コールバック</param> 
        //=====================================
        public void Raise(Message message, Action<Message> callback)
        {
            var h = this.Raised;
            if (h != null)
            {
                h(this, new MessageEventArgs(message, callback));
            }
        }
    }

    //━━━━━━━━━━━━━━━━━━━━━
    /// <summary> 
    /// Messengerの通知イベント用のイベント引数 
    /// </summary> 
    //━━━━━━━━━━━━━━━━━━━━━
    public class MessageEventArgs : EventArgs
    {
        //--------------------------------------------
        /// <summary> 送信するメッセージ</summary> 
        //--------------------------------------------
        public Message Message { get; private set; }

        //--------------------------------------------
        /// <summary> ViewModelのコールバック </summary> 
        //--------------------------------------------
        public Action<Message> Callback { get; private set; }

        //=====================================
        /// <summary> 
        /// コンストラクタ
        /// メッセージとコールバックを指定してイベント引数を作成する 
        /// </summary> 
        /// <param name="message">メッセージ</param> 
        /// <param name="callback">コールバック</param> 
        //=====================================
        public MessageEventArgs(Message message, Action<Message> callback)
        {
            this.Message = message;
            this.Callback = callback;
        }
    }
}
