using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Attributes
{
    internal class LegoStore : INotifyPropertyChanged
    {
        public int StarwarsTotal
        {
            get => _startwarsTotal;
            set
            {
                _startwarsTotal = value;
                OnPropertyChanged();
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StarwarsTotal)));
            }
        }
        private int _startwarsTotal;

        public int CityTotal
        {
            get => _cityTotal;
            set
            {
                _cityTotal = value;
                OnPropertyChanged();
            }
        }
        private int _cityTotal;


        public int CreatorTotal
        {
            get => _creatorTotal;
            set
            {
                _creatorTotal = value;
                OnPropertyChanged();
            }
        }
        private int _creatorTotal;

        // 프로퍼티가 바뀐 사건을 처리하는 대리자
        public event PropertyChangedEventHandler? PropertyChanged;


        public void OnPropertyChanged([CallerMemberName] string propertyName = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
