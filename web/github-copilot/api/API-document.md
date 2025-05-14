# API Documentation

## Authentication API

### Register
**POST** `/api/auth/register`
- **Request Body:**
  ```json
  {
    "username": "string",
    "email": "string",
    "password": "string",
    "fullName": "string (optional)"
  }
  ```
  - **Model:**
    - `username`: The unique username for the user.
    - `email`: The email address of the user.
    - `password`: The password for the user account.
    - `fullName`: (Optional) The full name of the user.
  - **Purpose:** Used to register a new user in the system.

- **Response:**
  ```json
  {
    "statusCode": 201,
    "message": "User registered successfully",
    "data": null
  }
  ```
  - **Model:**
    - `statusCode`: HTTP status code indicating the result.
    - `message`: A message describing the result.
    - `data`: Always null for this endpoint.
  - **Purpose:** Confirms successful registration of the user.

### Login
**POST** `/api/auth/login`
- **Request Body:**
  ```json
  {
    "usernameOrEmail": "string",
    "password": "string"
  }
  ```
  - **Model:**
    - `usernameOrEmail`: The username or email of the user.
    - `password`: The password for the user account.
  - **Purpose:** Used to authenticate a user and retrieve access tokens.

- **Response:**
  ```json
  {
    "statusCode": 200,
    "message": "Login successful",
    "data": {
      "userId": 1,
      "username": "string",
      "email": "string",
      "accessToken": "string",
      "refreshToken": "string"
    }
  }
  ```
  - **Model:**
    - `userId`: The unique identifier of the user.
    - `username`: The username of the user.
    - `email`: The email address of the user.
    - `accessToken`: The JWT access token for authentication.
    - `refreshToken`: The token used to refresh the access token.
  - **Purpose:** Provides tokens for authenticated access to the API.

### Refresh Token
**POST** `/api/auth/refresh-token`
- **Request Body:**
  ```json
  {
    "token": "string"
  }
  ```
  - **Model:**
    - `token`: The refresh token issued during login.
  - **Purpose:** Used to obtain a new access token when the current one expires.

- **Response:**
  ```json
  {
    "statusCode": 200,
    "message": "Token refreshed successfully",
    "data": {
      "accessToken": "string",
      "refreshToken": "string"
    }
  }
  ```
  - **Model:**
    - `accessToken`: The new JWT access token.
    - `refreshToken`: The new refresh token.
  - **Purpose:** Ensures continued access without requiring re-login.

### Revoke Token
**POST** `/api/auth/revoke-token`
- **Request Body:**
  ```json
  {
    "token": "string"
  }
  ```
  - **Model:**
    - `token`: The refresh token to be revoked.
  - **Purpose:** Used to invalidate a refresh token, preventing further use.

- **Response:**
  ```json
  {
    "statusCode": 200,
    "message": "Token revoked successfully",
    "data": null
  }
  ```
  - **Model:**
    - `statusCode`: HTTP status code indicating the result.
    - `message`: A message describing the result.
    - `data`: Always null for this endpoint.
  - **Purpose:** Confirms successful revocation of the token.

## User Management API

### Get Users
**GET** `/api/users`
- **Response:**
  ```json
  {
    "statusCode": 200,
    "message": "Users retrieved successfully",
    "data": [
      {
        "id": 1,
        "username": "string",
        "email": "string",
        "groups": ["string"],
        "isBlocked": false
      }
    ]
  }
  ```
  - **Model:**
    - `id`: The unique identifier of the user.
    - `username`: The username of the user.
    - `email`: The email address of the user.
    - `groups`: A list of groups the user belongs to.
    - `isBlocked`: Indicates whether the user is blocked.
  - **Purpose:** Retrieves a list of all users in the system.

### Update User Permissions
**POST** `/api/users/{id}/permissions`
- **Request Body:**
  ```json
  {
    "permissionIds": [1, 2, 3],
    "operation": "add" // or "remove"
  }
  ```
  - **Model:**
    - `permissionIds`: A list of permission IDs to add or remove.
    - `operation`: The operation to perform (`add` or `remove`).
  - **Purpose:** Updates the permissions assigned to a user.

- **Response:**
  ```json
  {
    "statusCode": 200,
    "message": "Permissions updated successfully",
    "data": null
  }
  ```
  - **Model:**
    - `statusCode`: HTTP status code indicating the result.
    - `message`: A message describing the result.
    - `data`: Always null for this endpoint.
  - **Purpose:** Confirms successful update of user permissions.

### Block User
**POST** `/api/users/{id}/block`
- **Response:**
  ```json
  {
    "statusCode": 200,
    "message": "User blocked successfully",
    "data": null
  }
  ```
  - **Model:**
    - `statusCode`: HTTP status code indicating the result.
    - `message`: A message describing the result.
    - `data`: Always null for this endpoint.
  - **Purpose:** Blocks a user, preventing them from accessing the system.

### Unblock User
**POST** `/api/users/{id}/unblock`
- **Response:**
  ```json
  {
    "statusCode": 200,
    "message": "User unblocked successfully",
    "data": null
  }
  ```
  - **Model:**
    - `statusCode`: HTTP status code indicating the result.
    - `message`: A message describing the result.
    - `data`: Always null for this endpoint.
  - **Purpose:** Unblocks a user, restoring their access to the system.

## Vocabulary API

