
using System.Text.RegularExpressions;

namespace src.Models.Offenses
{
    public class OffenseRecord
    {
        // identification properties
        public int RecordId { get; set; }
        public int RowVersion { get; set; }
        public string Weiserzeichen { get; set; } = string.Empty;
        public string Fallnummer { get; set; } = string.Empty;
        public DateTime Meldedatum { get; set; } = DateTime.MinValue;

        // insurance data
        public string Versicherungsunternehmensnummer { get; set; } = string.Empty;
        public string Krankenversicherung { get; set; } = string.Empty;
        public string Versicherungsnummer { get; set; } = string.Empty;
        public DateTime BeginnVersicherung { get; set; } = DateTime.MinValue;

        // personal data
        public string Geschlecht { get; set; } = string.Empty;
        public string Titel { get; set; } = string.Empty;
        public DateTime Geburtsdatum { get; set; } = DateTime.MinValue;
        public string Vorname { get; set; } = string.Empty;
        public string Nachname { get; set; } = string.Empty;
        public string Geburtsname { get; set; } = string.Empty;
        public string Str { get; set; } = string.Empty;
        public string Hausnummer { get; set; } = string.Empty;
        public string Plz { get; set; } = string.Empty;
        public string Wohnort { get; set; } = string.Empty;
        public string Ortsteil { get; set; } = string.Empty;
        public string Geburtsort { get; set; } = string.Empty;

        // information about offense
        public DateTime Aufforderungsdatum { get; set; } = DateTime.MinValue;
        public DateTime BeginnRueckstand { get; set; } = DateTime.MinValue;
        public DateTime VerzugBis { get; set; } = DateTime.MinValue;
        public DateTime? Verzugsende { get; set; }
        public string Beitragsrueckstand { get; set; } = string.Empty;
        public string Sollbeitrag { get; set; } = string.Empty;
        public int Folgemeldung { get; set; } = 0;
        
        // additional information
        public string Bemerkungen { get; set; } = string.Empty;

        // external documents
        public DateTime? Anhoerungsdatum { get; set; }

        /******* helper functions *******/

		/// <summary>
		/// Validates an OffenseRecord (primarily read from CSV)
		/// and returns a list of validation errors.
		/// An empty list indicates that the record is valid.
		/// </summary>
		/// <param name="record">The OffenseRecord object to validate.</param>
		/// <returns>A list of validation errors.</returns>
		internal static List<string> ValidateRecord(OffenseRecord record)
		{
			var errors = new List<string>();

			if (string.IsNullOrWhiteSpace(record.Weiserzeichen))
				errors.Add("Weiserzeichen wird benötigt.");

			if (string.IsNullOrWhiteSpace(record.Fallnummer))
				errors.Add("Fallnummer wird benötigt.");

			if (record.Meldedatum == DateTime.MinValue)
				errors.Add("Meldedatum wird benötigt oder ist im falschen Format.");

			if (string.IsNullOrWhiteSpace(record.Versicherungsunternehmensnummer))
				errors.Add("Versicherungsunternehmensnummer wird benötigt.");

			if (string.IsNullOrWhiteSpace(record.Krankenversicherung))
				errors.Add("Krankenversicherung wird benötigt.");

			if (string.IsNullOrWhiteSpace(record.Versicherungsnummer))
				errors.Add("Versicherungsnummer wird benötigt.");

			if (record.BeginnVersicherung == DateTime.MinValue)
				errors.Add("BeginnVersicherung wird benötigt oder ist im falschen Format.");

			if (string.IsNullOrWhiteSpace(record.Geschlecht))
				errors.Add("Geschlecht wird benötigt.");

			if (record.Geburtsdatum == DateTime.MinValue)
				errors.Add("Geburtsdatum wird benötigt oder ist im falschen Format.");

			if (string.IsNullOrWhiteSpace(record.Vorname))
				errors.Add("Vorname wird benötigt.");

			if (string.IsNullOrWhiteSpace(record.Nachname))
				errors.Add("Nachname wird benötigt.");

			if (string.IsNullOrWhiteSpace(record.Str))
				errors.Add("Straße (Str) wird benötigt.");

			if (string.IsNullOrWhiteSpace(record.Hausnummer))
				errors.Add("Hausnummer wird benötigt. Was: " + record.Hausnummer);

			if (string.IsNullOrWhiteSpace(record.Plz))
				errors.Add("PLZ wird benötigt.");
			if (!Regex.IsMatch(record.Plz, @"^\d{5}$"))
				errors.Add("Die PLZ muss aus genau 5 Ziffern bestehen.");

			if (string.IsNullOrWhiteSpace(record.Wohnort))
				errors.Add("Wohnort wird benötigt.");

			if (record.Aufforderungsdatum == DateTime.MinValue)
				errors.Add("Aufforderungsdatum wird benötigt oder ist im falschen Format.");

			if (record.BeginnRueckstand == DateTime.MinValue)
				errors.Add("BeginnRueckstand wird benötigt oder ist im falschen Format.");

			if (record.VerzugBis == DateTime.MinValue)
				errors.Add("VerzugBis wird benötigt oder ist im falschen Format.");

			if (string.IsNullOrWhiteSpace(record.Beitragsrueckstand))
				errors.Add("Beitragsrückstand wird benötigt.");

			if (string.IsNullOrWhiteSpace(record.Sollbeitrag))
				errors.Add("Sollbeitrag wird benötigt.");

			if (string.IsNullOrWhiteSpace(record.Folgemeldung.ToString()))
				errors.Add("Folgemeldung wird benötigt.");
			if (record.Folgemeldung < 1)
				errors.Add("Der Wert für Folgemeldung muss mindestens 1 sein.");
			
			return errors;
		}
    }
}