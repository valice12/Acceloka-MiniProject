using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Acceloka.Services;
using Acceloka.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Acceloka.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ControllerTicket : ControllerBase
    {
        public readonly ServiceTicket _service;

        public ControllerTicket(ServiceTicket service)
        {
            _service = service;
        }
      
        [HttpGet("get-available-ticket")]
        public async Task<IActionResult> GetTickets([FromQuery] ModelTicketSearchRequest model)
        {
            if (ModelState.IsValid == false) return BadRequest("Error gatau kenapa");

            if(model == null) return BadRequest("tidak ada data null");

            var data = await _service.GetAvailableTicketList(model);
            return Ok(data);

        }

    }
}
