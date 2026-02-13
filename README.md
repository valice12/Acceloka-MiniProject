<h1> ALL ID SHOWED IN HERE JUST DUMMY AND NOT AN ACTUAL ID FROM REAL LIFE PERSON</h1>

<h2> Nuget Packages </h2>

<h3> Project </h3>
FluentValidation (12.1.1)
FluentValidation.AspNetCore (11.3.1)
Microsoft.AspNetCore.OpenApi (10.0.2)
Serilog.AspNetCore (7.0.0)
Serilog.Sinks.File (7.0.0)
Sondor.Mediator (1.0.2)

<h3> Entities </h3>
Microsoft.EntityFrameworkCore.SqlServer (10.0.2)
Microsoft.EntityFrameworkCore.Design (10.0.2)

Database Migration Sql in .Entities Folder


<h2> Access </h2>

Get Data Ticket Available
http://localhost:5287/api/v1/get-available-ticket/
<br>
Get Data Booked Ticket 
http://localhost:5287/api/v1/get-booked-ticket/{bookedTicketId}
<br>
Post BookTicket (for booking ticket)
http://localhost:5287/api/v1/book-ticket
{<br>
  "tickets": [<br>
    {<br>
      "ticketCode": "5f0d101f-e40c-4a2b-8af4-4af181c7fd67",<br>
      "quantity": 2<br>
    },<br>
    {<br>
      "ticketCode": "69ADA74C-1A75-4A1B-B6DA-927317C12D47",<br>
      "quantity": 1<br>
    }<br>
  ]<br>
}<br>
<br>
Delete Revoke Ticket 
http://localhost:5287/api/v1/revoke-ticket/{bookedTicketId}/{ticketId}/{quantity of how much you want to delete}
<br>
Put EditBookedTicket
http://localhost:5287/api/v1/edit-booked-ticket/{bookedTicket}
// basically the same as delete revoke ticket, but it can go up and down and can't be deleted<br>
{<br>
  "tickets": [<br>
    {<br>
      "ticketCode": "69ada74c-1a75-4a1b-b6da-927317c12d47",<br>
      "quantity": 3<br>
    }<br>
  ]<br>
}<br>
