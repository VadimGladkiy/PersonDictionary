using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonDictionary.Models
{
    interface IRepository<T> where T: class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(int allById);
        T Get(int id);
        void Create(T item);
        void Update(T item);
        bool Delete(int id);
    }
}
