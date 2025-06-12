using CompanyEventAPI.Context;
using CompanyEventAPI.DTO;
using CompanyEventAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyEventAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientGetDTO>>> GetAllClients()
        {
            var clients = await _context.Client
                .Select(c => new ClientGetDTO
                {
                    Id = c.Id,
                    Empresa = c.Empresa,
                    NameCEO = c.NameCEO,
                    SurnameCEO = c.SurnameCEO,
                    Email = c.Email,
                    NombreAssistents = c.NombreAssistents,
                    Vip = c.Vip,
                    DataRegistre = c.DataRegistre
                })
                .ToListAsync();

            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientGetDTO>> GetClientById(int id)
        {
            var client = await _context.Client.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            ClientGetDTO clientGet = new ClientGetDTO
            {
                Id = client.Id,
                Empresa = client.Empresa,
                NameCEO = client.NameCEO,
                SurnameCEO = client.SurnameCEO,
                Email = client.Email,
                NombreAssistents = client.NombreAssistents,
                Vip = client.Vip,
                DataRegistre = client.DataRegistre
            };

            return Ok(clientGet);
        }

        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(ClientInsertDTO clientDTO)
        {
            var client = new Client
            {
                Empresa = clientDTO.Empresa,
                NameCEO = clientDTO.Name,
                SurnameCEO = clientDTO.Surename,
                Email = clientDTO.Email,
                NombreAssistents = clientDTO.NombreAssistents
            };

            try
            {
                await _context.Client.AddAsync(client);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex);
            }


            return CreatedAtAction(nameof(GetClientById), new { id = client.Id }, client);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Client.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            try
            {
                _context.Client.Remove(client);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex);
            }
            return NoContent();
        }

        [Authorize(Roles = "HR , Admin")]
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutFilm(int id, Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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


        private bool ClientExists(int id)
        {
            return _context.Client.Any(e => e.Id == id);
        }
    }
}
