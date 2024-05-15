using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Volontyor_Hakaton.Data;
using Volontyor_Hakaton.DTOs.Expenses;
using Volontyor_Hakaton.Models;

namespace Volontyor_Hakaton.Controllers.Stuff
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Expenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expenses>>> GetExpenses()
        {
          if (_context.Expenses == null)
          {
              return NotFound();
          }
          var expenses = await _context.Expenses.Include(d=>d.Project)
                .Select(d=> new ExpenseInfo()
                {
                    E_Price = d.E_Price,
                    ProjectId = d.ProjectId,
                    ProjectName = d.Project.ProjectName,
                    Reason = d.Reason,
                    E_Id = d.E_Id,
                })
                .ToListAsync();
            return Ok(expenses);
        }

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpenses(int id)
        {
          if (_context.Expenses == null)
          {
              return NotFound();
          }
            var expenses = await _context.Expenses
                .Include(d => d.Project)
                .Select(d => new ExpenseInfo()
                {
                    E_Price = d.E_Price,
                    ProjectId = d.ProjectId,
                    ProjectName = d.Project.ProjectName,
                    Reason = d.Reason,
                    E_Id = d.E_Id,
                }).FirstOrDefaultAsync(d => d.E_Id == id);

            if (expenses == null)
            {
                return NotFound();
            }

            return Ok(expenses);
        }

        // PUT: api/Expenses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpenses(int id, Expenses expenses)
        {
            if (id != expenses.E_Id)
            {
                return BadRequest();
            }

            _context.Entry(expenses).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpensesExists(id))
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

        // POST: api/Expenses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Expenses>> PostExpenses(ExpenseCreate expenses)
        {
          if (_context.Expenses == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Expenses'  is null.");
          }
            Expenses expense = new()
            {
                DateTime = DateTime.Now,
                E_Price = expenses.E_Price,
                ProjectId = expenses.ProjectId,
                Reason = expenses.Reason,
            };
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExpenses", new { id = expense.E_Id }, expense);
        }

        // DELETE: api/Expenses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpenses(int id)
        {
            if (_context.Expenses == null)
            {
                return NotFound();
            }
            var expenses = await _context.Expenses.FindAsync(id);
            if (expenses == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expenses);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExpensesExists(int id)
        {
            return (_context.Expenses?.Any(e => e.E_Id == id)).GetValueOrDefault();
        }
    }
}
