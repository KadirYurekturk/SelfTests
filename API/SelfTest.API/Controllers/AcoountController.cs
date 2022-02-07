using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfTest.API.Services;
using SelfTest.Data.Contexts;
using SelfTest.Model.Entities;
using SelfTest.Model.Request;
using SelfTest.Model.Response;
using System.Security.Cryptography;
using System.Text;

namespace SelfTest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly TestDbContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(TestDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(SignupRequest login)
        {
            if (await UserExists(login.Email)) return BadRequest("UserName Is Already Taken");
            var hmac = new HMACSHA512();

            var user = new User
            {
                Username = login.Email,
                Name = login.Name,
                Email= login.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password)),
                PasswordSalt = hmac.Key,

            };

            _context.User.Add(user);

            await _context.SaveChangesAsync();
            return user;
        }
        private async Task<bool> UserExists(string username)
        {
            return await _context.User.AnyAsync(x => x.Username == username.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest loginDto)
        {
            var user = await _context.User
                .SingleOrDefaultAsync(x => x.Username == loginDto.Email);

            if (user == null) return Unauthorized("Invalid UserName");

            var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return new LoginResponse
            {
                Username = user?.Username,
                Token = _tokenService.CreateToken(user)
            };

            //List<int> denemLsite = new List<int> { 123, 123, 123, 123, 3, 32, 34, 4 };            

            //var whereDEger = denemLsite.Any(x => x == 123 && x != 4);

            //return user;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("AdminGetir")]
        public ActionResult<IEnumerable<string>> GetAdmin()
        {
            return new string[] { "ADnminvalue1", "ADinvalue2" };
        }

    }
}
