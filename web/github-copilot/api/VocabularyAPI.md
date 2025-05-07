# Vocabulary API Documentation

This document provides details about the Vocabulary API, including endpoints, request models, and response models.

## Base URL
```
/api/vocabularies
```

---

## Endpoints

### 1. Create Vocabulary
**Endpoint:**
```
POST /api/vocabularies
```

**Description:**
Create a new vocabulary entry for the authenticated user.

**Request Body:**
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

**Response:**
- **Success (201):**
```json
{
  "message": "Vocabulary created successfully.",
  "statusCode": 201
}
```
- **Error (400/401):**
```json
{
  "message": "Error message.",
  "statusCode": 400
}
```

---

### 2. Update Vocabulary
**Endpoint:**
```
PUT /api/vocabularies
```

**Description:**
Update an existing vocabulary entry for the authenticated user.

**Request Body:**
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

**Response:**
- **Success (200):**
```json
{
  "message": "Vocabulary updated successfully.",
  "statusCode": 200
}
```
- **Error (400/401/404):**
```json
{
  "message": "Error message.",
  "statusCode": 400
}
```

---

### 3. Get Vocabulary by ID
**Endpoint:**
```
GET /api/vocabularies/{id}
```

**Description:**
Retrieve a specific vocabulary entry by its ID.

**Response:**
- **Success (200):**
```json
{
  "id": 1,
  "word": "string",
  "userId": 1,
  "meanings": [
    {
      "id": 1,
      "vocabularyId": 1,
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
- **Error (401/404):**
```json
{
  "message": "Error message.",
  "statusCode": 404
}
```

---

### 4. Get Vocabulary List
**Endpoint:**
```
GET /api/vocabularies
```

**Description:**
Retrieve a paginated list of vocabulary entries for the authenticated user.

**Query Parameters:**
- `page` (optional, default: 1): Page number.
- `pageSize` (optional, default: 10): Number of items per page.

**Response:**
- **Success (200):**
```json
[
  {
    "id": 1,
    "word": "string",
    "userId": 1,
    "meanings": [
      {
        "id": 1,
        "vocabularyId": 1,
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
]
```
- **Error (401):**
```json
{
  "message": "Error message.",
  "statusCode": 401
}
```

---

## Models

### VocabularyDTO
```json
{
  "id": 1,
  "word": "string",
  "userId": 1,
  "meanings": [
    {
      "id": 1,
      "vocabularyId": 1,
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

### CreateVocabularyRequest
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

### UpdateVocabularyRequest
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

### VocabularyMeaningDTO
```json
{
  "id": 1,
  "vocabularyId": 1,
  "partOfSpeech": "string",
  "meaning": "string",
  "ipa": "string",
  "pronunciation": "string",
  "example": "string",
  "note": "string",
  "usage": "string"
}
```