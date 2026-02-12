-- 2. Buat Database Baru
CREATE DATABASE ACCELOKA;
GO

USE ACCELOKA;
GO

-- 3. Tabel Master: Ticket
-- Berisi informasi acara/tiket yang dijual
CREATE TABLE Ticket(
    TicketCode UNIQUEIDENTIFIER NOT NULL 
        CONSTRAINT PK_Ticket PRIMARY KEY DEFAULT NEWID(),

    TicketName VARCHAR(255) NOT NULL,
    CategoryName VARCHAR(255) NOT NULL, -- Kategori gabung sini (sesuai request)
    Quota INT NOT NULL DEFAULT 0,
    Price DECIMAL(18, 2) NOT NULL,
    
    -- Informasi Waktu Event (Milik Tiket)
    EventDateStart DATETIMEOFFSET NOT NULL,
    EventDateEnd DATETIMEOFFSET NOT NULL, 

    -- Audit Trail
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
    UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM'
);
GO

-- 4. Tabel Transaksi: BookedTicket
-- Berisi data pembelian. 
-- SATU BookedTicketId bisa memiliki BANYAK baris (Id beda-beda).
CREATE TABLE BookedTicket(
    -- PRIMARY KEY BARU (Unik setiap baris)
    Id INT IDENTITY(1,1) NOT NULL 
        CONSTRAINT PK_BookedTicket_Id PRIMARY KEY,

    -- GROUP ID (Transaction ID) - Ini yang akan sama untuk list tiket dalam satu booking
    BookedTicketId UNIQUEIDENTIFIER NOT NULL,

    TicketCode UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT FK_BookedTicket_Ticket FOREIGN KEY REFERENCES Ticket(TicketCode),

    Quantity INT NOT NULL,
    Price DECIMAL(18, 2) NOT NULL, -- Harga snapshot saat beli
    
    -- Informasi Waktu Transaksi (Milik Booking)
    ScheduledDate DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    PurchaseDate DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),

    -- Audit Trail
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    CreatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM',
    UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    UpdatedBy VARCHAR(255) NOT NULL DEFAULT 'SYSTEM'
);
GO

-- Optional: Index untuk mempercepat pencarian by Booking ID
CREATE INDEX IX_BookedTicket_BookedTicketId ON BookedTicket(BookedTicketId);
GO