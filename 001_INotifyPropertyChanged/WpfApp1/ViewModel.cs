using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class ViewModel : BindingBase
    {
        public bool MyValue
        {
            get
            {
                return _myValue;
            }
            set
            {
                if (_myValue != value)
                {
                    _myValue = value;
                    OnPropertyChanged(nameof(MyValue));
                }
            }
        }
        private bool _myValue = false;



    }
}
