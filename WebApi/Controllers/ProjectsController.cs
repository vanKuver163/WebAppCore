using Core.Models;
using DataStore.EF;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WebAppCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly AppDBContext _db;
        public ProjectsController(AppDBContext db)
        {
            _db = db;
        }
    
        [HttpGet]       
        public IActionResult Get()
        {
            return Ok(_db.Projects.ToList());
        }

        [HttpGet("{id}")]        
        public IActionResult GetById(int id)
        {
            var project = _db.Projects.Find(id);
            if (project == null) return NotFound();
            return Ok(project);
        }

        //api/projects/{pid}/tickets?tid={tid}
        [HttpGet]
        [Route("/api/projects/{pid}/tickets")]
        public IActionResult GetProjectTickets(int pid)
        {
            var tickets = _db.Tickets.Where(t => t.ProjectId == pid).ToList();
            if(tickets == null || tickets.Count<=0)
            {
                return NotFound();
            }
            return Ok(tickets);
        }


        [HttpPost]        
        public IActionResult Post([FromBody]Project project)
        {
            _db.Projects.Add(project);
            _db.SaveChanges();
            return CreatedAtAction(nameof(GetById),
                new { id = project.ProjectId },
                project
                );
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Project project)
        {
            if (id != project.ProjectId) return BadRequest();

            _db.Entry(project).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            try
            {
                _db.SaveChanges();
            }
            catch
            {
                if (_db.Projects.Find(id) == null)
                    return NotFound();
                throw;
            }

            return NoContent();
        }



        [HttpDelete("{id}")]        
        public IActionResult Delete(int id)
        {
            var project = _db.Projects.Find(id);
            if (project == null) return NotFound();
            _db.Projects.Remove(project);
            _db.SaveChanges();

            return Ok(project);
        }
    }
}
