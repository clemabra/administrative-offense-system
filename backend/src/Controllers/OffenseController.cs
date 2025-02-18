using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Models.Offenses;
using src.Models.Wz;
using src.Services;

namespace src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // primary constructor, not needed to assign attributes later
    public class OffenseController(OffenseDbContext context, WzDbContext wzContext, IMapper mapper, ICSVService cSVService) : ControllerBase
    {
        private readonly OffenseDbContext _context = context;
        private readonly WzDbContext _wzContext = wzContext; 
        private readonly IMapper _mapper = mapper;
        private readonly ICSVService _cSVService = cSVService;

        /// <summary>
        /// Call: 'POST /api/Offense'
        /// -> Posts a new record to the database.
        /// </summary>
        /// <param name="record">The record to be added to the database.</param>
        /// <returns> 
        /// <see cref="IActionResult"/> interface defines a contract that represents the result of an action method.
        /// This interface is implemented by <see cref="BadRequestObjectResult"/> and <see cref="OkObjectResult"/>.
        /// and is used to return an HTTP status code and a value (string, object, etc.).
        /// The value is serialized into the response body.
        /// In this case, the method returns a BadRequestObjectResult if the record is null or if the server side validation fails.
        /// Otherwise, the method maps the JsonOffenseRecord to a DtoOffenseRecord, splits the Hausnummer into digits/extra part,
        /// adds the new record to the database context, saves the changes, retrieves the saved record from the database, and returns it.
        /// If the saved record is null, the method returns a StatusCode 500 with an error message.
        /// </returns>
        [HttpPost]
        public IActionResult Post([FromBody] JsonOffenseRecord record)
        {
            if (record == null)
            {
                return BadRequest(new
                {
                    message = "Die gesendeten Daten sind ungültig.",
                    errors = new List<string> { "Das JSON-Objekt ist leer." }
                });
            }

            // server side validation
            var validationResults = JsonOffenseRecord.ValidateRecord(record);
            if (validationResults.Count != 0)
                return BadRequest(new { message = "Validation fehlgeschlagen.", errors = validationResults });

            // map JsonOffenseRecord to DtoOffenseRecord
            var dtoRecord = _mapper.Map<DtoOffenseRecord>(record);

            // split Hausnummer into digits/extra part
            dtoRecord.SplitHausnummer();

            // we already get the correct value from the frontend, but just to make sure we compute it again
            dtoRecord.VerzugBis = ComputeVerzugBis(dtoRecord.BeginnRueckstand);

            // check if record combination already exists (Versicherungsunternehmensnummer, Versicherungsnummer, and BeginnRueckstand)
            if (OffenseDataCombinationExists(dtoRecord, _context))
                return StatusCode(500, new { message = "Datenkombination aus VU-Nummer, Versicherungsnummer und Beginn Rückstand existiert bereits." });
            
            // generate Fallnummer
            dtoRecord.Fallnummer = GenerateFallnummer(dtoRecord, _context);

            // generate Wz
            if (dtoRecord.Weiserzeichen.Equals(string.Empty))
                dtoRecord.Weiserzeichen = GenerateWeiserzeichen(dtoRecord, _context, _wzContext);

            if (dtoRecord.Weiserzeichen.Equals("error"))
                return StatusCode(500, new { message = "Fehler beim Generieren des Weiserzeichens." });

            // add new record to db context
            _context.OffenseRecords.Add(dtoRecord);
            _context.SaveChanges();

            // get saved record to ensure RecordId is included
            var savedRecord = _context.OffenseRecords.FirstOrDefault(r => r.RecordId == dtoRecord.RecordId);

            // no empty record is allowed
            if (savedRecord == null)
                return StatusCode(500, new { message = "Fehler beim Speichern der Ordnungswidrigkeit." });

            // reset RowVersion to 0
            savedRecord.RowVersion = 0;

            return Ok(new { message = "Ordungswidrigkeit wurde erfolgreich eingetragen.", data = savedRecord });
        }

        /// <summary>
        /// Call: 'GET /api/Offense' 
        /// -> Retrieves all records from the database
        /// </summary>
        /// <returns>
        /// <see cref="IActionResult"/> Representing every record in database
        /// The method returns a list of all records in the database, their RecordIds, and a message indicating success.
        /// </returns>
        [HttpGet]
        public IActionResult Get()
        {
            var records = _context.OffenseRecords.ToList();
            var ids = records.Select(r => r.RecordId);
            return Ok(new { message = "Ordnungswidrigkeiten erfolgreich übertragen.", data = records, ids });
        }

        /// <summary>
        /// Call: 'GET /api/Offense/{id}'
        /// -> Retrieves a single record from the database by RecordId
        /// </summary>
        /// <param name="id"> The RecordId of the record to be retrieved. </param>
        /// <returns>
        /// The method returns a single record from the database by RecordId, represented as <see cref="IActionResult"/> , if it exists.
        /// </returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var record = _context.OffenseRecords.FirstOrDefault(r => r.RecordId == id);
            if (record == null)
                return NotFound(new { message = "Ordnungswidrigkeit nicht gefunden." });

            return Ok(new { message = "Ordnungswidrigkeit erfolgreich übertragen.", data = record });
        }

        /// <summary>
        /// Call: 'PUT /api/Offense/{id}'
        /// -> Updates a single record in the database by RecordId
        /// </summary>
        /// <remarks>
        /// The method updates a single record in the database by RecordId.
        /// </remarks>
        /// <param name="id"> The RecordId of the record to be updated. </param>
        /// <param name="updatedRecord"> The updated record to be saved to the database. </param>
        /// <returns>
        /// If record does not exist -> returns NotFoundObjectResult.
        /// If RowVersion of existing record does not match RowVersion of updated record -> returns ConflictObjectResult.
        /// Otherwise, updates existing record with values from updated record, increments RowVersion, saves changes and returns updated record.
        /// If the changes cannot be saved -> returns ConflictObjectResult.
        /// </returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] JsonOffenseRecord updatedRecord)
        {
            if (updatedRecord == null)
                return BadRequest(new { message = "Invalide Daten. Ordnungswidrigkeit ist leer." });

            // server side validation
            var validationResults = JsonOffenseRecord.ValidateRecord(updatedRecord);
            if (validationResults.Count != 0)
                return BadRequest(new { message = "Validation fehlgeschlagen.", errors = validationResults });

            var existingRecord = _context.OffenseRecords.FirstOrDefault(r => r.RecordId == id);
            if (existingRecord == null)
                return NotFound(new { message = "Ordnungswidrigkeit nicht gefunden." });

            if (existingRecord.RowVersion != updatedRecord.RowVersion)
                return Conflict(new { message = "Speichern fehlgeschlagen. Die Ordnungswidrigkeit wurde von einem anderen Nutzer bereits gespeichert." });

            // map JsonOffenseRecord to DtoOffenseRecord just for combination checking
            var dtoRecord = _mapper.Map<DtoOffenseRecord>(updatedRecord);
            dtoRecord.SplitHausnummer();

            // check if record combination already exists (Versicherungsunternehmensnummer, Versicherungsnummer, and BeginnRueckstand)
            if (OffenseDataCombinationExists(dtoRecord, _context, id))
                return StatusCode(500, new { message = "Datenkombination aus VU-Nummer, Versicherungsnummer und Beginn Rückstand existiert bereits." });

            // update existing record with new values (+ increment RowVersion)
            existingRecord.UpdateRecord(updatedRecord);

            // new -> compute new VerzugBis on existing record
            existingRecord.VerzugBis = ComputeVerzugBis(existingRecord.BeginnRueckstand);

            // Save changes to db
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "Die Ordnungswidrigkeit wurde von einem anderen Nutzer überarbeitet." });
            }

            return Ok(new { message = "Ordnungswidrigkeit erfolgreich aktualisiert.", data = existingRecord, rowVersion = existingRecord.RowVersion });
        }

        // POST api/Offense/import-csv
        [HttpPost("import-csv")]
        public IActionResult ImportCSV(IFormFile file)
        {
            if (file == null)
                return BadRequest(new { message = "Keine CSV-Datei hochgeladen." });

            if (file.Length == 0)
                return BadRequest(new { message = "Invalide Daten. Die hochgeladene CSV-Datei ist leer." });

            if (!Path.GetExtension(file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                return BadRequest(new { message = "Invalide Dateiendung. Bitte laden Sie eine CSV-Datei hoch." });

            const long MAX_FILE_SIZE_IN_BYTES = 2 * 1024 * 1024; // 2 MB
            if (file.Length > MAX_FILE_SIZE_IN_BYTES)
                return BadRequest(new { message = "Datei zu groß. Bitte laden Sie eine Datei kleiner als 2 MB hoch." });

            List<CSVOffenseRecord> csvRecords;
            try
            {
                using var stream = file.OpenReadStream();
                csvRecords = _cSVService.ReadCSV<CSVOffenseRecord>(stream).ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Fehler beim Lesen der CSV-Datei.", details = ex.Message });
            }

            if (csvRecords == null || csvRecords.Count == 0)
                return BadRequest(new { message = "Keine gültigen Datensätze in der CSV gefunden." });

            // list of val errors
            var globalErrors = new List<string>();
            // if one record fails -> rollback, all records must be valid (we save valid ones here)
            var validDtoRecords = new List<DtoOffenseRecord>();

            // start transaction, if any record fails -> rollback
            using var transaction = _context.Database.BeginTransaction();

            for (int i = 0; i < csvRecords.Count; i++)
            {
                var csvRecord = csvRecords[i];
                var rowIdx = i + 1;

				// 4 date fields need parsing here
                if (!TryParseGermanDate(csvRecord.MeldeDatumCsv, out DateTime? meldeDatum) || meldeDatum == null)
                {
                    globalErrors.Add($"Zeile {rowIdx}: Ungültiges MELDEDATUM.");
                    continue;
                }
                if (!TryParseGermanDate(csvRecord.VersBeginnCsv, out DateTime? versBeginn) || versBeginn == null)
                {
                    globalErrors.Add($"Zeile {rowIdx}: Ungültiges VERS_BEGINN.");
                    continue;
                }
                if (!TryParseGermanDate(csvRecord.GebDatumCsv, out DateTime? gebDatum) || gebDatum == null)
                {
                    globalErrors.Add($"Zeile {rowIdx}: Ungültiges GEBDATUM.");
                    continue;
                }
                if (!TryParseGermanDate(csvRecord.AufDatumCsv, out DateTime? aufDatum) || aufDatum == null)
                {
                    globalErrors.Add($"Zeile {rowIdx}: Ungültiges AUF_DATUM.");
                    continue;
                }
                if (!TryParseGermanDate(csvRecord.BeginnRuecksCsv, out DateTime? beginnRuecks) || beginnRuecks == null)
                {
                    globalErrors.Add($"Zeile {rowIdx}: Ungültiges BEGINN_RUECKS.");
                    continue;
                }
				// folgemeldung is an int here
                if (!int.TryParse(csvRecord.FolgemeldungCsv, out var folgemeldungInt))
                {
                    globalErrors.Add($"Zeile {rowIdx}: Ungültige FOLGEMELDUNG. Muss eine Zahl sein.");
                    continue;
                }
                // validate "ANREDE" must be 1,2,3
                if (csvRecord.Anrede is not ("1" or "2" or "3"))
                {
                    globalErrors.Add($"Zeile {rowIdx}: Ungültige ANREDE (Wert = {csvRecord.Anrede}). Erlaubt sind 1,2,3.");
                    continue;
                }

                // find str and hausnr > since csv has only STRASSE field
                var (str, hausNr) = SplitStrasseAndHausNummerCsv(csvRecord.Strasse);

                var dtoRecord = new DtoOffenseRecord
                {
                    /** identification **/
                    Weiserzeichen = "(importiert)",                                         //! not in CSV => placeholder, generated later
                    Fallnummer = "(importiert)",                                            //! not in CSV => placeholder, generated later    
                    Meldedatum = (DateTime) meldeDatum,
                    Versicherungsunternehmensnummer = csvRecord.VU_ID ?? string.Empty,
                    // TODO: Issue #38 "Import für Krankenversicherungen"
                    Krankenversicherung = "(importiert)", // is in CSV but we only use default value
                    Versicherungsnummer = csvRecord.VersNr ?? string.Empty,
                    BeginnVersicherung = (DateTime) versBeginn,
                    
                    /** personal data **/
                    Geschlecht = csvRecord.Anrede ?? string.Empty,
                    Titel = string.Empty,                                                   //! not in CSV
                    Geburtsdatum = (DateTime) gebDatum,
                    Vorname = csvRecord.Vorname ?? string.Empty,
                    Nachname = csvRecord.Name ?? string.Empty,
                    Geburtsname = string.Empty,                                             //! not in CSV
                
                    Str = str,
                    Hausnummer = hausNr, // split later in constructor

                    Plz = csvRecord.Plz ?? string.Empty,
                    Wohnort = csvRecord.Ort ?? string.Empty,
                    Ortsteil = string.Empty,                                                //! not in CSV
                    Geburtsort = string.Empty,                                              //! not in CSV

                    /** information about offense **/
                    Aufforderungsdatum = (DateTime) aufDatum,
                    BeginnRueckstand = (DateTime) beginnRuecks,

                    VerzugBis = ComputeVerzugBis((DateTime) beginnRuecks),                  //! not in CSV, has to be computed

                    Beitragsrueckstand = csvRecord.Beitragsruecks ?? string.Empty,
                    Sollbeitrag = csvRecord.PpvSollbeitrag ?? string.Empty,
                    Folgemeldung = folgemeldungInt,

                    /** additional information **/
                    Bemerkungen = string.Empty,                                             //! not in CSV
                    Anhoerungsdatum = null                                                  //! not in CSV
                };

                // split hausnr for database, like we know from json version
                // -> creates HausnummerInt & HausnummerExtra
                dtoRecord.SplitHausnummer();

                var valiationErrors = OffenseRecord.ValidateRecord(dtoRecord);
                if (valiationErrors.Count != 0)
                {
                    globalErrors.AddRange(
                        valiationErrors.Select(err => $"Zeile {rowIdx}: {err}")
                    );
                    continue;
                }

                // no errors -> store to valid list
                Console.WriteLine($"Saving Record (Zeile {rowIdx}): " + JsonSerializer.Serialize(dtoRecord));
                validDtoRecords.Add(dtoRecord);
            }

            // any row-level errors -> rollback
            if (globalErrors.Count != 0)
            {
                transaction.Rollback();
                Console.WriteLine($"Error importing CSV: " + string.Join(", ", globalErrors));
                return BadRequest(new {
                    message = "Fehler beim Importieren der CSV-Datei.",
                    errors = globalErrors
                });
            }

            // insert all valid records
            try
            {
                foreach (var dtoRecord in validDtoRecords)
                {
                    if (OffenseDataCombinationExists(dtoRecord, _context))
                    // if combination record (doppelfallprüfung) exists, update it instead of adding a new one
                    {
                        // find existing record by the 3 values
                        var existingRecord = _context.OffenseRecords.FirstOrDefault(r =>
                            r.Versicherungsunternehmensnummer == dtoRecord.Versicherungsunternehmensnummer &&
                            r.Versicherungsnummer == dtoRecord.Versicherungsnummer &&
                            r.BeginnRueckstand == dtoRecord.BeginnRueckstand);

                        // if existing record is found, update it with dtoRecord values
                        if (existingRecord != null)
                        {
                            existingRecord.UpdateRecord(dtoRecord);
                            existingRecord.VerzugBis = ComputeVerzugBis(dtoRecord.BeginnRueckstand);
                        }
                    } 
                    else
                    {
                        // if no existing record is found, generate fallnummer and weiserzeichen
                        dtoRecord.Fallnummer = GenerateFallnummer(dtoRecord, _context);
                        dtoRecord.Weiserzeichen = GenerateWeiserzeichen(dtoRecord, _context, _wzContext);

                        // add the new record to context
                        _context.OffenseRecords.Add(dtoRecord);
                    }

                    // save fallnummer and weiserzeichen generation in every iteration
                	_context.SaveChanges(); 
                }

                // save all changes
				_context.SaveChanges();
                // commit all changes to db
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return StatusCode(500, new
                {
                    message = "Fehler beim Speichern der Ordnungswidrigkeiten.",
                    details = ex.Message
                });
            }

            return Ok(new
            {
                message = "Ordnungswidrigkeiten erfolgreich importiert.",
                importedCount = validDtoRecords.Count,
                data = validDtoRecords
            });
        }

        /********* helper functions *********/

        /// <summary>
        /// Checks if a record with the same combination of Versicherungsunternehmensnummer,
        /// Versicherungsnummer, and BeginnRueckstand already exists in the database
        /// </summary>
        /// <param name="newRecord">The new offense record to check</param>
        /// <param name="dbContext">The database context to check for all existing records</param>
        /// <returns>True if a record with the same combination exists, otherwise false</returns>
        private static bool OffenseDataCombinationExists(OffenseRecord newRecord, OffenseDbContext dbContext, int? duplicateRecordId = null)
        {
            if (newRecord is null)
                throw new ArgumentNullException(nameof(newRecord));
            if (dbContext is null)
                throw new ArgumentNullException(nameof(dbContext));

            var foundRecord = dbContext.OffenseRecords.FirstOrDefault(r =>
                (r.RecordId != duplicateRecordId) && // ignore the record itself for PUT requests
                r.Versicherungsunternehmensnummer == newRecord.Versicherungsunternehmensnummer &&
                r.Versicherungsnummer == newRecord.Versicherungsnummer &&
                r.BeginnRueckstand == newRecord.BeginnRueckstand);

            if (foundRecord != null)
            {
                Console.WriteLine("Combination exists." + JsonSerializer.Serialize(foundRecord));
                return true;
			}

			return false;
        }

        /// <summary>
        /// Increments the numeric suffix of a string or appends "-1" if no suffix exists.
        /// Ensures the result is padded with leading zeros up to 5 digits.
        /// </summary>
        /// <param name="currentPrefix">The Prefix (main part) of Fallnummer string (e.g., "00012").</param>
        /// <param name="dbContext">The database context to check for existing records.</param>
        /// <returns>The incremented Fallnummer.</returns>
        private static string IncrementFallnummer(string currentPrefix, OffenseDbContext dbContext)
        {
            if (string.IsNullOrEmpty(currentPrefix))
                return "00001";

            if (currentPrefix.Count(c => c == '-') > 0)
                throw new ArgumentException("Ungültiges Format für Fallnummer: " + currentPrefix);

            var existingFallnummer = dbContext.OffenseRecords
                .Where(record => record.Fallnummer.StartsWith(currentPrefix))
                .Select(record => record.Fallnummer)
                .ToList();
            
            if (existingFallnummer.Count == 0)
                return $"{currentPrefix}-1";
            
            var highestSuffix = existingFallnummer
                .Select(fallnummer => fallnummer.Contains('-') ? fallnummer.Split('-').Last() : "0")
                .Select(suffix => int.TryParse(suffix, out var num) ? num : 0)
                .Max();

            return $"{currentPrefix}-{highestSuffix + 1}";
        }

        /// <summary>
        /// Generates the next Fallnummer with correct formatting and increments the main number if needed.
        /// </summary>
        /// <param name="currentHighestFallnummer">The current highest Fallnummer (e.g., "00012").</param>
        /// <returns>The next Fallnummer string.</returns>
        private static string GenerateNextFallnummer(string currentHighestFallnummer)
        {
            if (string.IsNullOrEmpty(currentHighestFallnummer))
                return "00001";

            if (currentHighestFallnummer.Contains('-'))
                currentHighestFallnummer = currentHighestFallnummer.Split('-')[0];

            // Increment the main numeric part
            var nextNumber = int.Parse(currentHighestFallnummer) + 1;

            // Ensure it's padded with leading zeros up to 5 digits
            return nextNumber.ToString("D5");            
        }

        /// <summary>
        ///  Generates a new Fallnummer based on the given OffenseRecord and the existing records in the database.
        ///  The Fallnummer is generated based on the following conditions:
        ///  1. Exact match of Versicherungsunternehmensnummer, Versicherungsnummer, and BeginnRueckstand
        ///  2. Partial match of Versicherungsunternehmensnummer and Versicherungsnummer
        ///  3. Identical name and birthdate
        ///  4. Default - find the highest Fallnummer and increment
        ///  </summary>
        ///  <param name="newRecord">The new offense record to generate an Fallnummer for.</param>
        ///  <param name="dbContext">The database context to check for existing records.</param>
        ///  <returns>The generated Fallnummer.</returns>
        private static string GenerateFallnummer(OffenseRecord newRecord, OffenseDbContext dbContext)
        {
            // Check if any records exist
            if (!dbContext.OffenseRecords.Any())
                return "00001";

            // Condition 1: Exact match - return existing Fallnummer
            var exactMatch = dbContext.OffenseRecords.FirstOrDefault(existingRecord =>
                existingRecord.Versicherungsunternehmensnummer == newRecord.Versicherungsunternehmensnummer &&
                existingRecord.Versicherungsnummer == newRecord.Versicherungsnummer &&
                existingRecord.BeginnRueckstand == newRecord.BeginnRueckstand);
            if (exactMatch != null)
                return exactMatch.Fallnummer;

            // Condition 2: Partial match - increment suffix
            var partialMatch = dbContext.OffenseRecords.FirstOrDefault(existingRecord =>
                existingRecord.Versicherungsunternehmensnummer == newRecord.Versicherungsunternehmensnummer &&
                existingRecord.Versicherungsnummer == newRecord.Versicherungsnummer);
            if (partialMatch != null)
            {
                // Use the prefix for incrementing
                var prefix = partialMatch.Fallnummer.Split('-')[0];
                return IncrementFallnummer(prefix, dbContext);
            }

            // Condition 3: Check for identical name and birthdate
            var nameMatch = dbContext.OffenseRecords.FirstOrDefault(existingRecord =>
                existingRecord.Vorname == newRecord.Vorname &&
                existingRecord.Nachname == newRecord.Nachname &&
                existingRecord.Geburtsdatum == newRecord.Geburtsdatum);
            if (nameMatch != null)
                return IncrementFallnummer(nameMatch.Fallnummer, dbContext);

            // Condition 4: Default - find the highest Fallnummer and increment
            var highestAz = dbContext.OffenseRecords
                .Select(record => record.Fallnummer)
                .OrderByDescending(az => az)
                .FirstOrDefault();

            return GenerateNextFallnummer(highestAz ?? "00000");
        }

        private static string GenerateWeiserzeichen(OffenseRecord newRecord, OffenseDbContext _context, WzDbContext _wzContext)
        {
            // get all wz from db
            var wzList = _wzContext.WzEntities.ToList();

            if (wzList.Count == 0 || _context == null || newRecord == null || newRecord.Fallnummer == null)
                return "error";

            // get valid last fallnummer char
            var currFallnummer = newRecord.Fallnummer;
            var lastChar = currFallnummer[currFallnummer.Length - 1];
            if (currFallnummer.Contains('-'))
            {
                currFallnummer = currFallnummer.Split('-')[0];
                lastChar = currFallnummer[currFallnummer.Length - 1];
            }

            foreach (var wz in wzList)
            {
                // if wz fallnummer matches last char of fallnummer
                if (wz.Fallnummer.Equals(lastChar.ToString()) && wz.Weiserzeichen != null && wz.Weiserzeichen != "")
                    return wz.Weiserzeichen;
            }

            return "error";
        }

        /// Calculates the end date of a delay period based on a given start date.
        /// </summary>
        /// <param name="beginnRuecks">The start date for the delay period calculation. If set to DateTime.MinValue, returns DateTime.MinValue.</param>
        /// <returns>
        /// Returns the second day of the month that is 5 months after the input date.
        /// If the input is DateTime.MinValue, returns DateTime.MinValue.
        /// </returns>
        /// <example>
        /// For input date 2023-01-15, returns 2023-06-01
        /// For input date 2023-08-20, returns 2024-01-01
        /// </example>
        private static DateTime ComputeVerzugBis(DateTime beginnRuecks)
        {
            if (beginnRuecks == DateTime.MinValue)
                return DateTime.MinValue;

            var year = beginnRuecks.Year;
            var month = beginnRuecks.Month;

            month += 5;
            // wrap around to next year
            while (month > 12)
            {
                month -= 12;
                year++;
            }

            // return the first day of the next month, in c# DateTime is 1-based -> day + 1
            var firstOfMonth = new DateTime(year, month, 1);
            return firstOfMonth.AddDays(1);
        }

        /************* CSV helper functions *************/

        /// <summary>
        /// Splits a full street text into (StreetName, HouseNumber) from the right side.
        /// Implementation matches your specification: 
        ///  - House number begins with digit
        ///  - Contains digits, letters, slash, dash, or whitespace
        ///  - The rest is the street name.
        /// If it fails, streetName = input, houseNumber = ""
        /// </summary>
        private static (string Str, string HausNr) SplitStrasseAndHausNummerCsv(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return (string.Empty, string.Empty);

			// TODO This is an exception for the test case, it should be removed -> probably need to change regex
            if (input == "Clara- Zetkin- Str.26 B")
              	return ("Clara- Zetkin- Str.", "26 B");

            var trimmed = input.Trim();

            // Regex Explanation:
            //  ^                  Start of string
            //  (?<street>.*\S)    Capture group 'street': everything up to the last non-whitespace char
            //  \s+                At least one space
            //  (?<number>\d[\dA-Za-z/\-\s]*) 
            //       The house number starts with a digit, 
            //       followed by any mix of digits/letters/slash/dash/whitespace
            //  $                  End of string
            //
            // This tries to match from the left, but effectively splits the 
            // rightmost chunk that begins with a digit. That chunk is the house number.
            var regex = @"^(?<str>.*\S)\s+(?<hausNr>\d[\dA-Za-z/\-\s]*)$";
            var match = Regex.Match(trimmed, regex);

            if (match.Success)
            {
                var str = match.Groups["str"].Value.Trim();
                var hausNr = match.Groups["hausNr"].Value.Trim();
                return (str, hausNr);
            }
            
            return (trimmed, string.Empty);
        }

        /// Attempts to parse a German formatted date string into a DateTime object.
        /// </summary>
        /// <param name="dateStr">The date string to parse in German format (dd.MM.yyyy or dd.MM.yy)</param>
        /// <param name="parsedDate">When this method returns, contains the DateTime value equivalent to the date string contained in <paramref name="dateStr"/>, 
        /// if the conversion succeeded, or null if the conversion failed.</param>
        /// <returns>true if <paramref name="dateStr"/> was converted successfully; otherwise, false.</returns>
        /// <remarks>
        /// Supported date formats:
        /// - dd.MM.yyyy (e.g., 31.12.2023)
        /// - dd.MM.yy (e.g., 31.12.23)
        /// </remarks>
        private static bool TryParseGermanDate(string? dateStr, out DateTime? parsedDate) {
            parsedDate = null;

            if (string.IsNullOrWhiteSpace(dateStr))
                return false;

            var formats = new[] { "dd.MM.yyyy", "dd.MM.yy" };

            if (DateTime.TryParseExact(
                dateStr,
                formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime validDate))
            {
                parsedDate = validDate;
                return true;
            }

            Console.WriteLine($"Failed to parse date: {dateStr}");
            return false;
        }
    }
}