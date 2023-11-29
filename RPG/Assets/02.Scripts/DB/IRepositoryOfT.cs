using System;
using System.Collections.Generic;

namespace RPG.DB
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        T Find(Predicate<T> match);
        void Insert(T entity);
        void Update(T entity);
    }
}