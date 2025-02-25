# Known Issues and Bugs

## CSV Import Function

### Description
Improvements needed for CSV file import functionality for notifications according to §121 Abs.1 Nr.6 SGB XI.

### Details
1. File Number Generation During Import
   - Current issue: Incorrect file number generation when importing data
   - Example error: "Ungültiges Format für Fallnummer: 00008-1"
   - Root cause analysis:
     - Sequential numbering issue with reference numbers
     - System doesn't properly check highest sequential number for existing file numbers
     - Should generate "00008-3" instead of current incorrect numbering
   - **Priority:** Medium
   - **Status:** Open

2. CSV File Validation
   - End of delay is not mandatory in database and forms anymore
   - Line breaks in data fields trigger generic error messages
   - Files without BOM need proper rejection handling
   - Unpaired text qualifiers in last column need proper validation
   - **Priority:** Low
   - **Status:** Open

   ### Technical Notes
   - Issue occurs when matching records by:
     - Arrears date
     - Name with birth date
   - First case processes correctly
   - Second case: Name difference handling needs improvement
   - Need to implement proper sequential number checking

3. CSV Field Validation
   - Missing validation for reference number field (no error message when empty)
   - Missing validation for postal code field
   - All validations from manual entry form should be applied to CSV import:
     - Date fields (implemented)
     - Follow-up notification field (implemented)
     - Form of address field (implemented)
     - Postal code field (missing)
     - Amount fields with exactly 2 decimal places (missing)
   - **Priority:** Medium
   - **Status:** Open

## Automatic File Number Generation

### Description
Issue with automatic file number generation during initial case recording.

### Details
- Current behavior: After successful search for an existing delinquent, sequential number is appended instead of replaced
- Example: System generates "00013-5-7" instead of replacing the existing number
- **Priority:** Medium
- **Status:** Open

## Validation Messages

### Description
Missing validation messages for various form fields.

### Details
Fields needing validation messages:
- Date fields (5 instances)
- Follow-up notification
- Form of address
- Postal code
- Amounts (2 decimal places)
- **Priority:** Low
- **Status:** Open

## Form Field Requirements

### Description
Review and adjust mandatory field requirements in edit mode.

### Details
- "End of delay" field should not be mandatory in backend logic when editing
- Need to verify if this applies to initial capture form as well
- **Priority:** Medium
- **Status:** Open

## Contributing

If you'd like to help resolve these issues:
1. Check if the issue is already being worked on
2. Comment on the issue to express your interest
3. Fork the repository and create a branch
4. Submit a pull request with your solution

For more details, see our [Contributing Guidelines](CONTRIBUTING.md).
