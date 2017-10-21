using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace PersonDictionary.Models
{
    public class Customer : IdentityUser
    {
        
        [Required]
        public string login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
        
    }
}