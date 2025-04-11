### AdminController Request and Response Examples

#### Create Administrator

**Request:**

- **URL**: `/api/v1/admins`
- **Method**: `POST`
- **Headers**:
  ```plaintext
  Authorization: Bearer {your_jwt_token}
  Content-Type: application/json
  ```

- **Body**:
  ```json
  {
    "email": "admin@example.com",
    "userName": "adminUser",
    "password": "SecurePassword123!",
    "fullName": "Admin User",
    "dateOfBirth": "1990-01-01T00:00:00Z"
  }
  ```

**Response:**

- **Success (201 Created)**:

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

#### Update Administrator

**Request:**

- **URL**: `/api/v1/admins/{userId}`
- **Method**: `PUT`
- **Headers**:
  ```plaintext
  Authorization: Bearer {your_jwt_token}
  Content-Type: application/json
  ```

- **Body**:
  ```json
  {
    "email": "updatedadmin@example.com",
    "password": "NewSecurePassword123!",
    "fullName": "Updated Admin User",
    "dateOfBirth": "1990-01-01T00:00:00Z"
  }
  ```

**Response:**

- **Success (204 No Content)**:
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
    "message": "Administrator not found."
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

#### Delete Administrator

**Request:**

- **URL**: `/api/v1/admins/{userId}`
- **Method**: `DELETE`
- **Headers**:
  ```plaintext
  Authorization: Bearer {your_jwt_token}
  ```

**Response:**

- **Success (204 No Content)**:

- **Not Found (404 Not Found)**:
  ```json
  {
    "message": "Administrator not found."
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

#### Get All Administrators

**Request:**

- **URL**: `/api/v1/admins?pageNumber=1&pageSize=10`
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
      "email": "admin1@example.com",
      "userName": "adminUser1",
      "fullName": "Admin User 1",
      "dateOfBirth": "1990-01-01T00:00:00Z"
    },
    {
      "id": "{userId2}",
      "email": "admin2@example.com",
      "userName": "adminUser2",
      "fullName": "Admin User 2",
      "dateOfBirth": "1991-01-01T00:00:00Z"
    }
  ]
  ```

- **Response Headers**:
  ```plaintext
  X-Pagination: {
    "TotalCount": 25,
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