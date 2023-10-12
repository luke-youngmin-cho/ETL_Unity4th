using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    public struct KeyValuePair<T, K> : IEquatable<KeyValuePair<T, K>>
        where T : IEquatable<T>
        where K : IEquatable<K>
    {
        public T? Key;
        public K? Value;

        public KeyValuePair(T? key, K? value)
        {
            Key = key;
            Value = value;
        }

        public bool Equals(KeyValuePair<T, K> other)
        {
            return other.Key.Equals(Key) && other.Value.Equals(Value);
        }
    }

    internal class MyHashtable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
        where TKey : IEquatable<TKey>
        where TValue : IEquatable<TValue>
    {
        public TValue this[TKey key]
        {
            get
            {
                List<KeyValuePair<TKey, TValue>> bucket = _buckets[Hash(key)];

                if (bucket == null)
                    throw new Exception($"[MyHashtable<{nameof(TKey)},{nameof(TValue)}] : Key {key} doesn't exist");

                for (int i = 0; i < bucket.Count; i++)
                {
                    if (bucket[i].Key.Equals(key))
                        return bucket[i].Value;
                }

                throw new Exception($"[MyHashtable<{nameof(TKey)},{nameof(TValue)}] : Key {key} doesn't exist");
            }
            set
            {
                List<KeyValuePair<TKey, TValue>> bucket = _buckets[Hash(key)];

                if (bucket == null)
                    throw new Exception($"[MyHashtable<{nameof(TKey)},{nameof(TValue)}] : Key {key} doesn't exist");

                for (int i = 0; i < bucket.Count; i++)
                {
                    if (bucket[i].Key.Equals(key))
                        bucket[i] = new KeyValuePair<TKey, TValue>(key, value);
                }

                throw new Exception($"[MyHashtable<{nameof(TKey)},{nameof(TValue)}] : Key {key} doesn't exist");
            }
        }

        public IEnumerable<TKey> Keys
        {
            get
            {
                List<TKey> keys = new List<TKey>();
                for (int i = 0; i < _validIndexList.Count; i++)
                {
                    for (int j = 0; j < _buckets[_validIndexList[i]].Count; j++)
                    {
                        keys.Add(_buckets[_validIndexList[i]][j].Key);
                    }
                }
                return keys;
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                List<TValue> values = new List<TValue>();
                for (int i = 0; i < _validIndexList.Count; i++)
                {
                    for (int j = 0; j < _buckets[_validIndexList[i]].Count; j++)
                    {
                        values.Add(_buckets[_validIndexList[i]][j].Value);
                    }
                }
                return values;
            }
        }
        private const int DEFAULT_SIZE = 100;
        private List<KeyValuePair<TKey, TValue>>[] _buckets
            = new List<KeyValuePair<TKey, TValue>>[DEFAULT_SIZE];
        private List<int> _validIndexList = new List<int>(); // 등록된 Key 값이 있는 인덱스 목록

        public void Add(TKey key, TValue value)
        {
            int index = Hash(key);
            List<KeyValuePair<TKey, TValue>> bucket = _buckets[index];

            // 해당 인덱스에 버킷이 없으면 새로 만듬
            if (bucket == null)
            {
                bucket = _buckets[index] = new List<KeyValuePair<TKey, TValue>>();
                _validIndexList.Add(index);
            }
            else
            {
                // 버킷이 있으면 해당 버킷에 중복 키가 있는지 확인 (해시테이블은 중복키 허용 안하니까 예외던짐)
                for (int i = 0; i < bucket.Count; i++)
                {
                    if (bucket[i].Key.Equals(key))
                        throw new Exception($"[MyHashtable<{nameof(TKey)},{nameof(TValue)}] : Key {key} doesn't exist");
                }
            }

            bucket.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public bool TryAdd(TKey key, TValue value)
        {
            int index = Hash(key);
            List<KeyValuePair<TKey, TValue>> bucket = _buckets[index];

            if (bucket == null)
            {
                _buckets[index] = new List<KeyValuePair<TKey, TValue>>();
                _validIndexList.Add(index);
            }
            else
            {
                for (int i = 0; i < bucket.Count; i++)
                {
                    if (bucket[i].Key.Equals(key))
                        return false;
                }
            }

            bucket.Add(new KeyValuePair<TKey, TValue>(key, value));
            return true;
        }


        public bool TryGetValue(TKey key, out TValue value)
        {
            int index = Hash(key);
            List<KeyValuePair<TKey, TValue>> bucket = _buckets[index];

            if (bucket != null)
            {
                for (int i = 0; i < bucket.Count; i++)
                {
                    if (bucket[i].Key.Equals(key))
                    {
                        value = bucket[i].Value;
                        return true;
                    }
                }
            }

            value = default;
            return false;
        }

        public bool Remove(TKey key)
        {
            // 1. 해시 ID 구해서 버킷 찾음
            int index = Hash(key);
            var bucket = _buckets[index];
            // 2. 버킷에서 내가 원하는 key 와 동일한 KeyValuePair 있는지 확인
            for (int i = 0; i < bucket.Count; i++)
            {
                if (bucket[i].Equals(key))
                {
                    // 3. 있으면 해당 KeyValuePair 를 버킷에서 삭제 
                    bucket.RemoveAt(i);
                    // 4. 삭제했는데 만약 현재 버킷의 아이템 개수가 0개면 유효한인덱스리스트에서 해당 인덱스 제거
                    if (bucket.Count == 0)
                    {
                        _validIndexList.Remove(index);
                    }

                    return true;
                }
            }
            return false;
        }

        public int Hash(TKey key)
        {
            string keyName = key.ToString();
            int result = 0;
            for (int i = 0; i < keyName.Length; i++)
            {
                result += keyName[i];
            }
            result %= DEFAULT_SIZE;
            return result;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            public KeyValuePair<TKey, TValue> Current => _data._buckets[_data._validIndexList[_validIndex]][_itemIndex];

            object IEnumerator.Current => _data._buckets[_data._validIndexList[_validIndex]][_itemIndex];

            private MyHashtable<TKey, TValue> _data;
            private int _validIndex; // 현재 몇번째 버킷인지
            private int _itemIndex; // 현재 버킷에서 몇번째 아이템인지

            public Enumerator(MyHashtable<TKey, TValue> data)
            {
                _data = data;
                _validIndex = 0;
                _itemIndex = -1;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                // 끝까지 이미 탐색 다했으면 탐색안됨
                // (버킷인덱스가 초과했으면)
                if (_validIndex > _data._validIndexList.Count - 1)
                    return false;

                _itemIndex++; // 다음 아이템으로
                // 아이템 인덱스 초과시
                if (_itemIndex > _data._buckets[_data._validIndexList[_validIndex]].Count - 1)
                {
                    _validIndex++; // 다음 버킷으로
                    _itemIndex = 0; // 넘어간 버킷의 첫번째 아이템으로
                }

                return _validIndex < _data._validIndexList.Count; // 다음 아이템 유효한지
            }

            public void Reset()
            {
                _validIndex = 0;
                _itemIndex = -1;
            }
        }
    }

}
