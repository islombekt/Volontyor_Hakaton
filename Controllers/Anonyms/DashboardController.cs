using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Volontyor_Hakaton.Data;
using Volontyor_Hakaton.DTOs.Dashboard;
using Volontyor_Hakaton.Models;

namespace Volontyor_Hakaton.Controllers.Anonyms
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Donats")]
        public async Task<IActionResult> Donats([FromQuery] Period period)
        {
            DonatsDTO donatsDTO = new DonatsDTO();

            donatsDTO.DonatsChart = await _context.Donats
                .Where(d => d.DonatedAt.Date >= period.start.Date && d.DonatedAt <= period.end.Date)
                .GroupBy(d=> d.DonatedAt.Date)
                .Select(d =>new DonatsChart(){
                    Date = d.Key,
                    Amount = d.Sum(c => c.DonatAmount)
            }).ToListAsync();
            donatsDTO.ExpensesChart = await _context.Expenses
               .Where(d => d.DateTime.Date >= period.start.Date && d.DateTime <= period.end.Date)
               .GroupBy(d => d.DateTime.Date)
               .Select(d => new ExpensesChart()
               {
                   Date = d.Key,
                   Amount = d.Sum(c => c.E_Price)
               }).ToListAsync();

           

            return Ok(donatsDTO);
        }

        [HttpGet("VolontyorsRatings")]
        public async Task<IActionResult> VolontyorsRatings()
        {
            var users = await _context.User_Project.GroupBy(d => d.UserId).Select(d => new Ratings()
            {
                UserId = d.Key,
                score = d.Sum(c => c.Score),
                rate =d.Count() == 0 ? 0 : d.Sum(c => c.Score) / d.Count(),
                UserName = "",

            }).OrderByDescending(d=> d.score).Take(10).ToListAsync();
            return Ok(users);
        }

        [HttpGet("Project")]
        public async Task<IActionResult> Project()
        {
           
            var types =await _context.Projects.GroupBy(d => d.ProjectType)
                .Select(d => new ProjectsDTO()
                {
                    ProjectType = d.Key,
                    ActiveOnes = d.Where(d=>d.Status  == Status.Active).Count(),
                    FinishedOnes = d.Where(d=>d.Status == Status.Finished).Count(),
                }).ToListAsync();

            return Ok(types);
        }
    }
}
