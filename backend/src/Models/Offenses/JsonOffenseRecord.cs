using System.Text.Json.Serialization;

namespace src.Models.Offenses
{
    public class JsonOffenseRecord : OffenseRecord
    {
        [JsonRequired]
        [JsonPropertyName("rowVersion")]
        public new int RowVersion { get; set; } = 0;
        
        [JsonPropertyName("weiserzeichen")]
        public new string Weiserzeichen { get; set; } = string.Empty;
        
        [JsonRequired]
        [JsonPropertyName("fallnummer")]
        public new string Fallnummer { get; set; } = string.Empty;

        [JsonRequired]
        [JsonPropertyName("meldedatum")]
        public new DateTime Meldedatum { get; set; }

        // insurance data
        [JsonRequired]
        [JsonPropertyName("versicherungsunternehmensnummer")]
        public new string Versicherungsunternehmensnummer { get; set; } = string.Empty;

        [JsonRequired]
        [JsonPropertyName("krankenversicherung")]
        public new string Krankenversicherung { get; set; } = string.Empty;

        [JsonRequired]
        [JsonPropertyName("versicherungsnummer")]
        public new string Versicherungsnummer { get; set; } = string.Empty;

        [JsonRequired]
        [JsonPropertyName("beginnVersicherung")]
        public new DateTime BeginnVersicherung { get; set; }

        // personal data
        [JsonRequired]
        [JsonPropertyName("geschlecht")]
        public new string Geschlecht { get; set; } = string.Empty;

        [JsonPropertyName("titel")]
        public new string Titel { get; set; } = string.Empty;

        [JsonRequired]
        [JsonPropertyName("geburtsdatum")]
        public new DateTime Geburtsdatum { get; set; }

        [JsonRequired]
        [JsonPropertyName("vorname")]
        public new string Vorname { get; set; } = string.Empty;

        [JsonRequired]
        [JsonPropertyName("nachname")]
        public new string Nachname { get; set; } = string.Empty;

        [JsonPropertyName("geburtsname")]
        public new string Geburtsname { get; set; } = string.Empty;

        [JsonRequired]
        [JsonPropertyName("str")]
        public new string Str { get; set; } = string.Empty;

        [JsonRequired]
        [JsonPropertyName("hausnummer")]
        public new string Hausnummer { get; set; } = string.Empty;

        [JsonRequired]
        [JsonPropertyName("plz")]
        public new string Plz { get; set; } = string.Empty;

        [JsonRequired]
        [JsonPropertyName("wohnort")]
        public new string Wohnort { get; set; } = string.Empty;

        [JsonPropertyName("ortsteil")]
        public new string Ortsteil { get; set; } = string.Empty;

        [JsonPropertyName("geburtsort")]
        public new string Geburtsort { get; set; } = string.Empty;

        // information about offense
        [JsonRequired]
        [JsonPropertyName("aufforderungsdatum")]
        public new DateTime Aufforderungsdatum { get; set; }

        [JsonRequired]
        [JsonPropertyName("beginnRueckstand")]
        public new DateTime BeginnRueckstand { get; set; }

        [JsonRequired]
        [JsonPropertyName("verzugBis")]
        public new DateTime VerzugBis { get; set; }

        [JsonPropertyName("verzugsende")]
        public new DateTime? Verzugsende { get; set; }

        [JsonRequired]
        [JsonPropertyName("beitragsrueckstand")]
        public new string Beitragsrueckstand { get; set; } = string.Empty;

        [JsonRequired]
        [JsonPropertyName("sollbeitrag")]
        public new string Sollbeitrag { get; set; } = string.Empty;
        
        [JsonRequired]
        [JsonPropertyName("folgemeldung")]
        public new int Folgemeldung { get; set; }
        
        // additional information
        [JsonPropertyName("bemerkungen")]
        public new string Bemerkungen { get; set; } = string.Empty; 

        // documents
        [JsonPropertyName("anhoerungsdatum")]
        public new DateTime? Anhoerungsdatum { get; set; }

        /**************** helper functions ****************/

        /// <summary>
        /// Validates a JsonOffenseRecord object and returns a list of validation errors.
        /// </summary>
        /// <param name="record">The JsonOffenseRecord object to validate.</param>
        /// <returns>A list of validation errors. An empty list indicates that the record is valid.</returns>
        /// //TODO add validation for all fields like in frontend
        internal static List<string> ValidateRecord(JsonOffenseRecord record)
        {
            var errors = new List<string>();

            // if (string.IsNullOrEmpty(record.Weiserzeichen))                        
            //     errors.Add("Weiserzeichen wird benötigt.");
            if (record.Meldedatum == DateTime.MinValue)                         
                errors.Add("Meldedatum wird benötigt.");

            if (string.IsNullOrEmpty(record.Versicherungsunternehmensnummer))   
                errors.Add("Versicherungsunternehmensnummer wird benötigt.");
            if (string.IsNullOrEmpty(record.Krankenversicherung))               
                errors.Add("Krankenversicherung wird benötigt.");
            if (string.IsNullOrEmpty(record.Versicherungsnummer))               
                errors.Add("Versicherungsnummer wird benötigt.");
            if (record.BeginnVersicherung == DateTime.MinValue)                       
                errors.Add("Beginn Versicherung wird benötigt.");

            if (string.IsNullOrEmpty(record.Geschlecht))
                errors.Add("Geschlecht wird benötigt.");
            if (record.Geburtsdatum == DateTime.MinValue)                       
                errors.Add("Geburtsdatum wird benötigt.");
            if (string.IsNullOrEmpty(record.Vorname))                           
                errors.Add("Vorname wird benötigt.");
            if (string.IsNullOrEmpty(record.Nachname))                          
                errors.Add("Nachname wird benötigt.");
            if (string.IsNullOrEmpty(record.Str))                               
                errors.Add("Straße wird benötigt.");

            if (string.IsNullOrEmpty(record.Hausnummer))                        
                errors.Add("Hausnummer wird benötigt.");

            if (string.IsNullOrEmpty(record.Plz))                               
                errors.Add("PLZ wird benötigt.");
            if (string.IsNullOrEmpty(record.Wohnort))                           
                errors.Add("Wohnort wird benötigt.");

            if (record.Aufforderungsdatum == DateTime.MinValue)                 
                errors.Add("Aufforderungsdatum wird benötigt.");
            if (record.BeginnRueckstand == DateTime.MinValue)                         
                errors.Add("Beginn Rückstand wird benötigt.");

            if (string.IsNullOrEmpty(record.Beitragsrueckstand))                        
                errors.Add("Beitragsrückstand wird benötigt.");
            if (string.IsNullOrEmpty(record.Sollbeitrag))                        
                errors.Add("Sollbeitrag wird benötigt.");

            if (record.Folgemeldung < 1)                                    
                errors.Add("Es muss mindestens eine Folgemeldung existieren.");

            return errors;
        }
    }
}
