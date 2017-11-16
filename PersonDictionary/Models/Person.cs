using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PersonDictionary.Models
{
    public class Person
    {
        [Key]
        [HiddenInput (DisplayValue = false)]
        public String Id { get; set; }

        [Required,Display(Name="Имя"), MaxLength(50)]
        public string Name { get; set; }

        [Required,DataType(DataType.EmailAddress), MaxLength(100)]
        public string eMail { get; set; }

        public byte[] Foto { get; set; }

        [Required, MaxLength(50)]
        public string login { get; set; }

        [Required,DataType(DataType.Password), MaxLength(50)]
        public string password { get; set; }

        [NotMapped]
        [System.ComponentModel.DataAnnotations
            .Compare("password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        public List<Note> Notes { get; set; }

        public Person()
        {
            Notes = new List<Note>();
        }
        
        // the navigation prop
        //public Customer Customer { set; get; }    
    }
}