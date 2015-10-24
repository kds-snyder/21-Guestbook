// Data model for entries
function EntryModel() {
	this.EntryId = ko.observable();
	this.CreatedDate = ko.observable();
	this.Name = ko.observable();
	this.EntryText = ko.observable();	
}


// View model
function AppViewModel() {
  var self = this;
    
  var apiURL = 'http://localhost:49750/api'; // URL for API
  // Strings to add to API URL for resource
  var entriesURLString = '/entries';
  
  // Set strings for which pane to display:
  //  customer.All: Table of all customers
  //  customer.Add: Add/edit customer
  //  customer.Edit: Add/edit customer
  self.displayPageAllCustomers = ko.observable('customer.All');
  self.displayPageEditCustomer = ko.observable('customer.Add');
  self.displayPageAddCustomer = ko.observable('customer.Edit');

  // Initialize indicator of which page to display to all customers
  self.displayedPage = ko.observable(self.displayPageAllCustomers());

  self.entriesTable = ko.observableArray(); // Entries loaded from database

  self.selectedEntry = new EntryModel(); // Entry being added or edited
  
 

}; // End AppViewModel

ko.applyBindings(new AppViewModel());
