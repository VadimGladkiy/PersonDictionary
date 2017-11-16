using System;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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