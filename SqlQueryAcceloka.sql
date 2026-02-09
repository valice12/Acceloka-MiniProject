CREATE DATABASE ACCELOKA;
GO 

USE ACCELOKA;
GO

CREATE TABLE Ticket(
	TicketCode INT IDENTITY(1,1) PRIMARY KEY ,
	TicketName VARCHAR(50) NOT NULL,
	EventDateStart DATE DEFAULT GETDATE(), 
	Quantity INT DEFAULT 1,
	CategoryName VARCHAR(50) NOT NULL,
	Price INT NOT NULL
)

CREATE TABLE BookedTicket(
	BookedId INT IDENTITY(1,1) PRIMARY KEY,
	TicketCode INT FOREIGN KEY REFERENCES Ticket NOT NULL,
	EventDate DATE NOT NULL, 
	Quantity INT NOT NULL, 
	Price INT NOT NULL

-- TicketName tidak masuk karena redundant dan ada kemungkinan kesalahan penulisan sehingga berbeda makna
-- CategoryName juga memiliki Kasus yang serupa dengan TicketName
)

SELECT * FROM Ticket t
JOIN BookedTicket bt on t.TicketCode = Bt.TicketCode
