using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PersonDictionary.Models
{
    public class NoteRepository : IRepository<Note>
    {
        private DataContext db;

        public NoteRepository(DataContext context)
        {
            this.db = context;
        }

        public IEnumerable<Note> GetAll(int allById)
        {
            return db.Notations.Where(e => e.PersonId == allById)
                .OrderByDescending(e => e.time).ToList();
        }
        public Note Get(int id)
        {
            return db.Notations.Find(id);
        }

        public void Create(Note note)
        {
            db.Notations.Add(note);
        }

        public void Update(Note note)
        {
            db.Entry(note).State = EntityState.Modified;
        }

        public bool Delete(int id)
        { 
            Note itemToDel = db.Notations.First(x => x.id == id);
            if (itemToDel.time.AddMinutes(1440) < DateTime.Now)
            {
                return false;
            }
            else 
            {
                db.Notations.Remove(itemToDel);
                return true;
            }
        }
  
        public IEnumerable<Note> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}