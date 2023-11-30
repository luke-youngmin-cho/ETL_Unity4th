using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Collections.ObjectModel
{
    public struct Pair<T>
    {
        public int id;
        public T item;
        public Pair(int id, T item)
        {
            this.id = id;
            this.item = item;
        }
    }

    public class ObservableCollection<T> : IEnumerable<Pair<T>>
    {
        public ObservableCollection()
        {
            Items = new Dictionary<int, Pair<T>>();
        }

        public T this[int id]
        {
            get => Items[id].item;
            set => Change(id, value);
        }

        public Dictionary<int, Pair<T>> Items;

        public event Action<int, T> OnItemAdded;
        public event Action<int, T> OnItemRemoved;
        public event Action<int, T> OnItemChanged;
        public event Action OnCollectionChanged;

        public void Change(int id, T item)
        {
            if (Items.TryGetValue(id, out Pair<T> pair))
            {
                Items[id] = new Pair<T>(id, item);
                OnItemChanged?.Invoke(id, item);
                OnCollectionChanged?.Invoke();
            }
            else
                throw new Exception($"[ObservableCollection<{typeof(T)}>] : Failed to change item, {id} not found.");
        }

        public void Add(int id, T item)
        {
            if (Items.TryAdd(id, new Pair<T>(id, item)) == false)
                throw new Exception($"[ObservableCollection<{typeof(T)}>] : id {id} is already exist.");

            OnItemAdded?.Invoke(id, item);
            OnCollectionChanged?.Invoke();
        }

        public void Remove(int id)
        {
            if (Items.TryGetValue(id, out Pair<T> pair) == false)
                throw new Exception($"[ObservableCollection<{typeof(T)}>] : Failed to remove. id {id} is not exist.");

            Items.Remove(id);
            OnItemRemoved?.Invoke(id, pair.item);
            OnCollectionChanged?.Invoke();
        }


        public IEnumerator<Pair<T>> GetEnumerator()
        {
            return Items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.Values.GetEnumerator();
        }
    }
}