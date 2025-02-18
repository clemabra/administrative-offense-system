# Configuration Guide

## Environment Variables
- `NODE_ENV`: Set to 'development' or 'production'
- `API_BASE_URL`: Backend API base URL
- `PORT`: Server port (default: 5000)

## Database Configuration
- SQLite database location: `src/Data/`
- Database files:
  - `AuthenticationServerDB.db`: User authentication
  - `offenses.db`: Offense records
  - `wz.db`: Reference numbers

## Authentication Settings
- JWT token configuration
- LDAP server settings (if used)

## Development Settings
- CORS settings
- Logging levels
- Debug options

## Production Settings
- Security recommendations
- Performance optimizations
- Backup configurations
