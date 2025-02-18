# Application Logic with .NET Core (C#)

This is the application logic layer between the user interface and the database. \
It controls the connection between the layers as well as the correct formating and routing of the data.

## Version

ASP.NET Core in .NET 8.0

## Tooling

### **Framework**

- ASP.NET Core => framework for building web applications and APIs

### **API**

- RESTful APIs using ASP.NET Core Web API, will act as the bridge between the user interface and application logic, handling all requests and business logic.

### **Authentication and Authorization**

- Using ASP.NET Core Identity for user authentication and authorization
- For later development stages possibly OAuth or OpenID Connect

### **Extensions**

- VS Code extensions such as ESLint, Prettier for code formatting, and C# for .NET development

## Notes

- Web Server with Vue.js typically run on localhost:8080
- ASP.NET core ususally run on localhost:5000 with Kestrel or an adress like localhost:49960 with IISExpress

## Using Swagger to test API

1. Start App (_'cd src' -> 'dotnet run'_)
2. Open web browser and navigate to ```http://localhost:5000/swagger```

- **Get All Records:** Use _GET /api/Offense endpoint_ to retrieve all records from the database.

- **Post a New Record:** Use _POST /api/Offense endpoint_ to add a new record to the database.

## Example of Using Swagger to Add and View Data

1. Add a New Record:

    - In the Swagger UI, select the POST /api/Offense endpoint.
    - Click Try it out.
    - Enter the JSON data for the new record.
    - Click Execute.

2. View All Records:

    - In the Swagger UI, select the GET /api/Offense endpoint.
    - Click Try it out.
    - Click Execute to see all records in the database.

## Update Database

1. Ensure backend application is terminated
2. Delete files in Data folder
3. Update database ('dotnet ef database update')
4. Seed database with data using Swagger UI:

    - In the Swagger UI, select the POST /api/Seeder/seed endpoint.
    - Click Try it out.
    - Click Execute.

## Additional Commands

Checking for model switches withotu migrationm switch:

```dotnet ef migrations has-pending-model-changes```

## CSV Field mapping

| CSV-Field        | DTO-Field                          | Mapping |
| --------------- | --------------------------------- | ------- |
| MELDEDATUM      | Meldedatum                        | &check; |
| VU_ID           | Versicherungsunternehmensnummer   | &check; |
| VERSNR          | Versicherungsnummer               | &check; |
| VERS_BEGINN     | BeginnVersicherung                | &check; |
| ANREDE          | Geschlecht                        | &check; |
| NAME            | Nachname                          | &check; |
| VORNAME         | Vorname                           | &check; |
| PLZ             | Plz                               | &check; |
| ORT             | Wohnort                           | &check; |
| STRASSE         | Str und Hausnummer                | &check; |
| GEBDATUM        | Geburtsdatum                      | &check; |
| AUF_DATUM       | Aufforderungsdatum                | &check; |
| BEGINN_RUECKS   | BeginnRueckstand                  | &check; |
| BEITRAGSRUECKS  | Beitragsrueckstand                | &check; |
| PPV_SOLLBEITRAG | Sollbeitrag                       | &check; |
| FOLGEMELDUNG    | Folgemeldung                      | &check; |
