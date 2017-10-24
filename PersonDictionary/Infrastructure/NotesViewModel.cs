using PersonDictionary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonDictionary.Infrastructure
{
    public class NotesViewModel
    {
        public IEnumerable<Note> Notes { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}