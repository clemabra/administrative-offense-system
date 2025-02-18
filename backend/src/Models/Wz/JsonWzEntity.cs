using System.Text.Json.Serialization;

namespace src.Models.Wz
{
    public class JsonWzEntity : WzEntity
    {
        [JsonRequired]
        [JsonPropertyName("rowVersion")]
        public new int RowVersion { get; set; } = 0;
        
        [JsonRequired]
        [JsonPropertyName("fallnummer")]
        public new string Fallnummer { get; set; } = string.Empty;
        
        [JsonRequired]
        [JsonPropertyName("weiserzeichen")]
        public new string Weiserzeichen { get; set; } = string.Empty;

        /// <summary>
        /// Validates a JsonWzEntity object and returns a list of validation errors.
        /// </summary>
        /// <param name="entity">The JsonWzEntity object to validate.</param>
        /// <returns>A list of validation errors. An empty list indicates that the entity is valid.</returns>
        internal static List<string> ValidateEntity(JsonWzEntity entity)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(entity.Fallnummer))                        
                errors.Add("Fallnummer wird benötigt.");
            
            return errors;
        }
    }
}

