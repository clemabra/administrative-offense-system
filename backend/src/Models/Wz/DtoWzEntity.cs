using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models.Wz
{
    public class DtoWzEntity : WzEntity
    {
        // identification properties
        [Key] // primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // auto increment
        public new int Id { get; set; }
        
        /// <summary>
        /// Updates the wz entity with the values from the provided <see cref="JsonWzEntity"/>.
        /// </summary>
        /// <param name="newEntity">The new offense record containing the updated values.</param>
        public void UpdateEntity(JsonWzEntity newEntity)
        {
            Fallnummer = newEntity.Fallnummer;
            Weiserzeichen = newEntity.Weiserzeichen;
            
            // increment RowVersion
            RowVersion++;
        }
    }
}

