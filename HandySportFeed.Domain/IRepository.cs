using System.Collections.Generic;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Domain {
    public interface IRepository<T> where T : EntityBase {
        object Add(T item);
        void Remove(T item);
        void Update(T item);
        T FindById(int id);
        IEnumerable<T> FindAll();
    }
}