# Development Guidelines

## Project Structure

### Backend (.NET Core)
```
backend/
├── src/
│   ├── Controllers/     # API endpoints
│   ├── Models/          # Data models
│   ├── Services/        # Business logic
│   └── Data/           # Database context and migrations
```

### Frontend (Vue.js)
```
frontend/
├── src/
│   ├── components/     # Vue components
│   ├── assets/         # Static files
│   └── api.js         # API configuration
```

## Development Environment Setup

### Prerequisites
- .NET 8.0 SDK
- Node.js 16+
- Visual Studio Code or Visual Studio 2022
- SQLite

### Backend Development
```bash
cd backend
dotnet restore
dotnet run
```

### Frontend Development
```bash
cd frontend
npm install
npm run serve
```

## Coding Standards

### C# Guidelines
- Follow Microsoft C# Coding Conventions
- Use async/await for asynchronous operations
- Implement proper exception handling
- Document public APIs

### Vue.js Guidelines
- Follow Vue Style Guide
- Use Composition API for new components
- Implement proper type checking
- Keep components small and focused

## Database Management

### Entity Framework Migrations
```bash
# Create new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update
```

### Database Seeding
- Use seeder classes for test data
- Keep seed data consistent
- Document data relationships

## Testing

### Backend Testing
- Unit tests for business logic
- Integration tests for API endpoints
- Use xUnit for testing framework

### Frontend Testing
- Component testing with Vue Test Utils
- E2E testing with Cypress
- Regular accessibility testing

## Git Workflow

### Branching Strategy
- main: production-ready code
- develop: integration branch
- feature/*: new features
- bugfix/*: bug fixes

### Commit Messages
- Use clear, descriptive messages
- Reference issue numbers
- Follow conventional commits format

## Documentation

### Code Documentation
- Document complex logic
- Add XML comments to C# public APIs
- Include JSDoc comments in Vue components

### API Documentation
- Keep API documentation up to date
- Document request/response formats
- Include example requests

## Security Considerations

### Authentication
- Use JWT tokens
- Implement proper token validation
- Handle token refresh

### Data Protection
- Sanitize user inputs
- Implement proper CORS settings
- Follow GDPR requirements

## Performance

### Optimization Guidelines
- Minimize database queries
- Implement proper caching
- Optimize frontend bundle size
- Use lazy loading where appropriate
