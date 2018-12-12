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
    class ViewModel
    {
        public ICommand func { get; set; }

        public ViewModel()
        {
            func = new DelegateCommand(
                () => {
                    MessageBox.Show("トリガー発火");
                    return;
                },
                () =>
                {
                    return true;
                });
        }
    }
}
