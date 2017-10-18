using Microsoft.AspNet.Identity.EntityFramework;

namespace PersonDictionary.Models
{
    public class MyIdentityContext : IdentityDbContext<Customer>
    {
        public MyIdentityContext() : base("PersonsMessagesStore") { }

        public static MyIdentityContext Create()
        {
            return new MyIdentityContext();
        }
    }
}