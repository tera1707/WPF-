using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp3
{
    class ViewModel
    {
        public DelegateCommand<string> MyCommand { get; private set; }

        public ViewModel()
        {
            MyCommand = new DelegateCommand<string>(
                (param) =>
                {
                    // ボタンをおしたときに行う処理
                    MessageBox.Show("ボタン：" + param);
                },
                () =>
                {
                    // ボタンが押せるかどうかを決める処理(trueが「押せる」)
                    return true;
                });
        }
    }
}
