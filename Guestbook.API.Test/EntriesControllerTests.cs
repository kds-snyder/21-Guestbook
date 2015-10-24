using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Guestbook.API.Controllers;
using Guestbook.API.Models;

namespace Guestbook.API.Test
{
    [TestClass]
    public class EntriesControllerTests
    {
        [TestMethod]
        public void GetEntriesReturnsEntries()
        {
            //Arrange: Instantiate EntriesController so its methods can be called
            var entryController = new EntriesController();

            //Act: Call the GetEntries method
            IEnumerable<EntryModel> entries = entryController.GetEntries();

            //Assert: Verify that an array was returned with at least one element
            Assert.IsTrue(entries.Count() > 0);
        }

        [TestMethod]
        public void GetEntryReturnsEntry()
        {
            int entryIdForTest = 1;

            //Arrange: Instantiate EntriesController so its methods can be called
            var entryController = new EntriesController();

            //Act: Call the GetEntry method
            IHttpActionResult result = entryController.GetEntry(entryIdForTest);

            //Assert: 
            // Verify that HTTP status code is OK
            // Verify that returned entry ID is correct
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<EntryModel>));

            OkNegotiatedContentResult<EntryModel> contentResult =
                (OkNegotiatedContentResult<EntryModel>)result;
            Assert.IsTrue(contentResult.Content.EntryId == entryIdForTest);
        }

        [TestMethod]
        public void PutEntryUpdatesEntry()
        {
            int entryIdForTest = 1;
            string nameForTest = "Iphigenia Brown";
            string entryTextForTest = "Hallelujah!";

            //Arrange: Instantiate EntriesController so its methods can be called
            var entryController = new EntriesController();

            //Act: 
            // Get an existing entry, change it, and
            //  pass it to PutEntry           

            IHttpActionResult result = entryController.GetEntry(entryIdForTest);
            OkNegotiatedContentResult<EntryModel> contentResult =
                (OkNegotiatedContentResult<EntryModel>)result;
            EntryModel updatedEntry = (EntryModel)contentResult.Content;

            string nameBeforeUpdate = updatedEntry.Name;
            string entryTextBeforeUpdate = updatedEntry.EntryText;

            updatedEntry.Name = nameForTest;
            updatedEntry.EntryText = entryTextForTest;

            result = entryController.PutEntry
                                     (updatedEntry.EntryId, updatedEntry);

            //Assert: 
            // Verify that HTTP status code is OK
            // Get the entry and verify that it was updated

            var statusCode = (StatusCodeResult)result;

            Assert.IsTrue(statusCode.StatusCode == System.Net.HttpStatusCode.NoContent);

            result = entryController.GetEntry(entryIdForTest);

            Assert.IsInstanceOfType(result,
                typeof(OkNegotiatedContentResult<EntryModel>));

            OkNegotiatedContentResult<EntryModel> readContentResult =
                (OkNegotiatedContentResult<EntryModel>)result;
            updatedEntry = (EntryModel)readContentResult.Content;

            Assert.IsTrue(updatedEntry.Name == nameForTest);
            Assert.IsTrue(updatedEntry.EntryText == entryTextForTest);           

            updatedEntry.Name = nameBeforeUpdate;
            updatedEntry.EntryText = entryTextBeforeUpdate;

            /*
            updatedEntry.Name = "Sally Smith";
            updatedEntry.EntryText = "Great place!";           
            */

            result = entryController.PutEntry
                                 (updatedEntry.EntryId, updatedEntry);
        }

        [TestMethod]
        public void PostEntryCreatesEntry()
        {
            //Arrange: Instantiate EntriesController so its methods can be called
            var entryController = new EntriesController();

            //Act: 
            // Create an EntryModel object populated with test data,
            //  and call PostEntry
            var newEntry = new EntryModel
            {
                Name = "Testy",
                EntryText = "Welcome to my world!"
            };
            IHttpActionResult result = entryController.PostEntry(newEntry);

            //Assert:
            // Verify that the HTTP result is CreatedAtRouteNegotiatedContentResult
            // Verify that the HTTP result body contains a nonzero entry ID
            Assert.IsInstanceOfType
                (result, typeof(CreatedAtRouteNegotiatedContentResult<EntryModel>));
            CreatedAtRouteNegotiatedContentResult<EntryModel> contentResult =
                (CreatedAtRouteNegotiatedContentResult<EntryModel>)result;
            Assert.IsTrue(contentResult.Content.EntryId != 0);

            // Delete the test entry 
            result = entryController.DeleteEntry(contentResult.Content.EntryId);
        }

        [TestMethod]
        public void DeleteEntryDeletesEntry()
        {

            //Arrange:
            // Instantiate EntriessController so its methods can be called
            // Create a new entry to be deleted, and get its entry ID           
            var entryController = new EntriesController();

            var entry = new EntryModel
            {
                Name = "Zia Prostnow",
                EntryText = "Whippersnappers!"
            };
            IHttpActionResult result = entryController.PostEntry(entry);
            CreatedAtRouteNegotiatedContentResult<EntryModel> contentResult =
                (CreatedAtRouteNegotiatedContentResult<EntryModel>)result;

            int entryIdToDelete = contentResult.Content.EntryId;

            //Act: Call DeleteEntry 
            result = entryController.DeleteEntry(entryIdToDelete);

            //Assert: 
            // Verify that HTTP result is OK
            // Verify that reading deleted entry returns result not found
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Entry>));

            result = entryController.GetEntry(entryIdToDelete);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
