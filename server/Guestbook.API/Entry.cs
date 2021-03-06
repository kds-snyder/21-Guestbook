//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Guestbook.API
{
    using Models;
    using System;
    using System.Collections.Generic;

    public partial class Entry
    {
        public int EntryId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string Name { get; set; }
        public string EntryText { get; set; }


        // Update properties in Entry class from the input object
        public void Update(EntryModel modelEntry)
        {
            // If adding new customer, set created date
            if (modelEntry.EntryId == 0)
            {
                CreatedDate = DateTime.Now;
            }

            // Copy values from input object to Customer properties
            Name = modelEntry.Name;
            EntryText = modelEntry.EntryText;
        }
    }
}
