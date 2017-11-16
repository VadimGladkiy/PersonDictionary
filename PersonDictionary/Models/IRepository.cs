using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonDictionary.Models
{
    interface IRepository<T,V> where T: class  
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(String allByParameter);
        T Get(V id);
        void Create(T item);
        void Update(T item);
        bool Delete(V id);
    }
}
