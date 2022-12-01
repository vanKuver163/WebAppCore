using Core.Models;
using DataStore.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace WebAppCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   
    public class TicketsController : ControllerBase
    {

        private readonly AppDBContext _db;
        public TicketsController(AppDBContext db)
        {
            _db = db;
        }

        [HttpGet]
      //[Route("api/tickets")]
        public IActionResult Get()
        {
            return Ok(_db.Tickets.ToList());
        }

        [HttpGet("{id}")]
      //[Route("api/ticket/{id}")]
        public IActionResult GetById(int id)
        {
            var ticket = _db.Tickets.Find(id);
            if (ticket == null) return NotFound();
            return Ok(ticket);
        }

        [HttpPost]      
      //[Route("api/tickets")]
        public IActionResult Post([FromBody]Ticket ticket)
        {

            _db.Tickets.Add(ticket);
            _db.SaveChanges();
            return CreatedAtAction(nameof(GetById),
                new { id = ticket.TicketId },
                ticket
                );
        }
              

        [HttpPut]
      //[Route("api/tickets")]
        public IActionResult Put(int id, [FromBody] Ticket ticket)
        {
            if (id !=ticket.TicketId) return BadRequest();

            _db.Entry(ticket).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            try
            {
                _db.SaveChanges();
            }
            catch
            {
                if (_db.Tickets.Find(id) == null)
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
      //[Route("api/ticket/{id}")]
        public IActionResult Delete(int id)
        {
            var ticket = _db.Tickets.Find(id);
            if (ticket == null) return NotFound();
            _db.Tickets.Remove(ticket);
            _db.SaveChanges();

            return Ok(ticket);
        }
    }
}
