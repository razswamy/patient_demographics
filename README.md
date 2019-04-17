

# Senior Developer Practical Test

# Patient Demographics

1. Create a database with whatever tables, stored procedures, etc you think you need to complete the test.

1. Create two web API endpoints.

- One endpoint should retrieve the data from the database table.
- One endpoint should save the data to the database table.

1. Create a simple web application that will:
  1. Save the data

- It should save the data as XML.
- It should use the web API to save the data.
- It should have the ability to save the following fields:
  - Forenames.
  - Surname.
  - Date of Birth.
  - Gender.
  - Telephone numbers
- It should have the ability to add multiple phone numbers:
  - Home number
  - Work number
  - Mobile number.
- The Telephone number section should be a repeating section in the XML.
- The following fields should be mandatory:
  - Forenames.
  - Surname.
  - Gender.
- The following fields should have validation:
  - Forenames (minimum length: 3 characters, maximum length: 50 characters).
  - Surname (minimum length: 2 characters, maximum length: 50 characters).
  - Date of Birth (correct date format).

1.
  1. View the data

- It should use the web API to retrieve the data.
- It should deserialize the XML data type into an object.
- It should list the following fields from the returned records:
  - Forenames.
  - Surname.
  - Date of Birth.
  - Gender.

1. If time permits add any practical enhancements that you can think of.

