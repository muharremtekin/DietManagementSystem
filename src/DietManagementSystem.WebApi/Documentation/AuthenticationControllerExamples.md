### AuthenticationController Request and Response Examples

#### Login

**Request:**

- **URL**: `/api/v1/authentication/login`
- **Method**: `POST`
- **Headers**:
  ```plaintext
  Content-Type: application/json
  ```

- **Body**:
  ```json
  {
    "userName": "johnDoe",
    "email": "user@example.com",
    "password": "SecurePassword123!"
  }
  ```

**Response:**

- **Success (200 OK)**:
  ```json
  {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c",
    "tokenExpiryTime": "2024-03-15T10:00:00Z"
  }
  ```

- **Validation Error (400 Bad Request)**:
  ```json
  {
    "errors": {
      "Password": ["Password is required"],
      "UserName": ["Either UserName or Email must be provided"]
    }
  }
  ```

- **Unauthorized (401 Unauthorized)**:
  ```json
  {
    "message": "Invalid credentials."
  }
  ```

- **Generic Error (500 Internal Server Error)**:
  ```json
  {
    "message": "An unexpected error occurred. Please try again later."
  }
  ``` 