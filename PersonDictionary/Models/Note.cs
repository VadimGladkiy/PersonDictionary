using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonDictionary.Models
{
    public class Note
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required,StringLength(500),UIHint("MultilineText")]
        public string message { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime time { get; set; }

        [ForeignKey("NoteAuthor")]
        public int PersonId { get; set; }

        public virtual Person NoteAuthor { get; set; }
    }
}