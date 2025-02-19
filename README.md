# OPAS (Offense Processing and Administration System)

OPAS is an open-source system for recording and managing administrative offenses related to long-term care insurance premium arrears.

![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)
![Vue](https://img.shields.io/badge/Vue.js-3.x-4FC08D)

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

## Technology Stack

- Frontend: Vue.js
- Backend: ASP.NET Core
- Database: SQLite

## Quick Start Guide

1. Clone repository
2. Start backend:
   ```bash
   cd backend/src
   dotnet run
   ```
3. Start frontend:
   ```bash
   cd frontend
   npm install
   npm run serve
   ```

## Documentation

Detailed documentation can be found in the [docs](./docs) directory.

## License

This project is licensed under the [GNU General Public License v3.0](LICENSE).

## Contributing

We welcome contributions! Please read our [Contribution Guidelines](docs/CONTRIBUTING.md).
