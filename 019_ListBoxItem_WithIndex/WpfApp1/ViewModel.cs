using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    class ViewModel : BindingBase
    {
        public DelegateCommand VmMyCommand { get; private set; }

        // 横向きリストで、中身のアイテムの表示をDataTemplateでいじる実験
        public DelegateCommand ListBoxButtonCommand4 { get; private set; }
        public ObservableCollection<string> ListBoxData4 { get; private set; } = new ObservableCollection<string>();

        public ViewModel()
        {
            // コマンドの中身 登録
            VmMyCommand = new DelegateCommand(
                () =>
                {
                    // ボタンを押すと、リストにアイテムを追加する。
                    ListBoxData4.Add("ABCDE");
                });
        }
    }
}
