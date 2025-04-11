### ClientController Request and Response Examples

#### Create Client

**Request:**

- **URL**: `/api/v1/clients`
- **Method**: `POST`
- **Headers**:
  ```plaintext
  Authorization: Bearer {your_jwt_token}
  Content-Type: application/json
  ```

- **Body**:
  ```json
  {
    "email": "client@example.com",
    "userName": "clientUser",
    "password": "SecurePassword123!",
    "fullName": "Client User",
    "dateOfBirth": "1995-01-01T00:00:00Z"
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

#### Update Client

**Request:**

- **URL**: `/api/v1/clients/{userId}`
- **Method**: `PUT`
- **Headers**:
  ```plaintext
  Authorization: Bearer {your_jwt_token}
  Content-Type: application/json
  ```

- **Body**:
  ```json
  {
    "email": "updatedclient@example.com",
    "password": "NewSecurePassword123!",
    "fullName": "Updated Client User",
    "dateOfBirth": "1995-01-01T00:00:00Z"
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
    "message": "Client not found."
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

#### Delete Client

**Request:**

- **URL**: `/api/v1/clients/{userId}`
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
    "message": "Client not found."
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

#### Get Client by ID

**Request:**

- **URL**: `/api/v1/clients/{userId}`
- **Method**: `GET`
- **Headers**:
  ```plaintext
  Authorization: Bearer {your_jwt_token}
  ```

**Response:**

- **Success (200 OK)**:
  ```json
  {
    "id": "{userId}",
    "email": "client@example.com",
    "userName": "clientUser",
    "fullName": "Client User",
    "dateOfBirth": "1995-01-01T00:00:00Z"
  }
  ```

- **Not Found (404 Not Found)**:
  ```json
  {
    "message": "Client not found."
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

#### Get All Clients

**Request:**

- **URL**: `/api/v1/clients?pageNumber=1&pageSize=10`
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
      "email": "client1@example.com",
      "userName": "clientUser1",
      "fullName": "Client User 1",
      "dateOfBirth": "1995-01-01T00:00:00Z"
    },
    {
      "id": "{userId2}",
      "email": "client2@example.com",
      "userName": "clientUser2",
      "fullName": "Client User 2",
      "dateOfBirth": "1996-01-01T00:00:00Z"
    }
  ]
  ```

- **Response Headers**:
  ```plaintext
  X-Pagination: {
    "TotalCount": 50,
    "CurrentPage": 1,
    "PageSize": 10,
    "TotalPage": 5,
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