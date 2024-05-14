using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Volontyor_Hakaton.Data;
using Volontyor_Hakaton.DTOs.Dashboard;
using Volontyor_Hakaton.DTOs.Projects;
using Volontyor_Hakaton.Models;

namespace Volontyor_Hakaton.Controllers.Stuff
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Projects>>> GetProjects()
        {
          if (_context.Projects == null)
          {
              return NotFound();
          }
            return await _context.Projects.ToListAsync();
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Projects>> GetProjects(int id)
        {
          if (_context.Projects == null)
          {
              return NotFound();
          }
            var projects = await _context.Projects.FindAsync(id);

            if (projects == null)
            {
                return NotFound();
            }

            return projects;
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjects(int id, Projects projects)
        {
            if (id != projects.ProjectId)
            {
                return BadRequest();
            }

            _context.Entry(projects).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Projects>> PostProjects(ProjectCreateDTO projects)
        {
          if (_context.Projects == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Projects'  is null.");
          }
            Projects p = new()
            {
                AddressInfo = projects.AddressInfo,
                CreatedAt = DateTime.Now,
                InitiatedBy = projects.InitiatedBy,
                ProjectName = projects.ProjectName,
                Region = projects.Region,
                Status = Status.New,
                ProjectType = projects.ProjectType,

            };
            _context.Projects.Add(p);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjects", new { id = p.ProjectId }, p);
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjects(int id)
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }
            var projects = await _context.Projects.FindAsync(id);
            if (projects == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(projects);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectsExists(int id)
        {
            return (_context.Projects?.Any(e => e.ProjectId == id)).GetValueOrDefault();
        }
    }
}
