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
        private int ctr = 0;

        // 標準リストボックス実験
        public DelegateCommand ListBoxButtonCommand1 { get; private set; }
        public ObservableCollection<string> ListBoxData1 { get; private set; } = new ObservableCollection<string>();

        // 横向きリストボックス実験
        public DelegateCommand ListBoxButtonCommand2 { get; private set; }
        public ObservableCollection<string> ListBoxData2 { get; private set; } = new ObservableCollection<string>();

        // 横向きリストで、中身のアイテムの表示をItemContainerStyleでいじる実験
        public DelegateCommand ListBoxButtonCommand3 { get; private set; }
        public ObservableCollection<string> ListBoxData3 { get; private set; } = new ObservableCollection<string>();

        // 横向きリストで、中身のアイテムの表示をDataTemplateでいじる実験
        public DelegateCommand ListBoxButtonCommand4 { get; private set; }
        public ObservableCollection<string> ListBoxData4 { get; private set; } = new ObservableCollection<string>();

        public ViewModel()
        {
            ListBoxButtonCommand1 = new DelegateCommand( () =>
            {
                ListBoxData1.Add((ctr++).ToString());
            });

            ListBoxButtonCommand2 = new DelegateCommand(() =>
            {
                ListBoxData2.Add((ctr++).ToString());
            });

            ListBoxButtonCommand3 = new DelegateCommand(() =>
            {
                ListBoxData3.Add((ctr++).ToString());
            });

            ListBoxButtonCommand4 = new DelegateCommand(() =>
            {
                ListBoxData4.Add((ctr++).ToString());
            });
        }
    }
}
