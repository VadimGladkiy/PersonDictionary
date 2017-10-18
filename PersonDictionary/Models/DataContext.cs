using System.Data.Entity;

namespace PersonDictionary.Models
{
    public class DataContext : DbContext
    {
        public DataContext() : base("PersonsMessagesStore") { }
        
        public DbSet<Person> Persons { get; set; }
        public DbSet<Note> Notations { get; set; }
    }
}