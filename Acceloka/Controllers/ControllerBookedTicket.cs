using Acceloka.Entities;
using Acceloka.Models;
using Acceloka.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Acceloka.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ControllerBookedTicket : ControllerBase
    {
        private readonly ServiceBookedTicket _service;

        public ControllerBookedTicket(ServiceBookedTicket service)
        {
            _service = service;
        }

        // GET: api/v1/get-booked-ticket/{Guid}
        [HttpGet("get-booked-ticket/{BookedTicketId}")]
        public async Task<IActionResult> GetBookedTicket([FromRoute] Guid? BookedTicketId)
        {
            if (ModelState.IsValid == false) return BadRequest("Error");

            if (!BookedTicketId.HasValue)
            {
                return BadRequest(new
                {
                    BookedTicketId = "The BookedTicketId field is required."
                });
            }

            // Panggil Service
            var data = await _service.GetBookedTicketListById(BookedTicketId.Value);

            // Cek Data Kosong
            if (data == null || !data.Any())
            {
                return Problem(
                    detail: $"Booked Ticket dengan ID '{BookedTicketId}' tidak ada di database",
                    statusCode: 404,
                    title: "Data Not Found"
                );
            }

            // Logic Grouping Output
            var response = data
                .GroupBy(t => t.TicketCodeNavigation.CategoryName)
                .Select(Group => new
                {
                    QtyPerCategory = Group.Sum(t => t.Quantity),
                    CategoryName = Group.Key,
                    Tickets = Group.Select(t => new
                    {
                        TicketCode = t.TicketCode, // Sudah GUID
                        TicketName = t.TicketCodeNavigation.TicketName,
                        // Format DateTimeOffset ke String yang mudah dibaca
                        eventDate = t.TicketCodeNavigation.EventDateStart != default(DateTimeOffset)
                            ? t.TicketCodeNavigation.EventDateStart.ToString("dd-MM-yyyy") + " 00:00"
                            : null
                    }).ToList()
                });


            return Ok(response);
        }


        // Add this POST action to ControllerBookedTicket
        // Expect JSON in the request body. Validate null body to avoid 500.
        [HttpPost("create-booked-ticket")]
        public async Task<IActionResult> PostBookTicket([FromBody] ModelBookedTicketCreateRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { error = "Request body is required and must be valid JSON." });
            }

            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var Data = await _service.PostBookedTicket(request);

            return Ok(Data);
        }

        // PUT api/<ControllerBookedTicket>/5
        [HttpPut("edit-booked-ticket/{BookedTicketId}")]
        public async Task<IActionResult> PutBookedTicket(
            [FromRoute] Guid BookedTicketId,
            [FromBody] ModelEditBookedTicketRequest request)
        {
            try
            {
                var result = await _service.EditBookedTIcketQuantity(BookedTicketId, request);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = ex.Message,
                    Status = 404
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = ex.Message,
                    Status = 400
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = ex.Message,
                    Status = 500
                });

            }
        }
            // DELETE api/<ControllerBookedTicket>/5
            [HttpDelete("revoke-ticket/{BookedTicketId}/{TicketCode}/{Quantity}")]
        public async Task<IActionResult> RevokeTicket(
            [FromRoute] Guid BookedTicketId, 
            [FromRoute] Guid TicketCode, 
            [FromRoute] int Quantity)
        {

            try
            { 
                var result = await _service.RevokeBookedTicket(BookedTicketId, TicketCode, Quantity);

                if (result == null) return NotFound(new { title = "Data tidak ditemukan" }); ;

                return Ok(result);
            } catch (ArgumentException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = ex.Message,
                    Status = 400
                });
            } catch (KeyNotFoundException ex)
            {
                return NotFound(new ProblemDetails
                { 
                    Title = "Not Found",
                    Detail = ex.Message,
                    Status = 404
                });

            }


        }
    }
}