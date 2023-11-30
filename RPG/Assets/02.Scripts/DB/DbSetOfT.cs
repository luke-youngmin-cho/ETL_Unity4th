using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.DB
{
    public class DbSet<T> : IEnumerable<T>
    {
        public DbSet()
        {
            Entities = new List<T>();
            path = $"{Application.persistentDataPath}/{typeof(T)}.json";
        }


        public List<T> Entities;
        public string path { get; set; }

        public T Create()
        {
            T entity = Activator.CreateInstance<T>();
            Entities.Add(entity);
            return entity;
        }

        public void Add(T entity)
        {
            Entities.Add(entity);
        }

        public T Find(Predicate<T> match)
        {
            return Entities.Find(match);
        }

        public bool Remove(T entity)
        {
            return Entities.Remove(entity);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Entities.GetEnumerator();
        }
    }
}