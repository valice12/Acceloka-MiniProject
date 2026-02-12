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
        public async Task<IActionResult> GetBookedTicket([FromRoute] Guid BookedTicketId)
        {
            if (ModelState.IsValid == false) return BadRequest("Error");
            
            try
            {
                var result = await _service.GetBookedTicketList(BookedTicketId);
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


        // Add this POST action to ControllerBookedTicket
        // Expect JSON in the request body. Validate null body to avoid 500.
        [HttpPost("book-ticket")]
        public async Task<IActionResult> BookTicket([FromBody] ModelBookTicketRequest request)
        {

            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            if (request == null || request.Tickets == null || !request.Tickets.Any())
            {
                return BadRequest(new ProblemDetails
                {   
                    Title = "Validation Error",
                    Detail = "Request body cannot be null or empty.",
                    Status = 400
                });
            }

            try
            {
                var result = await _service.PostBookedTicket(request);
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

        // PUT api/<ControllerBookedTicket>/5
        [HttpPut("edit-booked-ticket/{BookedTicketId}")]
        public async Task<IActionResult> PutBookedTicket(
            [FromRoute] Guid BookedTicketId,
            [FromBody] ModelEditBookedTicketRequest request)
        {
            try
            {
                var result = await _service.EditBookedTicketQuantity(BookedTicketId, request);

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