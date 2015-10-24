-- Create Guestbook database & use it
CREATE DATABASE Guestbook;
GO
USE Guestbook;
GO

-- Create table Entries to hold guestbook entries
-- Use identity property for the primary key
CREATE TABLE Entries
(
	EntryId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	CreatedDate DATETIME NOT NULL,
	Name VARCHAR(50) NOT NULL,
	EntryText VARCHAR(1000) NOT NULL	
)
GO

-- Insert an entry
INSERT INTO Entries
(CreatedDate, Name, EntryText) 
VALUES ('2015-10-23','Jeff Winkler','Welcome to Origin Code Academy!');
GO

SELECT * FROM Entries;
