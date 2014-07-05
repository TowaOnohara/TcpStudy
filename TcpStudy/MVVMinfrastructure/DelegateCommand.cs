using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MVVMinfrastructure
{
    //━━━━━━━━━━━━━━━━━━━━━━━━━━
    /// <summary>
    /// コマンド共通処理クラス。   
    /// </summary>
    //━━━━━━━━━━━━━━━━━━━━━━━━━━
    public class DelegateCommand : ICommand
	{
        private Action<object> execute;
		private Func<object,bool> canExecute;

        //============================================
        /// <summary>
		/// コマンドのExecuteメソッドで実行する処理を指定して
		/// DelegateCommandのインスタンスを生成します。
		/// </summary>
		/// <param name="execute"></param>
        //============================================
		public DelegateCommand( Action<object> execute ) : this( execute, (dummy)=>true )	// <- オーバーロードされたコンストラクタの呼び出し
		{
		}


        //============================================
        /// <summary>
		/// コマンドのExecuteメソッドで実行する処理CanExecuteメソッドで
		/// 実行する処理を指定してDelegateCommandのインスタンスを作成します。
		/// </summary>
		/// <param name="execute"></param>
		/// <param name="canExecute"></param>
        //============================================
        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute) 
		{
			if (execute == null) 
			{
				throw new ArgumentException("execute");
			}
			if (canExecute == null) 
			{
				throw new ArgumentException("canExecute");
			}

			this.execute = execute;
			this.canExecute = canExecute;
		}

        //============================================
        /// <summary>
		/// コマンドを実行します。
		/// </summary>
        //============================================
        public void Execute(object parameter)
		{
			this.execute(parameter);
		}

        //============================================
        /// <summary>
		/// コマンドが実行可能な状態かどうか問い合わせます。
		/// </summary>
		/// <returns></returns>
        //============================================
        public bool CanExecute(object parameter) 
		{
            return this.canExecute(parameter);
		}

        //============================================
        /// <summary>
		/// CanExecuteの結果に変更があったことを通知するイベントです。
		/// WPFのCommandManagerから発行してもらうように設定しています。
		/// </summary>
        //============================================
        public event EventHandler CanExecuteChanged 
		{
			add     { CommandManager.RequerySuggested += value; }
			remove  { CommandManager.RequerySuggested -= value; }
		}

        //============================================
        /// <summary>
        /// ICommand.CanExecuteの明示的な実装。CanExecuteメソッドを
        /// コールする。
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
        //============================================
        bool ICommand.CanExecute(object parameter) 
		{
            return this.CanExecute(parameter);
		}

        //============================================
        /// <summary>
		/// ICommand.Executeの明示的な実装。Executeメソッドを
		/// コールする。
		/// </summary>
		/// <param name="parameter"></param>
        //============================================
        void ICommand.Execute(object parameter) 
		{
            this.Execute(parameter);
		}
	}
}
