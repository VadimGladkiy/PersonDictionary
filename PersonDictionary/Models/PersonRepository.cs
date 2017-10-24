using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PersonDictionary.Models
{
    public class PersonRepository : IRepository<Person>
    {
        private DataContext db;

        public PersonRepository(DataContext context)
        {
            this.db = context;
        }
        public IEnumerable<Person> GetAll()
        {
            return db.Persons;
        }
        public Person Get(int id)
        {
            return db.Persons.Find(id);
        }
        public void Create(Person pers)
        {
            db.Persons.Add(pers);
        }
        public void Update(Person pers)
        {
            db.Entry(pers).State = EntityState.Modified;
        }
        public bool Delete(int id)
        {
            Person pers = db.Persons.Find(id);
            if (pers != null)
            {
                db.Persons.Remove(pers);
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<Person> GetAll(int allById)
        {
            throw new NotImplementedException();
        }
    }
}