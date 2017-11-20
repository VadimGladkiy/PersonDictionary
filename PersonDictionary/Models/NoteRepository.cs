using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PersonDictionary.Models
{
    public class NoteRepository : IRepository<Note, Int32>
    {
        private DataContext db;

        public NoteRepository(DataContext context)
        {
            this.db = context;
        }

        public IEnumerable<Note> GetAll(String allByParameter)
        {
            return db.Notations.Where(e => e.PersonId == allByParameter)
                .OrderByDescending(e => e.time).ToList();
        }
        public Note Get(Int32 id)
        {
            return db.Notations.Find(id);
        }
        public Note Get(int id, String userId)
        {
            return db.Notations.FirstOrDefault(x => x.PersonId == userId && x.id == id);
        }
        public void Create(Note note)
        {
            db.Notations.Add(note);
        }

        public void Update(Note note)
        {
            db.Entry(note).State = EntityState.Modified;
        }

        public bool Delete(Int32 id)
        {
            try
            {
                Note itemToDel = db.Notations.SingleOrDefault(x => x.id == id);
                if (itemToDel == null) return false;
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
            catch (Exception)
            {
                return false;
            }
        }
        public IEnumerable<Note> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}