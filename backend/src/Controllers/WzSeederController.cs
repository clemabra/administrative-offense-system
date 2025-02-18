/*
 * OPAS - Database Seeding Controller for Reference Numbers
 * Copyright (C) 2025 Ims2425Bruh
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

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