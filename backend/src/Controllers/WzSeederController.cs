using Microsoft.AspNetCore.Mvc;
using src.Models.Wz;
using src.Seeders;

namespace src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WzSeederController : ControllerBase
    {
        private WzDbContext _context;

        public WzSeederController(WzDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("seed")]
        public IActionResult Seed()
        {
            WzDatabaseSeeder.Seed(_context);
            return Ok("Datenbank erfolgreich gefüllt.");
        }
    }
}