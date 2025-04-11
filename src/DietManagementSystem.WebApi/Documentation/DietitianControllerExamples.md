### DietitianController Request and Response Examples

#### Create Dietitian

**Request:**

- **URL**: `/api/v1/dietitians`
- **Method**: `POST`
- **Headers**:
  ```plaintext
  Authorization: Bearer {your_jwt_token}
  Content-Type: application/json
  ```

- **Body**:
  ```json
  {
    "email": "dietitian@example.com",
    "userName": "dietitianUser",
    "password": "SecurePassword123!",
    "fullName": "Dietitian User",
    "dateOfBirth": "1985-01-01T00:00:00Z"
  }
  ```

**Response:**

- **Success (201 Created)**: No content

- **Validation Error (400 Bad Request)**:
  ```json
  {
    "errors": {
      "Email": ["Invalid email format"],
      "Password": ["Password must meet complexity requirements"]
    }
  }
  ```

- **Not Found (404 Not Found)**:
  ```json
  {
    "message": "Resource not found."
  }
  ```

- **Forbidden (403 Forbidden)**:
  ```json
  {
    "message": "Access is denied."
  }
  ```

- **Generic Error (500 Internal Server Error)**:
  ```json
  {
    "message": "An unexpected error occurred. Please try again later."
  }
  ```

#### Update Dietitian

**Request:**

- **URL**: `/api/v1/dietitians/{userId}`
- **Method**: `PUT`
- **Headers**:
  ```plaintext
  Authorization: Bearer {your_jwt_token}
  Content-Type: application/json
  ```

- **Body**:
  ```json
  {
    "email": "updateddietitian@example.com",
    "password": "NewSecurePassword123!",
    "fullName": "Updated Dietitian User",
    "dateOfBirth": "1985-01-01T00:00:00Z"
  }
  ```

**Response:**

- **Success (204 No Content)**: No content

- **Validation Error (400 Bad Request)**:
  ```json
  {
    "errors": {
      "Email": ["Invalid email format"]
    }
  }
  ```

- **Not Found (404 Not Found)**:
  ```json
  {
    "message": "Dietitian not found."
  }
  ```

- **Forbidden (403 Forbidden)**:
  ```json
  {
    "message": "Access is denied."
  }
  ```

- **Generic Error (500 Internal Server Error)**:
  ```json
  {
    "message": "An unexpected error occurred. Please try again later."
  }
  ```

#### Delete Dietitian

**Request:**

- **URL**: `/api/v1/dietitians/{userId}`
- **Method**: `DELETE`
- **Headers**:
  ```plaintext
  Authorization: Bearer {your_jwt_token}
  ```

**Response:**

- **Success (204 No Content)**: No content

- **Not Found (404 Not Found)**:
  ```json
  {
    "message": "Dietitian not found."
  }
  ```

- **Forbidden (403 Forbidden)**:
  ```json
  {
    "message": "Access is denied."
  }
  ```

- **Generic Error (500 Internal Server Error)**:
  ```json
  {
    "message": "An unexpected error occurred. Please try again later."
  }
  ```

#### Get All Dietitians

**Request:**

- **URL**: `/api/v1/dietitians?pageNumber=1&pageSize=10`
- **Method**: `GET`
- **Headers**:
  ```plaintext
  Authorization: Bearer {your_jwt_token}
  ```

**Response:**

- **Success (200 OK)**:
  ```json
  [
    {
      "id": "{userId1}",
      "email": "dietitian1@example.com",
      "userName": "dietitianUser1",
      "fullName": "Dietitian User 1",
      "dateOfBirth": "1985-01-01T00:00:00Z"
    },
    {
      "id": "{userId2}",
      "email": "dietitian2@example.com",
      "userName": "dietitianUser2",
      "fullName": "Dietitian User 2",
      "dateOfBirth": "1986-01-01T00:00:00Z"
    }
  ]
  ```

- **Response Headers**:
  ```plaintext
  X-Pagination: {
    "TotalCount": 30,
    "CurrentPage": 1,
    "PageSize": 10,
    "TotalPage": 3,
    "HasPrevious": false,
    "HasNextPage": true
  }
  ```

- **Forbidden (403 Forbidden)**:
  ```json
  {
    "message": "Access is denied."
  }
  ```

- **Generic Error (500 Internal Server Error)**:
  ```json
  {
    "message": "An unexpected error occurred. Please try again later."
  }
  ``` 