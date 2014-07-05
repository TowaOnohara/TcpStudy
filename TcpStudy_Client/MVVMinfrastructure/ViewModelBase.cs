using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MVVMinfrastructure
{
    //━━━━━━━━━━━━━━━━━━━━━━━
	/// <summary>
	/// ViewModelの基本クラス。
    /// ViewModelクラスで共通で実装するインターフェースINotifyPropertyChangedの実装や、
    /// 必要となるメソッドを提供します。
	/// </summary>
    //━━━━━━━━━━━━━━━━━━━━━━━
    public class ViewModelBase : INotifyPropertyChanged, IDataErrorInfo
    {

        #region INotifyPropertyChanged メンバ
        //--------------------------------------

        //=============================================
        /// <summary>
		/// プロパティに変更があったときに発行される。
		/// </summary>
        //=============================================
        public event PropertyChangedEventHandler PropertyChanged;

        //=============================================
        /// <summary>
		/// PropertyChangedイベントの発行
		/// </summary>
		/// <param name="propertyName"></param>
        //=============================================
        protected virtual void OnPropertyChanged(string propertyName) 
		{
			var h = this.PropertyChanged;
			if (h != null) 
			{
				h(this, new PropertyChangedEventArgs(propertyName));
			}
		}

        //--------------------------------------
        # endregion


        #region IDataErrorInfo メンバ
        //--------------------------------------

        //=============================================
        /// <summary>
		/// IDataErrorInfoのメンバ
		/// </summary>
        //=============================================
        public string Error
		{
			get { return String.Empty; }
		}

        //=============================================
        /// <summary>
		/// columNameで指定したプロパティのエラーを返します。
		/// nullを返す場合はエラーなしとなります。
		/// </summary>
		/// <param name="columName"></param>
		/// <returns></returns>
        //=============================================
        public string this[string columName]
		{
			get
			{
				if(this.errors.ContainsKey(columName))
				{
					return this.errors[columName];
				}
				return null;
			}
		}

        //--------------------------------------
        # endregion



        //=============================================
        /// <summary>
        ///  プロパティに紐付いたエラーメッセージを格納します。
        /// </summary>
        //=============================================
        private Dictionary<string, string> errors = new Dictionary<string, string>();


        //=============================================
        /// <summary>
		/// プロパティにエラーメッセージを設定します。
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="errorMessage"></param>
        //=============================================
        protected void SetError(string propertyName, string errorMessage)
		{
			this.errors[propertyName] = errorMessage;
            this.OnPropertyChanged("IsError");
		}

        //=============================================
        /// <summary>
        /// プロパティにエラーメッセージを設定します。
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="errorMessage"></param>
        //=============================================
        public void SetErrorByExternal(string propertyName, string errorMessage)
        {
            this.errors[propertyName] = errorMessage;
            this.OnPropertyChanged("IsError");
        }
        //=============================================
        /// <summary>
		/// プロパティのエラーをクリアします。
		/// </summary>
		/// <param name="propertyName"></param>
        //=============================================
        protected void ClearError(string propertyName)
		{
			if(this.errors.ContainsKey(propertyName))
			{
				this.errors.Remove(propertyName);
                this.OnPropertyChanged("IsError");
			}
		}
        //=============================================
        /// <summary>
        /// プロパティのエラーをクリアします。
        /// </summary>
        /// <param name="propertyName"></param>
        //=============================================
        public void ClearErrorByExternal(string propertyName)
        {
            if (this.errors.ContainsKey(propertyName))
            {
                this.errors.Remove(propertyName);
                this.OnPropertyChanged("IsError");
            }
        }
        //=============================================
        /// <summary>
		/// すべてのエラーをクリアします。
		/// </summary>
        //=============================================
        protected void ClearErrors()
		{
			this.errors.Clear();
            this.OnPropertyChanged("IsError");
		}

        //=============================================
        /// <summary>
		/// エラーの有無を取得します。
		/// </summary>
        //=============================================
        public bool IsError
		{
			get
			{
				return this.errors.Count != 0;
			}
		}

        //============================================
        /// <summary> 
        /// 終了コマンドが実行されたことを通知するメッセージを送信する
        /// メッセンジャー
        /// </summary> 
        //============================================
        private Messenger _msgBoxMessenger = new Messenger();
        public Messenger MsgBoxMessenger
        {
            get
            {
                return this._msgBoxMessenger;
            }
        }
    }
}
