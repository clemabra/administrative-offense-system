# Future Development

This document outlines potential future enhancements and features for OPAS.

## 1. Navigation Improvements

### User Experience
- Enable navigation back to parent pages from any subpage
- Display warning when leaving input forms through page/browser navigation if data would be lost
- Warning only appears if user has actually modified data in the form
- Form jumps to first error when attempting to submit with errors
- Opening edit mode shows start of page
- After initial capture, mask switches to edit mode (at page start)
- Success snackbars disappear after consistent time period

## 2. Contact Data Management

### Enhanced Contact Information
- Implement variable list of contact data for each delinquent
- New section below personal information
- Each entry consists of:
  - Type (empty, email, phone, fax, WhatsApp, Skype, homepage...)
  - Data (email address, phone/fax number, or messenger name)
  - Comments field
- Dynamic entry creation:
  - New empty entry appears when data is entered in last entry
  - Empty entries are deleted on save
  - New/modified entries are added/updated on save

## 3. Insurance Name Import

### CSV Import Function
- Remove manual insurance name input to reduce effort
- Import insurance names from CSV
- Handling:
  - Mark non-existent numbers as deleted without changing names
  - Add names for new numbers
  - Update existing names
  - Remove deleted marking if applicable

## 4. Case Overview

### Search and Filter
- Implement comprehensive search functionality
- Add filtering options for case management

## 5. PDF Generation

### Automatic Document Generation
- Implement automatic PDF generation with download functionality
- Include documents such as:
  - Hearing forms
  - Warning letters

## 6. Testing Infrastructure

### Test Coverage
- Implement comprehensive unit tests for backend services
- Add integration tests for API endpoints
- Set up end-to-end testing for critical user workflows

### Test Automation
- Automate test execution in CI/CD pipeline
- Add test reporting and coverage metrics
- Implement regression test suite

## 7. Additional Security Features

### Authentication
- Add Single Sign-On (SSO) integration option
- Implement basic audit logging for user actions

### Data Management
- Improve backup and recovery processes
- Add data export functionality
- Basic data anonymization for DSGVO/GDPR compliance

## 8. DSGVO/GDPR Compliance

### Data Protection Features
- Implementation of data retention and deletion concept
- Creation of privacy policy for users
- Documentation of data processing activities
- Function for data subject access requests
- Data portability implementation

## Implementation Priority

These features are listed in order of potential impact and implementation complexity. Each feature should be evaluated based on:
- User benefit
- Implementation effort
- Technical feasibility
- Integration requirements

## Contributing

If you'd like to implement any of these features, please:
1. Check if there's already an issue for the feature
2. Create a new issue if none exists
3. Discuss implementation approach
4. Submit a pull request

For detailed contribution guidelines, see [CONTRIBUTING.md](CONTRIBUTING.md). 