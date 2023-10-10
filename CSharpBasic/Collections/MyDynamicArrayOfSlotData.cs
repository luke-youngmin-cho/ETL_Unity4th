using System.Collections;

namespace Collections
{
    internal class MyDynamicArrayOfSlotData : IEnumerable<SlotData>
    {
        // 인덱스 탐색 
        // O(1)
        public SlotData this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new IndexOutOfRangeException();

                return _items[index];
            }
            set
            {
                if (index < 0 || index >= _count)
                    throw new IndexOutOfRangeException();

                _items[index] = value;
            }
        }

        //public int Count
        //{
        //    get
        //    {
        //        return _count;
        //    }
        //}

        public int Count => _count;
        public int Capacity => _items.Length;

        private int _count;
        private const int DEFAULT_SIZE = 1;
        private SlotData[] _items = new SlotData[DEFAULT_SIZE];

        // 아이템 삽입
        // 시간복잡도 : O(N)
        // 평상시에는 아이템을 가장 마지막 인덱스에 추가하면 되지만 , 
        // 최악의 경우는 공간이 모자랄 경우이기 때문에 더큰 배열을 만들어서 
        // 아이템들을 복제해야하므로 자료개수에 비례한 연산이 필요하다.
        //
        // 공간복잡도 : O(N)
        public void Add(SlotData item)
        {
            if (_count >= _items.Length)
            {
                SlotData[] tmp = new SlotData[_count * 2];
                Array.Copy(_items, tmp, _count);
                _items = tmp;
            }

            _items[_count++] = item;
        }

        // 매치조건 탐색
        // O(N)
        // 최악의경우, 아이템을 못찾게되면 처음부터 끝가지 순회해야하므로 자료갯수에 비례한 연산이 필요함.
        public T Find(Predicate<T> match)
        {
            for (int i = 0; i < _count; i++)
            {
                if (match(_items[i]))
                    return _items[i];
            }
            return default;
        }

        public int FindIndex(Predicate<T> match)
        {
            for (int i = 0; i < _count; i++)
            {
                if (match(_items[i]))
                    return i;
            }
            return -1;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < _count; i++)
            {
                // Default 비교연산 (C# 기본제공 비교연산자 쓸 때)
                if (Comparer<T>.Default.Compare(_items[i], item) == 0)
                    return true;

                // IComparable 비교연산.. (내가 비교연산내용을 직접 구현해서 쓸때)
                if (item.CompareTo(_items[i]) == 0)
                    return true;
            }

            return false;
        }

        // 인덱스 삭제 
        // O(N)
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _count)
                throw new IndexOutOfRangeException();

            for (int i = index; i < _count - 1; i++)
            {
                _items[i] = _items[i + 1];
            }
            _count--;
        }

        // 아이템 삭제
        // O(N)
        public bool Remove(T item)
        {
            int index = FindIndex(x => item.CompareTo(x) == 0);

            // 지우려는 대상 못찾으면 false 반환
            if (index < 0)
                return false;

            RemoveAt(index);
            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        // 책읽어주는자
        public struct Enumerator : IEnumerator<T>
        {
            // 현재 페이지 내용 읽기
            public T Current => _data[_index];

            object IEnumerator.Current => _data[_index];

            private MyDynamicArray<T> _data; // 책
            private int _index; // 책의 현재 페이지

            public Enumerator(MyDynamicArray<T> data)
            {
                _data = data;
                _index = -1; // 책 표지 덮은 상태로 시작
            }

            // 책읽을때 필요했던 자원들(리소스) 을 메모리에서 해제하는 내용을 구현하는 부분
            public void Dispose()
            {
            }

            // 다음 페이지로
            public bool MoveNext()
            {
                // 넘길 수 있는 다음장이 존재한다면 다음장으로 넘기고 true 반환
                if (_index < _data._count - 1)
                {
                    _index++;
                    return true;
                }

                return false;
            }

            // 책 덮기
            public void Reset()
            {
                _index = -1;
            }
        }
    }
}
