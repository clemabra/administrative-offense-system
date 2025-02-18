using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using src.Models.Wz;

namespace src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WzController(WzDbContext context) : ControllerBase
    {
        private readonly WzDbContext _context = context;
        
        [HttpGet]
        public IActionResult Get()
        {
            var entities = _context.WzEntities.ToList();
            var ids = entities.Select(r => r.Id);
            return Ok(new { message = "Daten erfolgreich übertragen.", data = entities, ids });
        }
       
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var entity = _context.WzEntities.FirstOrDefault(r => r.Id == id);
            if (entity == null)
                return NotFound(new { message = "Daten nicht gefunden." });

            return Ok(new { message = "Daten erfolgreich übertragen.", data = entity });
        }
        
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] JsonWzEntity updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest(new { message = "Invalide Daten. Entity ist leer." });
            
            // Checking that 'Weiserzeichen' has a maximum length of 2
            if (!string.IsNullOrEmpty(updatedEntity.Weiserzeichen) && updatedEntity.Weiserzeichen.Length > 2)
            {
                return BadRequest(new { message = "Weiserzeichen darf maximal 2 Zeichen lang sein." });
            }

            // server side validation
            var validationResults = JsonWzEntity.ValidateEntity(updatedEntity);
            if (validationResults.Count != 0)
                return BadRequest(new { message = "Validation fehlgeschlagen.", errors = validationResults });

            var existingEntity = _context.WzEntities.FirstOrDefault(r => r.Id == id);
            if (existingEntity == null)
                return NotFound(new { message = "Entity nicht gefunden." });
            
            if (existingEntity.RowVersion != updatedEntity.RowVersion)
                return Conflict(new { message = "Speichern fehlgeschlagen. Die Daten wurden von einem anderen Nutzer bereits gespeichert." });
            
            // update existing entity with new values (+ increment RowVersion)
            existingEntity.UpdateEntity(updatedEntity);

            // Save changes to db
            _context.SaveChanges();
            
            return Ok(new { message = "Daten erfolgreich aktualisiert.", data = existingEntity });
        }
    }
}

