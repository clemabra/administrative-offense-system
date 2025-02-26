# Deployment Guide

## Prerequisites
- Docker
- Docker Compose
- SSL certificate (for production)

## Configuration

### Environment Setup

#### Database Configuration
- SQLite databases are created automatically in the `backend/src/Data` directory:
  - `offenses.db`: Main database for administrative offenses
  - `AuthenticationServerDB.db`: User authentication database
  - `wz.db`: Reference number mapping database
- Database files are excluded from version control (.gitignore)
- Backup these files regularly in production

#### Environment Variables
##### Backend
- `ASPNETCORE_ENVIRONMENT`: Set to "Development" or "Production"
- `JWT_SECRET`: Secret key for JWT token generation
- `CORS_ORIGINS`: Allowed origins for CORS

##### Frontend
- `VUE_APP_API_URL`: Backend API endpoint
- `VUE_APP_ENV`: Environment setting ("development" or "production")

#### Backend Configuration
1. Create `appsettings.json` from template:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=Data/offenses.db",
    "AuthenticationConnection": "Data Source=Data/AuthenticationServerDB.db"
  },
  "Jwt": {
    "Key": "your-secret-key",
    "Issuer": "your-issuer",
    "Audience": "your-audience"
  }
}
```

#### Frontend Configuration
1. Create `.env` file from `.env.example`:
```
VUE_APP_API_URL=http://localhost:5000/api
```

### Docker Deployment

1. Build Images:
```bash
# Backend
docker build -f backend/Dockerfile -t opas-backend .

# Frontend
docker build -f frontend/Dockerfile -t opas-frontend .
```

2. Run Containers:
```bash
docker-compose up -d
```

### Production Considerations

#### Security
- Use HTTPS in production
- Configure proper CORS settings
- Set secure JWT settings
- Use environment variables for sensitive data

#### Database
- Configure proper backup strategy
- Set up database maintenance plans
- Consider data retention policies

#### Monitoring
- Set up logging
- Configure health checks
- Monitor system resources

## Troubleshooting

### Common Issues
1. Database connection errors
   - Check connection strings
   - Verify database files exist
   - Check permissions

2. Frontend API connection
   - Verify API URL in .env
   - Check CORS settings
   - Verify network connectivity

## Maintenance

### Backup Procedures
- Database backup
- Configuration backup
- Document backup schedule

### Updates
- Document update process
- Version compatibility checks
- Rollback procedures
