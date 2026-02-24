# Online Course Enrollment System

A .NET Core Web API project for managing students, courses, and enrollments.

## Project Structure

- **[./Controllers/](./Controllers/)**: Contains API controllers that handle incoming HTTP requests and return responses.
  - `CourseController.cs`: Endpoints for course management.
  - `EnrollmentController.cs`: Endpoints for handling student enrollments.
  - `StudentController.cs`: Endpoints for student management.

- **[./Services/](./Services/)**: Contains the business logic layer. Controllers interact with these services to perform operations.
  - Interfaces (e.g., `ICourseManagementService.cs`) define the contracts.
  - Implementations (e.g., `CourseManagement.cs`) contain the actual logic.

- **[./Model/](./Model/)**: Contains the data models and database context.
  - `Course.cs`, `Enrollment.cs`, `Student.cs`: Entity models representing database tables.
  - **[./Model/Context/](./Model/Context/)**: Contains the Entity Framework Core `DbContext` for database connectivity.

- **[./Migrations/](./Migrations/)**: Contains Entity Framework Core migration files used to manage database schema changes.

- **[./Properties/](./Properties/)**: Contains configuration for application launching (e.g., `launchSettings.json`).

- **[./Program.cs](./Program.cs)**: The entry point of the application. It configures services (Dependency Injection), middleware, and starts the web host.

- **[./appsettings.json](./appsettings.json)**: Main configuration file for settings like connection strings and logging.

## Technologies Used

- **.NET Core Web API**
- **Entity Framework Core** (SQL Server)
- **Dependency Injection** (Scoped services)
