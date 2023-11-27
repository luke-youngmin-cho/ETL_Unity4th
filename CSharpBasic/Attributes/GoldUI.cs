namespace Attributes
{
    internal class GoldUI
    {
        [Bind("Value", SourceTag.Gold)]
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                _text = _value.ToString();
            }
        }
        private int _value;

        [Bind("Diamonds", SourceTag.Gold)]
        public int Diamonds
        {
            get; set;
        }

        public string text => _text;
        private string _text;
        private Binder<GoldUI> _binder;

        public GoldUI()
        {
            _binder = new Binder<GoldUI>(this, GoldViewModel.Instance, SourceTag.Gold);
        }
    }
}
