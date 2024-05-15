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

        [HttpPost("Join")]
        public async Task<IActionResult> Join([FromQuery]JoinTeam joinTeam)
        {
            var projects = await _context.Projects.FindAsync(joinTeam.ProjectId);

            if (projects == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(joinTeam.UserId);

            if (user == null)
            {
                return NotFound();
            }
            var findExisting = await _context.User_Project.FirstOrDefaultAsync(d=>d.ProjectId == joinTeam.ProjectId && d.UserId == joinTeam.UserId);
            if (findExisting != null)
            {
                return BadRequest("Mavjud volontyor!");
            }
            User_Project project = new()
            {
                ProjectId = joinTeam.ProjectId,
                UserId = joinTeam.UserId,

            };
            await _context.User_Project.AddAsync(project);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("GetProjectMembers")]
        public async Task<IActionResult> GetProjectMembers(int id)
        {
            if (_context.User_Project == null)
            {
                return NotFound();
            }
            var data = await _context.User_Project.Where(d=>d.ProjectId == id)
                .Include(d => d.User).Include(d => d.Project)
                .Select(d=> new ProjectMembers()
                {
                    ProjectId=d.ProjectId,
                    ProjectName = d.Project.ProjectName,
                    Score = d.Score,
                    UserId = d.UserId,
                    up_Id=d.up_Id,
                    UserInfo = $"{d.User.FIO}  (tel: {d.User.PhoneNumber})"
                })
                .ToListAsync();
            return Ok(data);
        }
        [HttpPut("GiveRatings")]
        public async Task<IActionResult> GiveRatings(int id, User_Project user_Project)
        {
            if (id != user_Project.up_Id)
            {
                return BadRequest();
            }

            _context.Entry(user_Project).State = EntityState.Modified;

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

            return Ok();
           
        }

        [HttpGet("GetActiveOnes")]
        public async Task<IActionResult> GetActiveOnes()
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }
            return Ok(await _context.Projects.Where(d=>d.Status == Status.Active).ToListAsync());
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
        public async Task<IActionResult> GetProjects(int id)
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
         
            
            return Ok(projects);
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

            return Ok();
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
