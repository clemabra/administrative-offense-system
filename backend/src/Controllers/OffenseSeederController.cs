using Microsoft.AspNetCore.Mvc;
using src.Models.Offenses;
using src.Seeders;

namespace src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffenseSeederController : ControllerBase
    {
        private OffenseDbContext _context;

        public OffenseSeederController(OffenseDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("seed")]
        public IActionResult Seed()
        {
            OffenseDatabaseSeeder.Seed(_context);
            return Ok("Datenbank erfolgreich gefüllt.");
        }
    }
}