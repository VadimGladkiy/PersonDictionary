using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PersonDictionary.Models
{
    public class PersonRepository : IRepository<Person, String>
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
        public Person Get(String id)
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
        public bool Delete(String id)
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
        public IEnumerable<Person> GetAll(String allByParameter)
        {
            throw new NotImplementedException();
        }
        public String GetUserName(String userId)
        {
            return db.Persons.Single(x => x.Id == userId).Name;
        }
        public String GetPassword(String userId)
        {
            return db.Persons.Single(x => x.Id == userId).password;
        }
    }
}