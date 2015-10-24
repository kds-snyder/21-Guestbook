using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Guestbook.API;
using Guestbook.API.Models;

namespace Guestbook.API.Controllers
{
    public class EntriesController : ApiController
    {
        private GuestbookEntities db = new GuestbookEntities();

        // GET: api/Entries
        public IQueryable<EntryModel> GetEntries()
        {
            // Project the list of Entries objects 
            //  onto a list of EntryModel objects
            return db.Entries.Select(e => new EntryModel
            {
                EntryId = e.EntryId,
                CreatedDate = e.CreatedDate,               
                Name = e.Name,
                EntryText = e.EntryText
            });
        }

        // GET: api/Entries/5
        [ResponseType(typeof(EntryModel))]
        public IHttpActionResult GetEntry(int id)
        {
            Entry dbEntry = db.Entries.Find(id);

            if (dbEntry == null)
            {
                return NotFound();
            }

            // Populate new EntryModel object from Entry object
            EntryModel modelEntry = new EntryModel
            {
                EntryId = dbEntry.EntryId,
                CreatedDate = dbEntry.CreatedDate,
                Name = dbEntry.Name,
                EntryText = dbEntry.EntryText
            };

            return Ok(modelEntry);
        }

        // PUT: api/Entries/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEntry(int id, EntryModel entry)
        {
            // Validate the request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != entry.EntryId)
            {
                return BadRequest();
            }

            if (!EntryExists(id))
            {
                return BadRequest();
            }

            //  Get the entry record corresponding to the entry ID, 
            //   update its properties to the values in the input EntryModel object,
            //    and then set an indicator that the record has been modified
            var dbEntry = db.Entries.Find(id);
            dbEntry.Update(entry);
            db.Entry(dbEntry).State = EntityState.Modified;

            // Perform update by saving changes to DB
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw new Exception("Unable to update the entry in the database.");
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Entries
        [ResponseType(typeof(EntryModel))]
        public IHttpActionResult PostEntry(EntryModel entry)
        {
            // Validate the request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Set up new Entry object,
            //  and populate it with the values from 
            //  the input EntryModel object
            Entry dbEntry = new Entry();
            dbEntry.Update(entry);

            // Add the new Entry object to the list of Entry objects
            db.Entries.Add(dbEntry);

            // Save the changes to the DB
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {

                throw new Exception("Unable to add the entry to the database.");
            }

            // Update the EntryModel object with the new entry ID
            //  that was placed in the Entry object after the changes
            //  were saved to the DB
            entry.EntryId = dbEntry.EntryId;
            return CreatedAtRoute("DefaultApi", new { id = dbEntry.EntryId }, entry);
        }

        // DELETE: api/Entries/5
        [ResponseType(typeof(EntryModel))]
        public IHttpActionResult DeleteEntry(int id)
        {
            Entry entry = db.Entries.Find(id);
            if (entry == null)
            {
                return NotFound();
            }

            db.Entries.Remove(entry);

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {

                throw new Exception("Unable to delete the entry from the database.");
            }
            

            return Ok(entry);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EntryExists(int id)
        {
            return db.Entries.Count(e => e.EntryId == id) > 0;
        }
    }
}