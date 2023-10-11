using System;
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

    internal class MyHashtable<TKey, TValue>
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
                _buckets[index] = new List<KeyValuePair<TKey, TValue>>();
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

        // 숙 제 
        public bool Remove(TKey key)
        {
            // 1. 해시 ID 구해서 버킷 찾음
            // 2. 버킷에서 내가 원하는 key 와 동일한 KeyValuePair 있는지 확인
            // 3. 있으면 해당 KeyValuePair 를 버킷에서 삭제 
            // 4. 삭제했는데 만약 현재 버킷의 아이템 개수가 0개면 유효한인덱스리스트에서 해당 인덱스 제거
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

        // 숙 제 
        // Enumerator 구현
    }

}
