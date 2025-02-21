# Deployment Guide

## Prerequisites
- Node.js 16+
- .NET 8.0 SDK
- Docker
- Apache/httpd (for non-Docker deployments)

## Docker Deployment (Recommended)

### Backend Deployment
1. Build using provided Dockerfile:
   ```bash
   cd backend
   docker build -t opas-backend -f examples/docker/backend.Dockerfile .
   ```

2. Run the container:
   ```bash
   docker run -d \
     -p 5000:5000 \
     -p 7042:7042 \
     -p 7254:7254 \
     --name opas-backend \
     opas-backend
   ```

### Frontend Deployment
1. Build using provided Dockerfile:
   ```bash
   cd frontend
   docker build -t opas-frontend -f examples/docker/frontend.Dockerfile .
   ```

2. Run the container:
   ```bash
   docker run -d \
     -p 80:80 \
     --name opas-frontend \
     opas-frontend
   ```

## Manual Deployment

### Backend (.NET)
1. Build the application:
   ```bash
   cd backend
   dotnet restore
   dotnet build --configuration Release
   dotnet publish -c Release -o ./publish
   ```

2. Database setup:
   ```bash
   cd src
   dotnet ef database update --context OffenseDbContext
   dotnet ef database update --context SSOTokenDbContext
   ```

3. Run the application:
   ```bash
   cd publish
   dotnet src.dll
   ```

### Frontend (Vue.js + Apache)
1. Build the application:
   ```bash
   cd frontend
   npm install
   npm run build
   ```

2. Configure Apache:
   - Copy `examples/apache/httpd.conf` to Apache config directory
   - Copy `examples/apache/.htaccess` to your web root
   - Enable required Apache modules:
     ```bash
     a2enmod rewrite
     a2enmod proxy
     a2enmod proxy_http
     a2enmod headers
     ```

## CI/CD Configuration

Example GitLab CI configurations are provided in the `examples` directory:
- `backend.gitlab-ci.yml`: Backend CI/CD pipeline
- `frontend.gitlab-ci.yml`: Frontend CI/CD pipeline

### Pipeline Stages
1. Build
2. Test
3. Deploy

## Directory Structure

```
examples/
├── docker/
│   ├── backend.Dockerfile
│   └── frontend.Dockerfile
├── ci/
│   ├── backend.gitlab-ci.yml
│   └── frontend.gitlab-ci.yml
└── apache/
    ├── .htaccess
    └── httpd.conf
```

## Environment Variables

### Required Environment Variables

#### Backend Variables
Configure these environment variables:
- Server Configuration:
  - `ASPNETCORE_URLS`: Backend URLs (default: "http://+:5000")
  - `ASPNETCORE_ENVIRONMENT`: Runtime environment

- Authentication:
  - `JWT_SECRET`: Secret key for JWT token generation
  - `JWT_ISSUER`: Token issuer
  - `JWT_AUDIENCE`: Token audience
  - `JWT_EXPIRY_DAYS`: Token expiration in days

- LDAP Configuration:
  - `LDAP_SERVER`: LDAP server URL
  - `LDAP_PORT`: LDAP server port
  - `LDAP_BASE_DN`: Base DN for LDAP searches
  - `LDAP_USERNAME`: LDAP admin username
  - `LDAP_PASSWORD`: LDAP admin password

- Database:
  - `DB_PATH`: Path to database files
  - `OFFENSE_DB_NAME`: Offense database filename
  - `SSO_DB_NAME`: SSO database filename
  - `WZ_DB_NAME`: Reference number database filename

- CORS:
  - `ALLOWED_ORIGINS`: Comma-separated list of allowed origins

#### Frontend Variables
- API Configuration:
  - `VUE_APP_API_BASE_URL`: Backend API URL
  - `VUE_APP_ENVIRONMENT`: Runtime environment

- Authentication:
  - `VUE_APP_AUTH_ENABLED`: Enable authentication
  - `VUE_APP_AUTH_TYPE`: Authentication type (ldap/local)

### Environment Files
Example environment files are provided:
- Backend: `backend/.env.example`
- Frontend: `frontend/.env.example`

Copy these files and adjust the values:
```bash
# Backend
cp backend/.env.example backend/.env

# Frontend
cp frontend/.env.example frontend/.env
```

### Security Notes
- Never commit `.env` files to version control
- Use secure values in production
- Regularly rotate sensitive credentials
- Use appropriate file permissions for `.env` files

## Monitoring
- Backend health check: `http://localhost:5000/health`
- Apache status: `http://localhost/server-status`
- Docker logs:
  ```bash
  docker logs opas-backend
  docker logs opas-frontend
  ```

## Backup
1. Database backup:
   ```bash
   cp src/Data/*.db backup/
   ```
2. Configuration backup:
   ```bash
   cp examples/*.conf backup/
   ```

## Example Files
All deployment-related example files can be found in the `examples` directory. These files serve as templates and should be adjusted according to your specific deployment environment.
