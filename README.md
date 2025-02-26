# OPAS (Offense Processing and Administration System)

OPAS is an open-source system for recording and managing administrative offenses related to long-term care insurance premium arrears.

![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)
![Vue](https://img.shields.io/badge/Vue.js-3.x-4FC08D)

## Prerequisites
- Node.js 16+
- .NET 8.0 SDK
- Git
- VS Code (recommended)

## Project Description

OPAS streamlines the process of managing cases where individuals have fallen behind on their long-term care insurance payments. The system enables efficient handling of administrative offense procedures from initial recording to case completion.

### Key Features

#### Case Management
- Initial case recording for administrative offenses according to §121 Abs.1 Nr.6 SGB XI
- Automatic file number generation during initial recording
- Case overview with search functionality by case number or name
- CSV file import for bulk processing of administrative offense reports

#### Reference Number System
- Reference number configuration system
- Mapping of file number endings (Ez.) to reference numbers (Wz.)

#### Technical Features
- Database integration with SQLite
- Data validation and format checks
- Internal calculations
- Artificial primary key implementation
- Comprehensive error handling (including duplicate case detection)

#### User Management
- User authentication and authorization
- Role-based access control
- User administration for user-dependent operations

#### UI/UX
- Implementation of LVwA Sachsen-Anhalt design guidelines
- Responsive design for various screen sizes
- Comprehensive style guide
- OPAS UI Design template @figma: https://www.figma.com/design/UkqFSVSTzatC9xJiE6rcDp/OPUS?node-id=126-154&t=IngUoR9Sc5Zerk75-0


## Technology Stack

- Frontend: Vue.js
- Backend: ASP.NET Core
- Database: SQLite

## Project Structure
```
opas/
├── backend/               # .NET Core backend
│   ├── src/              # Source files
│   │   ├── Controllers/  # API endpoints
│   │   ├── Models/       # Data models
│   │   └── Services/     # Business logic
│   └── tests/            # Unit tests
└── frontend/             # Vue.js frontend
    ├── src/
    │   ├── components/   # Vue components
    │   ├── views/        # Page components
    │   ├── router/       # Route definitions
    │   └── assets/       # Static files
    └── public/           # Public assets
```

## Development Setup

### Recommended VS Code Extensions

#### Backend Development
- C# for Visual Studio Code (ms-dotnettools.csharp)

#### Frontend Development
- Volar (Vue Language Features)
- ESLint
- Prettier - Code formatter

## Quick Start Guide

1. Clone Repository
```bash
git clone [repository-url]
cd opas
```

2. Backend Setup
```bash
cd backend
dotnet restore
dotnet run
```
The backend will start at http://localhost:5000

3. Frontend Setup
```bash
cd frontend
npm install
npm run serve
```
The frontend will start at http://localhost:8080

### Database Setup
- Initial database creation happens automatically on first run
- Use Swagger UI to seed test data (optional):
  1. Navigate to http://localhost:5000/swagger
  2. Use the POST /api/Seeder/seed endpoint

### Basic Configuration
- Backend: Configure connection strings in `appsettings.json`
- Frontend: Set API endpoints in `.env` file
- For detailed deployment and configuration options, see [Deployment Guide](docs/deployment.md)

## Documentation

Detailed documentation can be found in the [docs](./docs) directory.

## License

This project is licensed under the [GNU General Public License v3.0](LICENSE).

## Contributing

We welcome contributions! Please read our [Contribution Guidelines](docs/CONTRIBUTING.md).
