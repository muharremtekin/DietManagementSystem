### ProgressController Request and Response Examples

#### Create Progress Record

**Request:**

- **URL**: `/api/v1/progress`
- **Method**: `POST`
- **Headers**:
  ```plaintext
  Authorization: Bearer {your_jwt_token}
  Content-Type: application/json
  ```

- **Body**:
  ```json
  {
    "dietPlanId": "{dietPlanId}",
    "date": "2024-03-01T10:00:00Z",
    "weight": 75.5,
    "notes": "Initial progress measurement"
  }
  ```

**Response:**

- **Success (201 Created)**: No content

- **Validation Error (400 Bad Request)**:
  ```json
  {
    "errors": {
      "Weight": ["Weight must be greater than 0"],
      "Date": ["Date is required"],
      "Notes": ["Notes are required"]
    }
  }
  ```

- **Not Found (404 Not Found)**:
  ```json
  {
    "message": "Diet plan not found."
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

#### Update Progress Record

**Request:**

- **URL**: `/api/v1/progress/{progressId}`
- **Method**: `PUT`
- **Headers**:
  ```plaintext
  Authorization: Bearer {your_jwt_token}
  Content-Type: application/json
  ```

- **Body**:
  ```json
  {
    "date": "2024-03-15T10:00:00Z",
    "weight": 74.8,
    "notes": "Progress after 2 weeks"
  }
  ```

**Response:**

- **Success (204 No Content)**: No content

- **Validation Error (400 Bad Request)**:
  ```json
  {
    "errors": {
      "Weight": ["Weight must be greater than 0"]
    }
  }
  ```

- **Not Found (404 Not Found)**:
  ```json
  {
    "message": "Progress record not found."
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

#### Delete Progress Record

**Request:**

- **URL**: `/api/v1/progress/{progressId}`
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
    "message": "Progress record not found."
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

#### Get Progress Record by ID

**Request:**

- **URL**: `/api/v1/progress/{progressId}`
- **Method**: `GET`
- **Headers**:
  ```plaintext
  Authorization: Bearer {your_jwt_token}
  ```

**Response:**

- **Success (200 OK)**:
  ```json
  {
    "id": "{progressId}",
    "date": "2024-03-01T10:00:00Z",
    "weight": 75.5,
    "notes": "Initial progress measurement"
  }
  ```

- **Not Found (404 Not Found)**:
  ```json
  {
    "message": "Progress record not found."
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

#### Get All Progress Records

**Request:**

- **URL**: `/api/v1/progress?dietPlanId={dietPlanId}&userId={userId}&fromDate=2024-03-01&toDate=2024-03-31&pageNumber=1&pageSize=10`
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
      "id": "{progressId1}",
      "date": "2024-03-01T10:00:00Z",
      "weight": 75.5,
      "notes": "Initial progress measurement"
    },
    {
      "id": "{progressId2}",
      "date": "2024-03-15T10:00:00Z",
      "weight": 74.8,
      "notes": "Progress after 2 weeks"
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

#### Get Progress Records by Client

**Request:**

- **URL**: `/api/v1/progress/client/{clientId}`
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
      "id": "{progressId1}",
      "clientId": "{clientId}",
      "weight": 75.5,
      "height": 175,
      "bodyFatPercentage": 20.5,
      "waistCircumference": 80,
      "hipCircumference": 95,
      "chestCircumference": 100,
      "bicepsCircumference": 35,
      "thighCircumference": 55,
      "notes": "Initial progress measurement",
      "measurementDate": "2024-03-01T10:00:00Z",
      "createdAt": "2024-03-01T10:00:00Z",
      "updatedAt": "2024-03-01T10:00:00Z"
    },
    {
      "id": "{progressId2}",
      "clientId": "{clientId}",
      "weight": 74.8,
      "height": 175,
      "bodyFatPercentage": 19.8,
      "waistCircumference": 79,
      "hipCircumference": 94,
      "chestCircumference": 99,
      "bicepsCircumference": 34.5,
      "thighCircumference": 54.5,
      "notes": "Progress after 2 weeks",
      "measurementDate": "2024-03-15T10:00:00Z",
      "createdAt": "2024-03-15T10:00:00Z",
      "updatedAt": "2024-03-15T10:00:00Z"
    }
  ]
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