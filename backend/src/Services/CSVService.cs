using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.Text;

namespace src.Services
{
    public class CSVService : ICSVService
    {
        public IEnumerable<T> ReadCSV<T>(Stream file)
        {
            using var reader = new StreamReader(file, Encoding.UTF8, true);

            // check for byte order mark
            var bom = reader.CurrentEncoding.GetPreamble();
            if (bom.Length == 0 || bom[0] != 0xEF || bom[1] != 0xBB || bom[2] != 0xBF)
            {
                throw new Exception("Die Datei muss eine Bytereihenfolgemarkierung enthalten.");
            }

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                IgnoreBlankLines = true, // ignore empty lines

                // disable following warnings --> get handled manually by us
                MissingFieldFound = null,  
                BadDataFound = null,
                HeaderValidated = null
            };

            using var csv = new CsvReader(reader, config);

            csv.Read();

            // read and validate headers
            csv.ReadHeader();
            var headers = csv.HeaderRecord;
            if (headers == null || headers.Length == 0)
            {
                throw new Exception("Die CSV-Datei enthält keine Kopfzeile.");
            }

            var requiredHeaders = new[] 
            {
                "VU_ID",
                "VERSNR",
                "MELDEDATUM",
                "VERS_BEGINN",
                "GEBDATUM",
                "AUF_DATUM",
                "BEGINN_RUECKS",
                "FOLGEMELDUNG",
                "Anrede",
                "Vorname",
                "Name",
                "STRASSE",
                "PLZ",
                "ORT",
                "BEITRAGSRUECKS",
                "PPV_SOLLBEITRAG"
            };
            var missingHeaders = requiredHeaders.Except(headers).ToList();
            if (missingHeaders.Count != 0)
            {
                throw new Exception($"Fehlende Pflichtspalten: {string.Join(", ", missingHeaders)}");
            }

            // validate read records
            var records = new List<T>();
            var rowNumber = 1; // header row is 1

            while (csv.Read())
            {
                rowNumber++;

                try
                {
                    // valiate required fields
                    foreach (var header in requiredHeaders)
                    {
                        if (string.IsNullOrWhiteSpace(csv.GetField(header)))
                            throw new Exception($"Fehlende Pflichtangabe in Zeile {rowNumber}: {header}");
                    }

                    // date format validation
                    if (!DateTime.TryParseExact(csv.GetField("MELDEDATUM"), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                        throw new Exception($"Ungültiges Datumsformat in Zeile {rowNumber}: MeldeDatumCsv");
                    if (!DateTime.TryParseExact(csv.GetField("VERS_BEGINN"), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                        throw new Exception($"Ungültiges Datumsformat in Zeile {rowNumber}: VersBeginnCsv");
                    if (!DateTime.TryParseExact(csv.GetField("GEBDATUM"), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                        throw new Exception($"Ungültiges Datumsformat in Zeile {rowNumber}: GebDatumCsv");
                    if (!DateTime.TryParseExact(csv.GetField("AUF_DATUM"), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                        throw new Exception($"Ungültiges Datumsformat in Zeile {rowNumber}: AufDatumCsv");
                    if (!DateTime.TryParseExact(csv.GetField("BEGINN_RUECKS"), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                        throw new Exception($"Ungültiges Datumsformat in Zeile {rowNumber}: BeginnRuecksCsv");

                    if (!int.TryParse(csv.GetField("FOLGEMELDUNG"), out _))
                        throw new Exception($"Ungültige FOLGEMELDUNG in Zeile {rowNumber}: Muss eine Zahl sein.");

                    var anrede = csv.GetField("Anrede");
                    if (anrede != "1" && anrede != "2" && anrede != "3")
                        throw new Exception($"Ungültige ANREDE in Zeile {rowNumber}: Erlaubt sind 1, 2, 3.");

                    // map record to target type
                    var record = csv.GetRecord<T>();
                    if (record == null)
                        throw new Exception($"Fehler beim Lesen der Daten in Zeile {rowNumber}.");

                    records.Add(record);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Fehler in Zeile {rowNumber}: {ex.Message}");
                }
            }

            return records;
        }
    }
}