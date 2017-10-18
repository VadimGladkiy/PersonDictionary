using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonDictionary.Models
{
    public class Note
    {
        [Required,StringLength(500),UIHint("MultilineText")]
        string message { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime time { get; set; }

        [ForeignKey("NoteAuthor")]
        public int PersonId { get; set; }

        public virtual Person NoteAuthor { get; set; }
    }
}