using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Volontyor_Hakaton.Data;
using Volontyor_Hakaton.DTOs.Identity;
using Volontyor_Hakaton.Models;
using Volontyor_Hakaton.Services.Identity;

namespace Volontyor_Hakaton.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly ITokenService _identityService;
        private readonly ApplicationDbContext _context;

        public IdentityController(ITokenService identityService, ApplicationDbContext context)
        {
            _context = context;
            _identityService = identityService;
        }
        [HttpGet("Users")]
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.Select(d => new
            {
                d.FIO,d.UserName, d.UserRole,d.UserId,d.Description,d.PhoneNumber
            }).ToListAsync();
            return Ok(users);
        }

        [HttpGet("Volontyors")]
        public async Task<IActionResult> Volontyors()
        {
            var users = await _context.Users.Where(d=> d.UserRole == "volontyor").Select(d => new
            {
                d.FIO,
                d.UserName,
                d.UserRole,
                d.UserId,
                d.Description,
                d.PhoneNumber
            }).ToListAsync();
            return Ok(users);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginRequest model)
        {
            var userList = new Users();
            if (userList == null)
            {
                return NotFound();
            }

            var response = await _identityService.LoginAsync(model);
            if (response.Token == "401")
            {
                if (!string.IsNullOrEmpty(response.RefreshToken))
                {
                    return BadRequest(response.RefreshToken);
                }
                return Unauthorized();
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterUser model)
        {
            try
            {
                var response = await _identityService.RegisterAsync(model);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "||||" + ex.InnerException);
            }
        }
        [HttpPost("RegisterVolontyors")]
        public async Task<ActionResult> RegisterVolontyors(RegisterUser model)
        {
            try
            {
                var response = await _identityService.RegisterAsync(model);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "||||" + ex.InnerException);
            }
        }
    }
}
