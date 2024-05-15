using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Volontyor_Hakaton.Data;
using Volontyor_Hakaton.DTOs.Donats;
using Volontyor_Hakaton.Models;

namespace Volontyor_Hakaton.Controllers.Stuff
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonatsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DonatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Donats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Donats>>> GetDonats()
        {
          if (_context.Donats == null)
          {
              return NotFound();
          }
            return await _context.Donats.ToListAsync();
        }

        // GET: api/Donats/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Donats>> GetDonats(int id)
        {
          if (_context.Donats == null)
          {
              return NotFound();
          }
            var donats = await _context.Donats.FindAsync(id);

            if (donats == null)
            {
                return NotFound();
            }

            return donats;
        }

        // PUT: api/Donats/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDonats(int id, Donats donats)
        {
            if (id != donats.DonatId)
            {
                return BadRequest();
            }

            _context.Entry(donats).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonatsExists(id))
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

        // POST: api/Donats
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Donats>> PostDonats(CreateDonat donats)
        {
          if (_context.Donats == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Donats'  is null.");
          }
            Donats donat = new()
            {
                DonatAmount = donats.DonatAmount,
                DonatedAt = DateTime.Now,
                Donater = donats.Donater,
                DotanetFor = donats.DotanetFor,
            };
            _context.Donats.Add(donat);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDonats", new { id = donat.DonatId }, donat);
        }

        // DELETE: api/Donats/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonats(int id)
        {
            if (_context.Donats == null)
            {
                return NotFound();
            }
            var donats = await _context.Donats.FindAsync(id);
            if (donats == null)
            {
                return NotFound();
            }

            _context.Donats.Remove(donats);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DonatsExists(int id)
        {
            return (_context.Donats?.Any(e => e.DonatId == id)).GetValueOrDefault();
        }
    }
}
