using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    class ViewModel
    {
        public string VmMyText { get; set; } = "あいうえお";

        public DelegateCommand VmMyCommand { get; private set; }

        public ViewModel()
        {
            // コマンドの中身 登録
            VmMyCommand = new DelegateCommand(
                () =>
                {
                    // ボタンをおしたときに行う処理
                    MessageBox.Show("aaaa");
                },
                () =>
                {
                    // ボタンが押せるかどうかを決める処理(trueが「押せる」)
                    return true;
                });
        }
    }
}
