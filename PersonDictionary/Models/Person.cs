using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PersonDictionary.Models
{
    public class Person
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [HiddenInput (DisplayValue = false)]
        public int Id { get; set; }

        [Required,Display(Name="Имя") ]
        public string Name { get; set; }

        [RegularExpression(@"^([\w\.\-] +)@([\w\-(@)]+)((\.(\w){2,3})+)$"),
            DataType(DataType.EmailAddress)]
        public string eMail { get; set; }

        public byte[] Foto { get; set; }

        [Required]
        public string login { get; set; }

        [Required,DataType(DataType.Password)]
        public string password { get; set; }

        public List<Note> Notes { get; set; }

        public Person()
        {
            Notes = new List<Note>();
        }    
    }
}