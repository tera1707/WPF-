using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace WpfApp1
{
    // 下記の参照の追加が必要。
    // System.Windows.Interactivity
    // Microsoft.Expression.Interactions
    // ※Ver4.5を使用すること
    public class AlertAction : TriggerAction<Grid>
    {
        #region Messageプロパティ
        public ICommand MyCommand
        {
            get { return (ICommand)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("MyCommand", typeof(ICommand), typeof(AlertAction), new UIPropertyMetadata(null));
        #endregion


        public AlertAction()
        {
        }

        // Actionが実行されたときの処理
        protected override void Invoke(object o)
        {
            if (MyCommand == null)
            {
                return;
            }

            MyCommand.Execute(o);
        }
    }
}
