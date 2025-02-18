# Deployment Guide

## Prerequisites
- Node.js 16+
- .NET 8.0 SDK
- SQLite

## Production Deployment Steps

### Backend Deployment
1. Build the application:
   ```bash
   cd backend
   dotnet publish -c Release
   ```
2. Deploy files to server
3. Configure environment variables
4. Start the service

### Frontend Deployment
1. Build the application:
   ```bash
   cd frontend
   npm install
   npm run build
   ```
2. Deploy dist folder to web server
3. Configure web server (nginx/apache)

## Docker Deployment
(If applicable, add Docker deployment instructions)

## Monitoring
- Log file locations
- Health check endpoints
- Error monitoring

## Backup Procedures
- Database backup
- Configuration backup
- Recovery procedures
