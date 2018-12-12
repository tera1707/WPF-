using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp2
{
    class ViewModel
    {
        public DelegateCommand MyCommand { get; private set; }

        public ViewModel()
        {            
            MyCommand = new DelegateCommand(
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
