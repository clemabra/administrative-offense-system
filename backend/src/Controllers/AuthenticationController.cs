using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net;
using src.Models.Authentication;
using System.DirectoryServices.Protocols;

namespace src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly SSOTokenDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            SSOTokenDbContext context,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
        }

        // POST: api/Authentication/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new IdentityUser { UserName = model.Username };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
                return Ok(new { Result = "User registered successfully" });

            return BadRequest(result.Errors);
        }

        // POST: api/Authentication/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(model.Username);

            // validate user credentials
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                // Generate the JWT token and return it
                var token = GenerateJwtToken(user);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid username or password: " + model.Username + " " + model.Password);
        }

        [Authorize]
        [HttpGet("sso-login")]
        public IActionResult SSOLogin()
        {
            // get username from windows authentication
            var username = User.Identity?.Name;
            
            System.Console.WriteLine("Authentifizierter Benutzer: " + username);
            
            if (string.IsNullOrEmpty(username))
            {
                System.Console.WriteLine("Username konnte nicht vom Token abgerufen werden.");
                return Unauthorized("Username konnte nicht vom Token abgerufen werden" + username);
            }

            // ldap authentication (optional, if additional authentication is required)
            var isAuthenticated = AuthenticateUserWithLDAP(username, "<default_password>");
            if (!isAuthenticated)
            {
                System.Console.WriteLine("LDAP-Authentifizierung fehlgeschlagen für Benutzer: " + username);
                return Unauthorized("LDAP-Authentifizierung fehlgeschlagen" + username);
            }

            // generate jwt token
            var user = _userManager.FindByNameAsync(username).Result;
            if (user == null)
                return Unauthorized("User does not have permission to access this site" + username);

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        // POST: api/Authentication/generate-sso-token
        [HttpPost("generate-sso-token")]
        [Authorize]
        public async Task<IActionResult> GenerateSSOToken()
        {
            try
            {
                // get user id from jwt
                var userId = User.FindFirstValue("User_Id");
                if (userId == null) return BadRequest("User Id not found in the token");

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null) return NotFound("User not found");

                // create new SSO token and add to db
                var ssoToken = new SSOToken
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Token = Guid.NewGuid().ToString(),
                    ExpiryDate = DateTime.UtcNow.AddMinutes(10),
                    IsUsed = false
                };

                _context.SSOTokens.Add(ssoToken);
                await _context.SaveChangesAsync();

                return Ok(new { SSOToken = ssoToken.Token });
            }
            catch (Exception ex)
            {
                // server error response for any exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Authentication/validate-sso-token
        [HttpPost("validate-sso-token")]
        public async Task<IActionResult> ValidateSSOToken([FromBody] ValidateSSOTokenRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var ssoToken = await _context.SSOTokens.SingleOrDefaultAsync(t => t.Token == request.SSOToken);

                if (ssoToken == null || ssoToken.IsUsed || ssoToken.ExpiryDate < DateTime.UtcNow)
                    return BadRequest("Invalid or expired SSO token");

                ssoToken.IsUsed = true;
                _context.SSOTokens.Update(ssoToken);
                await _context.SaveChangesAsync();

                var user = await _userManager.FindByIdAsync(ssoToken.UserId);

                if (user == null) return NotFound("User not found");
                var newJwtToken = GenerateJwtToken(user);

                return Ok(new
                {
                    Token = newJwtToken,
                    UserDetails = new
                    {
                        Username = user.UserName,
                        UserId = user.Id
                    }
                });
            }
            catch (Exception ex)
            {
                // server error response for any exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // public endpoint not requiring authentication
        [HttpGet("public-data")]
        public IActionResult GetPublicData()
        {
            var publicData = new { Message = "This is public data accessbile without authentication" };
            return Ok(publicData);
        }

        // protected endpoint requiring JWT token authentication
        [Authorize]
        [HttpGet("protected-data")]
        public IActionResult GetProtectedData()
        {
            var protectedData = new { Message = "This is protected data accessible only with a valid JWT token" };
            return Ok(protectedData);
        }

        // shows all logged in users
        [HttpGet("logged-users")]
        public IActionResult GetLoggedUsers()
        {
            var users = _context.Users.Select(user => new
            {
                Username = user.UserName,
            }).ToList();

            return Ok(users);
        }

        /****************************************  Helper functions ******************************************************/

        // Helper method to generate a jwt for an authenticated user
        private string GenerateJwtToken(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id ?? string.Empty),
                new(ClaimTypes.NameIdentifier, user.UserName ?? string.Empty),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            if (user.UserName != null && IsUserInGroup(user.UserName, "AdminGroup"))
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static bool AuthenticateUserWithLDAP(string username, string password)
        {
            try
            {
                var ldapConnection = new LdapConnection(new LdapDirectoryIdentifier("ldap.example.com"))
                {
                    Credential = new NetworkCredential(username, password)
                };
                
                ldapConnection.Bind();
                return true;
            }
            catch (LdapException)
            {
                return false;
            }
        }

        private static bool IsUserInGroup(string UserName, string GroupName)
        {
            try
            {
                var ldapConnection = new LdapConnection(new LdapDirectoryIdentifier("ldap.example.com"))
                    { Credential = new NetworkCredential("admin", "password") };

                ldapConnection.Bind();
                var searchFilter = $"(&(objectClass=user)(sAMAccountName={UserName}))";
                var searchRequest = new SearchRequest("DC=example,DC=com", searchFilter, SearchScope.Subtree, null);
                var response = (SearchResponse)ldapConnection.SendRequest(searchRequest);
                
                return response.Entries.Count > 0;
            }
            catch (LdapException) { return false; }
        }
    }
}