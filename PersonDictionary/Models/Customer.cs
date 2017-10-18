using Microsoft.AspNet.Identity.EntityFramework;

namespace PersonDictionary.Models
{
    public class Customer: IdentityUser
    {
        public int id { get; set; }
        public string login { get; set; }
    }
}