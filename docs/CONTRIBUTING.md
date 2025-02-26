# Contribution Guidelines

We welcome your contributions to OPAS!

## How to Contribute

1. Fork the repository
2. Create a feature branch
   ```bash
   git checkout -b feature/your-feature-name
   ```
3. Commit your changes
   ```bash
   git commit -m "feat: add your feature description"
   ```
4. Create a pull request

## Development Process

1. Check existing issues and PRs to avoid duplicate work
2. For major changes, open an issue first to discuss your proposal
3. Write clear commit messages following conventional commits format:
   - `feat:` for new features
   - `fix:` for bug fixes
   - `docs:` for documentation
   - `test:` for tests
   - `refactor:` for code refactoring

## Code Style

### C# Code
- Follow Microsoft C# Coding Conventions
- Use meaningful variable and method names
- Add XML documentation for public APIs
- Implement proper exception handling
- Write unit tests for new features

### Vue.js Code
- Follow the Vue Style Guide
- Use Composition API for new components
- Implement proper type checking
- Keep components small and focused
- Add JSDoc comments for methods

## Testing

- Add unit tests for new features
- Ensure all tests pass before submitting PR
- Update existing tests if needed
- Add integration tests for API endpoints

## Documentation

- Update README.md if needed
- Add JSDoc/XML comments for new code
- Update API documentation
- Include examples for new features

## Pull Request Process

1. Update the README.md with details of changes if needed
2. Update the documentation with details of any changes to the interface
3. The PR will be merged once you have the sign-off of at least one maintainer

## License

By submitting code contributions, you agree that your contributions will be licensed under the GPL v3.

## Git Workflow

1. Create a new branch for your feature
```bash
git checkout -b feature/your-feature-name
```

2. Make your changes and commit
```bash
git add .
git commit -m "Description of changes"
```

3. Push changes and create pull request
```bash
git push origin feature/your-feature-name
``` 