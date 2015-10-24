using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Guestbook.API.Models
{
    public class EntryModel
    {
        public int EntryId { get; set; }       
        public DateTime CreatedDate { get; set; }
        public string Name { get; set; }
        public string EntryText { get; set; }
    }
}