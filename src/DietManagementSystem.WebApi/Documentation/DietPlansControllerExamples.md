### DietPlansController Request and Response Examples

#### Create Diet Plan

**Request:**

- **URL**: `/api/v1/dietplans`
- **Method**: `POST`
- **Headers**:
  ```plaintext
  Authorization: Bearer {your_jwt_token}
  Content-Type: application/json
  ```

- **Body**:
  ```json
  {
    "title": "Weight Loss Program",
    "startDate": "2024-03-01T00:00:00Z",
    "endDate": "2024-04-01T00:00:00Z",
    "initialWeight": 75.5,
    "clientId": "{clientId}"
  }
  ```

**Response:**

- **Success (201 Created)**: No content

- **Validation Error (400 Bad Request)**:
  ```json
  {
    "errors": {
      "Title": ["Title is required"],
      "StartDate": ["Start date must be before end date"],
      "InitialWeight": ["Initial weight must be greater than 0"]
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

#### Update Diet Plan

**Request:**

- **URL**: `/api/v1/dietplans/{dietPlanId}`
- **Method**: `PUT`
- **Headers**:
  ```plaintext
  Authorization: Bearer {your_jwt_token}
  Content-Type: application/json
  ```

- **Body**:
  ```json
  {
    "startDate": "2024-03-01T00:00:00Z",
    "endDate": "2024-04-15T00:00:00Z",
    "dailyCalorieTarget": 2200,
    "dailyProteinTarget": 160,
    "dailyCarbTarget": 260,
    "dailyFatTarget": 75,
    "notes": "Updated diet plan with adjusted targets"
  }
  ```

**Response:**

- **Success (204 No Content)**: No content

- **Validation Error (400 Bad Request)**:
  ```json
  {
    "errors": {
      "EndDate": ["End date must be after start date"]
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

#### Delete Diet Plan

**Request:**

- **URL**: `/api/v1/dietplans/{dietPlanId}`
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

#### Get Diet Plan by ID

**Request:**

- **URL**: `/api/v1/dietplans/{dietPlanId}`
- **Method**: `GET`
- **Headers**:
  ```plaintext
  Authorization: Bearer {your_jwt_token}
  ```

**Response:**

- **Success (200 OK)**:
  ```json
  {
    "id": "{dietPlanId}",
    "title": "Weight Loss Program",
    "startDate": "2024-03-01T00:00:00Z",
    "endDate": "2024-04-01T00:00:00Z",
    "initialWeight": 75.5,
    "clientFullName": "John Doe",
    "meals": [
      {
        "id": "{mealId1}",
        "title": "Breakfast",
        "startTime": "08:00:00",
        "endTime": "09:00:00",
        "content": "Oatmeal with fruits and nuts",
        "dietPlanId": "{dietPlanId}"
      },
      {
        "id": "{mealId2}",
        "title": "Lunch",
        "startTime": "12:00:00",
        "endTime": "13:00:00",
        "content": "Grilled chicken with vegetables",
        "dietPlanId": "{dietPlanId}"
      }
    ],
    "processes": [
      {
        "id": "{progressId1}",
        "weight": 75.5,
        "measurementDate": "2024-03-01T10:00:00Z",
        "dietPlanId": "{dietPlanId}"
      },
      {
        "id": "{progressId2}",
        "weight": 74.8,
        "measurementDate": "2024-03-15T10:00:00Z",
        "dietPlanId": "{dietPlanId}"
      }
    ]
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

#### Get All Diet Plans

**Request:**

- **URL**: `/api/v1/dietplans`
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
      "id": "{dietPlanId1}",
      "clientId": "{clientId1}",
      "dietitianId": "{dietitianId1}",
      "startDate": "2024-03-01T00:00:00Z",
      "endDate": "2024-04-01T00:00:00Z",
      "dailyCalorieTarget": 2000,
      "dailyProteinTarget": 150,
      "dailyCarbTarget": 250,
      "dailyFatTarget": 70,
      "notes": "Initial diet plan for weight loss",
      "createdAt": "2024-02-15T10:00:00Z",
      "updatedAt": "2024-02-15T10:00:00Z"
    },
    {
      "id": "{dietPlanId2}",
      "clientId": "{clientId2}",
      "dietitianId": "{dietitianId2}",
      "startDate": "2024-03-15T00:00:00Z",
      "endDate": "2024-04-15T00:00:00Z",
      "dailyCalorieTarget": 1800,
      "dailyProteinTarget": 140,
      "dailyCarbTarget": 200,
      "dailyFatTarget": 60,
      "notes": "Diet plan for maintenance",
      "createdAt": "2024-02-20T14:30:00Z",
      "updatedAt": "2024-02-20T14:30:00Z"
    }
  ]
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