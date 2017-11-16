using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonDictionary.Models
{
    public class UnitOfWork : IDisposable
    {
        private DataContext db = new DataContext();
        private PersonRepository personRepo;
        private NoteRepository noteRepo;
        public String currUserId { set; get; }
        public PersonRepository Persons
        {
            get
            {
                if (personRepo == null)
                    personRepo = new PersonRepository(db);
                return personRepo;
            }
        }
        public NoteRepository Notations
        {
            get
            {
                if (noteRepo == null)
                    noteRepo = new NoteRepository(db);
                return noteRepo;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}