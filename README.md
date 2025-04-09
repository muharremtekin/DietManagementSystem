# Diet Management System

A comprehensive diet management system built with .NET 8, designed to help dietitians manage their clients' diet plans and track their progress.

## 🚀 Features

- **User Management**
  - Role-based authentication (Admin, Dietitian, User)
  - Secure user registration and login
  - User profile management
  - Password reset functionality

- **Diet Plan Management**
  - Create and manage diet plans
  - Assign diet plans to clients
  - Track meal schedules and content
  - Customize diet plans based on client needs

- **Meal Management**
  - Create and update meals
  - Set meal times and content
  - Organize meals within diet plans
  - Nutritional information tracking

- **Progress Tracking**
  - Monitor client progress
  - Track measurements and goals
  - Generate progress reports
  - Visual progress charts

## 🛠️ Technology Stack

- **Backend**
  - .NET 8
  - Entity Framework Core
  - MediatR for CQRS pattern
  - FluentValidation for request validation
  - JWT Authentication
  - AutoMapper for object mapping
  - Swagger for API documentation

- **Database**
  - PostgreSQL
  - Entity Framework Core Migrations
  - Npgsql.EntityFrameworkCore.PostgreSQL

- **Development Tools**
  - Visual Studio 2022 / VS Code
  - Git for version control
  - Postman for API testing
  - pgAdmin for database management

## 📁 Project Structure

```
DietManagementSystem/
├── src/
│   ├── DietManagementSystem.Domain/         # Domain entities and interfaces
│   │   ├── Entities/                        # Domain models
│   │   └── Interfaces/                      # Repository interfaces
│   │
│   ├── DietManagementSystem.Application/    # Application logic and features
│   │   ├── Features/                        # CQRS handlers
│   │   ├── Validators/                      # FluentValidation rules
│   │   ├── DTOs/                           # Data transfer objects
│   │   └── Extensions/                      # Service extensions
│   │
│   ├── DietManagementSystem.Persistence/    # Database context and repositories
│   │   ├── Context/                         # DbContext
│   │   ├── Repositories/                    # Repository implementations
│   │   └── Configurations/                  # Entity configurations
│   │
│   └── DietManagementSystem.WebApi/         # API endpoints and controllers
│       ├── Controllers/                     # API controllers
│       ├── Extensions/                      # Web API extensions
│       └── Middleware/                      # Custom middleware
```

## 🔧 Setup and Installation

### Prerequisites

1. **Development Environment**
   - [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
   - [PostgreSQL](https://www.postgresql.org/download/)
   - [pgAdmin](https://www.pgadmin.org/download/) (optional, for database management)
   - [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
   - [Git](https://git-scm.com/downloads)

2. **Database Setup**
   ```bash
   # Create a new PostgreSQL database
   # Update connection string in appsettings.json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Database=dietmanagementsystem;Username=postgres;Password=yourpassword"
   }
   ```

3. **Clone and Build**
   ```bash
   # Clone the repository
   git clone https://github.com/yourusername/DietManagementSystem.git
   cd DietManagementSystem

   # Restore NuGet packages
   dotnet restore

   # Build the solution
   dotnet build
   ```

4. **Database Migration**
   ```bash
   # Navigate to the WebApi project
   cd src/DietManagementSystem.WebApi

   # Run migrations
   dotnet ef database update
   ```

5. **Seed Initial Data**
   ```bash
   # The system will automatically create default roles and admin user
   # Default admin credentials:
   # Email: admin@dietmanagement.com
   # Password: Admin123!
   ```

## 🧪 Testing

### Running Tests
```bash
# Navigate to the test project
cd src/DietManagementSystem.Tests

# Run all tests
dotnet test

# Run specific test category
dotnet test --filter "Category=Unit"
dotnet test --filter "Category=Integration"
```

### API Testing with Postman
1. Import the Postman collection from `/docs/postman/DietManagementSystem.postman_collection.json`
2. Set up environment variables:
   - `baseUrl`: Your API base URL
   - `token`: JWT token (will be set after login)

### Testing Different User Roles
1. **Admin User**
   - Full access to all features
   - Can manage all users and diet plans

2. **Dietitian User**
   - Can manage their clients and diet plans
   - Can track client progress

3. **Regular User**
   - Can view assigned diet plans
   - Can track their own progress

## 🚀 Deployment

### Development Environment
1. **Local Development with Docker**
   ```bash
   # Navigate to the local compose directory
   cd dms_local_compose

   # Start the PostgreSQL container
   docker-compose up -d
   ```

   The local development environment will use:
   - Database: DietManagementLocalDb
   - Username: admin
   - Password: password
   - Port: 5432

2. **Local Development without Docker**
   ```bash
   # Run the application
   dotnet run --project src/DietManagementSystem.WebApi
   ```

### Production Environment
1. **Production Deployment with Docker**
   ```bash
   # Start the production containers
   docker-compose up -d
   ```

   The production environment will use:
   - Database: DietManagementProductionDb
   - Username: dms_admin
   - Password: [secure password]
   - Port: 5432
   - API Port: 8080

2. **Environment Configuration**
   - Local Development (`dms_local_compose/docker-compose.yml`)
     - PostgreSQL container with local settings
     - Development database configuration
     - Local network setup

   - Production (`docker-compose.yml`)
     - PostgreSQL container with production settings
     - Production database configuration
     - Secure network setup
     - API container with production settings

3. **Database Connection Strings**
   - Local Development:
     ```
     Host=localhost;Database=DietManagementLocalDb;Username=admin;Password=password
     ```
   
   - Production:
     ```
     Host=postgresql;Database=DietManagementProductionDb;Username=dms_admin;Password=[secure password]
     ```

## 📝 API Documentation

The API documentation is available at `/swagger` when running the application in development mode.

### Key Endpoints

- **Authentication**
  - `POST /api/auth/login` - User login
  - `POST /api/auth/register` - User registration
  - `POST /api/auth/refresh-token` - Refresh JWT token
  - `POST /api/auth/forgot-password` - Request password reset
  - `POST /api/auth/reset-password` - Reset password

- **Diet Plans**
  - `GET /api/diet-plans` - Get all diet plans
  - `POST /api/diet-plans` - Create a new diet plan
  - `PUT /api/diet-plans/{id}` - Update a diet plan
  - `DELETE /api/diet-plans/{id}` - Delete a diet plan
  - `GET /api/diet-plans/{id}/meals` - Get meals for a diet plan

- **Meals**
  - `GET /api/meals` - Get all meals
  - `POST /api/meals` - Create a new meal
  - `PUT /api/meals/{id}` - Update a meal
  - `DELETE /api/meals/{id}` - Delete a meal

- **Progress**
  - `GET /api/progress` - Get progress records
  - `POST /api/progress` - Create a new progress record
  - `PUT /api/progress/{id}` - Update a progress record
  - `GET /api/progress/statistics` - Get progress statistics

## 🔒 Security

- JWT-based authentication with refresh tokens
- Role-based authorization
- Secure password hashing using ASP.NET Identity
- Input validation using FluentValidation
- CORS policy configuration
- HTTPS enforcement in production
- SQL injection prevention
- XSS protection

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Development Guidelines
- Follow SOLID principles
- Write unit tests for new features
- Use meaningful commit messages
- Update documentation when necessary
- Follow the existing code style

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Authors

- Your Name - Initial work

## 🙏 Acknowledgments

- Thanks to all contributors who have helped shape this project
- Special thanks to the .NET community for their excellent tools and libraries