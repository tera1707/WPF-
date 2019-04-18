namespace WpfApp1
{
    class ViewModel : BindingBase
    {
        public string VmMyText
        {
            get { return _vmMyText; }
            set { _vmMyText = value; OnPropertyChanged(nameof(VmMyText)); }
        }
        public string _vmMyText = "あいうえお";

        public DelegateCommand VmMyCommand { get; private set; }

        public ViewModel()
        {
            // コマンドの中身 登録
            VmMyCommand = new DelegateCommand(
                () =>
                {
                    VmMyText = "１２３４５";
                },
                () =>
                {
                    // ボタンが押せるかどうかを決める処理(trueが「押せる」)
                    return true;
                });
        }
    }
}
