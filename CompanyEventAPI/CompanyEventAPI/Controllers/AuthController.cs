using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CompanyEventAPI.DTO;
using CompanyEventAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CompanyEventAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userManager, ILogger<AuthController> logger, IConfiguration configuration)
        {
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
        }


        [HttpPost("client/registre")]
        public async Task<IActionResult> ClientRegister([FromBody] RegisterDTO model)
        {
            var usuari = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Name + model.Surename,
                Name = model.Name,
                Surname = model.Surename
            };
            var resultat = await _userManager.CreateAsync(usuari, model.Password);
            var resultatRol = new IdentityResult();

            if (resultat.Succeeded)
            {
                resultatRol = await _userManager.AddToRoleAsync(usuari, "Client");
            }

            if (resultat.Succeeded && resultatRol.Succeeded)
                return Ok("Usuari Client registrat");

            return BadRequest(resultat.Errors);
        }

        [HttpPost("hr/registre")]
        public async Task<IActionResult> HrRegister([FromBody] RegisterDTO model)
        {
            var usuari = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Name + model.Surename,
                Name = model.Name,
                Surname = model.Surename
            };
            var resultat = await _userManager.CreateAsync(usuari, model.Password);
            var resultatRol = new IdentityResult();

            if (resultat.Succeeded)
            {
                resultatRol = await _userManager.AddToRoleAsync(usuari, "HR");
            }

            if (resultat.Succeeded && resultatRol.Succeeded)
                return Ok("Usuari HR registrat");

            return BadRequest(resultat.Errors);
        }

        [HttpPost("admin/registre")]
        public async Task<IActionResult> AdminRegister([FromBody] RegisterDTO model)
        {
            var usuari = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Name + model.Surename,
                Name = model.Name,
                Surname = model.Surename
            };
            var resultat = await _userManager.CreateAsync(usuari, model.Password);
            var resultatRol = new IdentityResult();

            if (resultat.Succeeded)
            {
                resultatRol = await _userManager.AddToRoleAsync(usuari, "Admin");
            }

            if (resultat.Succeeded && resultatRol.Succeeded)
                return Ok("Usuari Admin registrat");

            return BadRequest(resultat.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            //Certifiquem que el mail existeix
            var usuari = await _userManager.FindByEmailAsync(model.Email);
            if (usuari == null || !await _userManager.CheckPasswordAsync(usuari, model.Password))
                return Unauthorized("Mail o contrasenya erronis");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuari.UserName),
                new Claim(ClaimTypes.NameIdentifier, usuari.Id.ToString())
            };

            //Adquirim els rols de l'usuari. Pot tenir mes d'un. En aquest cas 1.
            var roles = await _userManager.GetRolesAsync(usuari);

            if (roles != null && roles.Count > 0)
            {
                foreach (var rol in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, rol));
                }
            }

            return Ok(CreateToken(claims.ToArray()));
        }



        private string CreateToken(Claim[] claims)
        {
            // Carreguem les dades des del appsettings.json
            var jwtConfig = _configuration.GetSection("JwtSettings");
            var secretKey = jwtConfig["Key"];
            var issuer = jwtConfig["Issuer"];
            var audience = jwtConfig["Audience"];
            var expirationMinutes = int.Parse(jwtConfig["ExpirationMinutes"]);

            // Creem la clau i les credencials de signatura
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Construcció del token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: creds
            );

            // Retornem el token serialitzat
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
