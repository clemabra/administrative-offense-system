using CsvHelper.Configuration.Attributes;

namespace src.Models.Offenses
{
	public class CSVOffenseRecord
	{	
		// TODO: Anything missing in csv is either defaulted or set to "(importiert)".
		/**** Weiserzeichen -> not in CSV ****/
		/**** Fallnummer -> generated ****/

		/**** Meldedatum -> MELDEDATUM ****/
		[Name("MELDEDATUM")]
        public required string MeldeDatumCsv { get; set; }
        
		/**** Versicherungsunternehmensnummer -> VU_ID ****/
        [Name("VU_ID")]
        public required string VU_ID { get; set; }
        
		/**** conversion not yet implemented -> defaults to "(importiert)" ****/

		/**** Versicherungsnummer -> VERSNR ****/
        [Name("VERSNR")]
        public required string VersNr { get; set; }
        
		/**** BeginnVersicherung -> VERS_BEGINN ****/
        [Name("VERS_BEGINN")]
        public required string VersBeginnCsv { get; set; }

		/**** Geschlecht -> ANREDE ****/
        [Name("Anrede")]
        public required string Anrede { get; set; } // 1-3 only
        
		/**** Titel -> not in CSV ****/
		// [Name("TITEL")]
		// public string? Titel { get; set; } = string.Empty;

		/**** Nachname -> NAME ****/
        [Name("Name")]
        public required string Name { get; set; }   // Maps to OffenseRecord.Nachname
        
		/**** Vorname -> VORNAME ****/
        [Name("Vorname")]
        public required string Vorname { get; set; }
        
		/**** plz -> PLZ ****/
        [Name("PLZ")]
        public required string Plz { get; set; }
        
		/**** Wohnort -> ORT ****/
        [Name("ORT")]
        public required string Ort { get; set; }
        
		/**** Str -> STRASSE (street part) ****/
		/**** Hausnummer -> STRASSE (hourse number part) ****/
        [Name("STRASSE")]
        public required string Strasse { get; set; }
        
		/**** Geburtsdatum -> GEBDATUM ****/
        [Name("GEBDATUM")]
        public required string GebDatumCsv { get; set; }
        
		/**** Aufforderungsdatum -> AUF_DATUM ****/
        [Name("AUF_DATUM")]
        public required string AufDatumCsv { get; set; }
        
		/**** BeginnRueckstand -> BEGINN_RUECKSTAND ****/
        [Name("BEGINN_RUECKS")]
        public required string BeginnRuecksCsv { get; set; }

		/**** VerzugBis -> computed ****/
		/**** Verzugsende -> not in CSV, not required anymore ****/
        
		/**** Beitragsrueckstand -> BEITRAGSRUECKSTAND ****/
        [Name("BEITRAGSRUECKS")]
        public required string Beitragsruecks { get; set; }
        
		/**** Sollbeitrag -> PPV_SOLLBEITRAG ****/
        [Name("PPV_SOLLBEITRAG")]
        public required string PpvSollbeitrag { get; set; }
        
		/**** Folgemeldung -> FOLGEMELDUNG ****/
		// string, but before going into database, is converted to int
        [Name("FOLGEMELDUNG")]
        public required string FolgemeldungCsv { get; set; }
        
		/**** Bemerkungen -> not in CSV (set string.Empty) ****/
		/**** Anhoerungsdatum -> not in CSV (nullable) ****/
	}
}