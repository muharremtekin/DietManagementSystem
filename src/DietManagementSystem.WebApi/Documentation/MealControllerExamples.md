### MealController Request and Response Examples

#### Create Meal

**Request:**

- **URL**: `/api/v1/meals`
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
    "title": "Breakfast",
    "startTime": "2024-03-01T08:00:00Z",
    "endTime": "2024-03-01T09:00:00Z",
    "content": "Oatmeal with fruits and nuts"
  }
  ```

**Response:**

- **Success (201 Created)**: No content

- **Validation Error (400 Bad Request)**:
  ```json
  {
    "errors": {
      "Title": ["Title is required"],
      "StartTime": ["Start time must be before end time"],
      "Content": ["Content is required"]
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

#### Update Meal

**Request:**

- **URL**: `/api/v1/meals/{mealId}`
- **Method**: `PUT`
- **Headers**:
  ```plaintext
  Authorization: Bearer {your_jwt_token}
  Content-Type: application/json
  ```

- **Body**:
  ```json
  {
    "title": "Updated Breakfast",
    "startTime": "08:00:00",
    "endTime": "09:00:00",
    "content": "Updated oatmeal with fruits and nuts"
  }
  ```

**Response:**

- **Success (204 No Content)**: No content

- **Validation Error (400 Bad Request)**:
  ```json
  {
    "errors": {
      "StartTime": ["Start time must be before end time"]
    }
  }
  ```

- **Not Found (404 Not Found)**:
  ```json
  {
    "message": "Meal not found."
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

#### Delete Meal

**Request:**

- **URL**: `/api/v1/meals/{mealId}`
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
    "message": "Meal not found."
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

#### Get Meal by ID

**Request:**

- **URL**: `/api/v1/meals/{mealId}`
- **Method**: `GET`
- **Headers**:
  ```plaintext
  Authorization: Bearer {your_jwt_token}
  ```

**Response:**

- **Success (200 OK)**:
  ```json
  {
    "id": "{mealId}",
    "title": "Breakfast",
    "startTime": "08:00:00",
    "endTime": "09:00:00",
    "content": "Oatmeal with fruits and nuts",
    "dietPlanId": "{dietPlanId}"
  }
  ```

- **Not Found (404 Not Found)**:
  ```json
  {
    "message": "Meal not found."
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

#### Get All Meals

**Request:**

- **URL**: `/api/v1/meals?userId={userId}&dietPlanId={dietPlanId}&startDate=2024-03-01&endDate=2024-03-31&pageNumber=1&pageSize=10`
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
      "id": "{mealId1}",
      "title": "Breakfast",
      "startTime": "08:00:00",
      "endTime": "09:00:00",
      "content": "Oatmeal with fruits and nuts",
      "dietPlanId": "{dietPlanId1}"
    },
    {
      "id": "{mealId2}",
      "title": "Lunch",
      "startTime": "12:00:00",
      "endTime": "13:00:00",
      "content": "Grilled chicken with vegetables",
      "dietPlanId": "{dietPlanId2}"
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

#### Get Meals by Diet Plan

**Request:**

- **URL**: `/api/v1/meals/dietplan/{dietPlanId}`
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
      "id": "{mealId1}",
      "name": "Breakfast",
      "description": "Healthy breakfast meal",
      "calories": 450,
      "protein": 25,
      "carbohydrates": 45,
      "fat": 15,
      "mealTime": "2024-03-01T08:00:00Z",
      "clientId": "{clientId}",
      "dietPlanId": "{dietPlanId}",
      "createdAt": "2024-02-15T10:00:00Z",
      "updatedAt": "2024-02-15T10:00:00Z"
    },
    {
      "id": "{mealId2}",
      "name": "Lunch",
      "description": "Balanced lunch meal",
      "calories": 600,
      "protein": 35,
      "carbohydrates": 60,
      "fat": 20,
      "mealTime": "2024-03-01T12:30:00Z",
      "clientId": "{clientId}",
      "dietPlanId": "{dietPlanId}",
      "createdAt": "2024-02-15T11:00:00Z",
      "updatedAt": "2024-02-15T11:00:00Z"
    }
  ]
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