### Create Vocabulary
**POST** `/api/vocabularies`
- **Request Body:**
  ```json
  {
    "word": "string",
    "meanings": [
      {
        "partOfSpeech": "string",
        "meaning": "string",
        "ipa": "string",
        "pronunciation": "string",
        "example": "string",
        "note": "string",
        "usage": "string"
      }
    ]
  }
  ```
  - **Model:**
    - `word`: The vocabulary word.
    - `meanings`: A list of meanings for the word.
      - `partOfSpeech`: The part of speech (e.g., noun, verb).
      - `meaning`: The definition of the word.
      - `ipa`: The International Phonetic Alphabet representation.
      - `pronunciation`: The pronunciation of the word.
      - `example`: An example sentence using the word.
      - `note`: Additional notes about the word.
      - `usage`: Usage information for the word.
  - **Purpose:** Adds a new vocabulary word to the system.

- **Response:**
  ```json
  {
    "statusCode": 201,
    "message": "Vocabulary created successfully",
    "data": null
  }
  ```
  - **Model:**
    - `statusCode`: HTTP status code indicating the result.
    - `message`: A message describing the result.
    - `data`: Always null for this endpoint.
  - **Purpose:** Confirms successful creation of the vocabulary word.

### Update Vocabulary
**PUT** `/api/vocabularies`
- **Request Body:**
  ```json
  {
    "id": 1,
    "word": "string",
    "meanings": [
      {
        "id": 1,
        "partOfSpeech": "string",
        "meaning": "string",
        "ipa": "string",
        "pronunciation": "string",
        "example": "string",
        "note": "string",
        "usage": "string"
      }
    ]
  }
  ```
  - **Model:**
    - `id`: The unique identifier of the vocabulary word.
    - `word`: The updated vocabulary word.
    - `meanings`: A list of updated meanings for the word.
      - `id`: The unique identifier of the meaning.
      - `partOfSpeech`: The part of speech (e.g., noun, verb).
      - `meaning`: The definition of the word.
      - `ipa`: The International Phonetic Alphabet representation.
      - `pronunciation`: The pronunciation of the word.
      - `example`: An example sentence using the word.
      - `note`: Additional notes about the word.
      - `usage`: Usage information for the word.
  - **Purpose:** Updates an existing vocabulary word in the system.

- **Response:**
  ```json
  {
    "statusCode": 200,
    "message": "Vocabulary updated successfully",
    "data": null
  }
  ```
  - **Model:**
    - `statusCode`: HTTP status code indicating the result.
    - `message`: A message describing the result.
    - `data`: Always null for this endpoint.
  - **Purpose:** Confirms successful update of the vocabulary word.

### Get Vocabulary by ID
**GET** `/api/vocabularies/{id}`
- **Response:**
  ```json
  {
    "statusCode": 200,
    "message": "Vocabulary retrieved successfully",
    "data": {
      "id": 1,
      "word": "string",
      "meanings": [
        {
          "id": 1,
          "partOfSpeech": "string",
          "meaning": "string",
          "ipa": "string",
          "pronunciation": "string",
          "example": "string",
          "note": "string",
          "usage": "string"
        }
      ]
    }
  }
  ```
  - **Model:**
    - `id`: The unique identifier of the vocabulary word.
    - `word`: The vocabulary word.
    - `meanings`: A list of meanings for the word.
      - `id`: The unique identifier of the meaning.
      - `partOfSpeech`: The part of speech (e.g., noun, verb).
      - `meaning`: The definition of the word.
      - `ipa`: The International Phonetic Alphabet representation.
      - `pronunciation`: The pronunciation of the word.
      - `example`: An example sentence using the word.
      - `note`: Additional notes about the word.
      - `usage`: Usage information for the word.
  - **Purpose:** Retrieves details of a specific vocabulary word.

### Get Vocabulary List
**GET** `/api/vocabularies`
- **Query Parameters:**
  - `page` (default: 1): The page number to retrieve.
  - `pageSize` (default: 10): The number of items per page.
- **Response:**
  ```json
  {
    "statusCode": 200,
    "message": "Vocabularies retrieved successfully",
    "data": {
      "items": [
        {
          "id": 1,
          "word": "string",
          "meanings": [
            {
              "id": 1,
              "partOfSpeech": "string",
              "meaning": "string",
              "ipa": "string",
              "pronunciation": "string",
              "example": "string",
              "note": "string",
              "usage": "string"
            }
          ]
        }
      ],
      "totalCount": 100,
      "page": 1,
      "pageSize": 10,
      "totalPages": 10
    }
  }
  ```
  - **Model:**
    - `items`: A list of vocabulary words.
      - `id`: The unique identifier of the vocabulary word.
      - `word`: The vocabulary word.
      - `meanings`: A list of meanings for the word.
    - `totalCount`: The total number of vocabulary words.
    - `page`: The current page number.
    - `pageSize`: The number of items per page.
    - `totalPages`: The total number of pages.
  - **Purpose:** Retrieves a paginated list of vocabulary words.

### Import Vocabulary
**POST** `/api/vocabularies/import`
- **Request Body:**
  - FormData with a file containing vocabulary data.
  - **Purpose:** Imports vocabulary data in bulk from a file.

- **Response:**
  ```json
  {
    "statusCode": 200,
    "message": "Vocabulary imported successfully",
    "data": {
      "inserted": 10,
      "failed": 2
    }
  }
  ```
  - **Model:**
    - `inserted`: The number of successfully imported vocabulary words.
    - `failed`: The number of failed imports.
  - **Purpose:** Confirms the result of the import operation.

### Export Vocabulary to JSON
**GET** `/api/vocabularies/export-json`
- **Response:**
  - File download with JSON content.
  - **Purpose:** Exports all vocabulary data in JSON format for download.
