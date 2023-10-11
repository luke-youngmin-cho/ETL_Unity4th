using System.Collections;

namespace Collections
{
    internal class MyLinkedListNode<T>
    {
        public T? Value;
        public MyLinkedListNode<T>? Prev;
        public MyLinkedListNode<T>? Next;

        public MyLinkedListNode(T value)
        {
            Value = value;
        }
    }

    internal class MyLinkedList<T> : IEnumerable<T>
        where T : IEquatable<T>
    {
        public MyLinkedListNode<T>? First => _first;
        public MyLinkedListNode<T>? Last => _last;
        public int Count => _count;

        private MyLinkedListNode<T>? _first, _last, _tmp;
        private int _count;

        /// <summary>
        /// 가장 앞에 삽입
        /// </summary>
        public void AddFirst(T value)
        {
            _tmp = new MyLinkedListNode<T>(value);
            // 하나이상의 노드가 존재한다면 (기존에 First 가 있다면)
            if (_first != null)
            {
                _tmp.Next = _first; //방금 만든 노드의 다음노드가 기존 First 노드가 되야함.
                _first.Prev = _tmp;
            }
            else
            {
                _last = _tmp;
            }

            _first = _tmp;
            _count++;
        }

        /// <summary>
        /// 가장 뒤에 삽입
        /// </summary>
        public void AddLast(T value)
        {
            _tmp = new MyLinkedListNode<T>(value);

            if (_last != null)
            {
                _tmp.Prev = _last;
                _last.Next = _tmp;
            }
            else
            {
                _first = _tmp;
            }

            _last = _tmp;
            _count++;
        }

        /// <summary>
        /// 특정 노드 앞에 삽입
        /// </summary>
        /// <param name="node"> 기준 노드 </param>
        public void AddBefore(MyLinkedListNode<T> node, T value)
        {
            _tmp = new MyLinkedListNode<T>(value);

            // 기준노드 이전에 다른 노드가 있다면
            if (node.Prev != null)
            {
                node.Prev.Next = _tmp;
                _tmp.Prev = node.Prev;
            }
            // 기준노드 이전에 다른노드가 없으면 내가 추가하려는 위치가 First 자리임.
            else
            {
                _first = _tmp;
            }

            node.Prev = _tmp;
            _tmp.Next = node;
            _count++;
        }

        public void AddAfter(MyLinkedListNode<T> node, T value)
        {
            _tmp = new MyLinkedListNode<T>(value);

            // 기준노드 이전에 다른 노드가 있다면
            if (node.Next != null)
            {
                node.Next.Prev = _tmp;
                _tmp.Next = node.Next;
            }
            // 기준노드 다음에 다른노드가 없으면 내가 추가하려는 위치가 Last 자리임.
            else
            {
                _last = _tmp;
            }

            node.Next = _tmp;
            _tmp.Prev = node;
            _count++;
        }

        /// <summary>
        /// First 부터 match 조건에 맞는 노드를 찾을때까지 Next 탐색
        /// </summary>
        public MyLinkedListNode<T> Find(Predicate<T> match)
        {
            _tmp = _first;
            while (_tmp != null)
            {
                if (match(_tmp.Value))
                    return _tmp;

                _tmp = _tmp.Next;
            }

            return null;
        }

        public MyLinkedListNode<T> Find(int index)
        {
            _tmp = _first;
            int _tmpCnt = 0;
            while (_tmp != null)
            {
                if (_tmpCnt == index)
                    return _tmp;

                _tmp = _tmp.Next;
                _tmpCnt++;
            }

            return null;
        }

        public MyLinkedListNode<T> FindLast(Predicate<T> match)
        {
            _tmp = _last;
            while (_tmp != null)
            {
                if (match(_tmp.Value))
                    return _tmp;

                _tmp = _tmp.Prev;
            }

            return null;
        }

        public bool Remove(MyLinkedListNode<T> node)
        {
            if (node == null)
                return false;

            if (node.Prev != null)
            {
                node.Prev.Next = node.Next;
            }
            else
            {
                _first = node.Next;
            }

            if (node.Next != null)
            {
                node.Next.Prev = node.Prev;
            }
            else
            {
                _last = node.Prev;
            }
            _count--;
            return true;
        }

        public bool Remove(T value)
        {   
            return Remove(Find(x => x.Equals(value)));
        }

        public bool RemoveLast(T value)
        {
            return Remove(FindLast(x => x.Equals(value)));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public struct Enumerator : IEnumerator<T>
        {
            public T Current => _node.Value;

            object IEnumerator.Current => _node.Value;

            private MyLinkedList<T> _data; // 책
            private MyLinkedListNode<T>? _node; // 현재 노드 (페이지)
            private MyLinkedListNode<T> _error;
            public Enumerator(MyLinkedList<T> data)
            {
                _data = data;
                _node = _error = new MyLinkedListNode<T>(default);
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_node == null)
                    return false;

                _node = _node == _error ? _data._first : _node.Next;
                return _node != null;
            }

            public void Reset()
            {
                _node = _error;
            }
        }
    }
}
