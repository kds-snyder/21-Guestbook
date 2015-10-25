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

  // URL for API
  var apiURL = 'http://localhost:52264/api';
  // String to add to API URL for entries
  var entriesURLString = '/entries';

  // Indicates whether to display form for entry
  self.displayEntryForm = ko.observable(false);

  self.entriesTable = ko.observableArray(); // Entries loaded from database

  self.inputEntry = new EntryModel(); // Entry being added

  // Add entry
  self.addEntry = function() {
      self.initializeInputEntry();
      self.displayEntryForm(true);
  };

  // From input DateTime, returns date string in form mm/dd/yyyy
  //self.dateString = ko.computed(function(entry) {
  //self.dateString = ko.computed(function() {
  //self.dateString = ko.computed(function(inputDate) {
  self.dateString = function(entry) {
    var inputDateTime = entry.CreatedDate();
    //var inputDateTime = this.CreatedDate;
    //var inputDateTime = inputDate;
    return ((inputDateTime.getMonth()+1) + '/' +
            inputDateTime.getDate() + '/' +
            inputDateTime.getFullYear())
  //});
  };

    // Cancel saving entry
  self.cancelSaveEntry = function() {
    self.initializeInputEntry();
    self.displayEntryForm(false);
  };

  // Initialize input entry
  self.initializeInputEntry = function() {
    self.inputEntry.EntryId(0);
    self.inputEntry.Name(null);
		self.inputEntry.EntryText(null);
  };

  // Push new entry to entry table arrray
  self.pushNewEntry = function() {
    var newEntry = ko.mapping.fromJS(ko.mapping.toJS(self.inputEntry));
    self.entriesTable.push(newEntry);
    return true;
  };

  // Reload functions
  self.reload = {

    // Populate the entries table array from the entries table in the DB
    entries: function() {
      $.ajax({
              type: 'GET',
              url: apiURL + entriesURLString,
              success: function(data) {
                ko.mapping.fromJS(data, {}, self.entriesTable);
              }
      });
    }
  };

  // Save entry
  self.saveEntry = function() {
    // Validate the entered entry data before saving
    if (self.validateInputEntry()) {

      $.ajax({
        type: 'POST',
        url: apiURL + entriesURLString,
        contentType: 'application/json;charset=utf-8',
        data: ko.mapping.toJSON(self.inputEntry),
        success: function(data) {

          toastr.success('Your comment was successfully added');

          // Push new entry onto entries array
          //self.pushNewEntry();

          // Reload entries
          self.reload.entries();

          //  Initialize input entry data
          //  Clear indicator to display entry form
          self.initializeInputEntry();
          self.displayEntryForm(false);

        },
        error: function(jqXHR, textStatus, errorThrown) {
          toastr.error(JSON.parse(jqXHR.responseText).ExceptionMessage, 'Error Adding Comment');
       }
      });
    }
};

  // Validate input entry: name and entry text must not be empty
  self.validateInputEntry = function() {
    if (self.inputEntry.Name() == null ||
        self.inputEntry.EntryText() == null) {
        toastr.warning('The name and entry must be filled in',
                            'Required Field(s) Empty');
        return false;
    }
    else {
      return true;
    }
  };



  // After all definitions are done, display the entries
  self.reload.entries();

}; // End AppViewModel

ko.applyBindings(new AppViewModel());
