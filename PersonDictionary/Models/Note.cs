using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;

namespace PersonDictionary.Models
{   
    [Serializable, DataContract]
    public class Note
    {
        [DataMember]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [DataMember]
        [Required,StringLength(500),UIHint("MultilineText")]
        public string message { get; set; }
        [DataMember]
        [DataType(DataType.DateTime)]
        public DateTime time { get; set; }
        [DataMember]
        [ForeignKey("NoteAuthor")]
        public String PersonId { get; set; }
        [ScriptIgnore]
        public virtual Person NoteAuthor { get; set; }
    }
}