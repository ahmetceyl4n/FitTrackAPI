# FitnessApp - Workout Tracking API

**FitnessApp** is a modern Backend API project developed with .NET 8 and Clean Architecture principles, allowing users to track their workouts, exercises, and set counts.

## 🚀 Features

- **User Management**: Secure registration and login (JWT Authentication).
- **Workout Tracking**: Create, update, and delete workouts.
- **Exercise Management**: Add and remove exercises from workouts.
- **Set Tracking**: Record set details like weight and repetition count for each exercise.
- **Clean Architecture**: Layered architecture structure (API, Application, Domain, Infrastructure).

## 🛠 Technologies Used

- **.NET 8 SDK**: Latest .NET version.
- **Entity Framework Core**: Database ORM tool.
- **AutoMapper**: Object mapping.
- **FluentValidation**: Data validation rules.
- **JWT Helper**: Token-based authentication.
- **Swagger/OpenAPI**: API documentation and testing.

## 📂 Project Structure

The project is divided into 4 main layers in accordance with the **Clean Architecture** structure:

1.  **FitnessApp.Domain**: Entities and core business rules. No external dependencies.
2.  **FitnessApp.Application**: Business logic (Services), interfaces (Interfaces), DTOs, and Validators.
3.  **FitnessApp.Infrastructure**: Database access (Repository implementation), external service integrations.
4.  **FitnessApp.API**: API endpoints exposed to end-users (Controllers).

## ⚙️ Installation and Running

1.  **Clone the Repo**:
    ```bash
    git clone https://github.com/username/FitnessApp.git
    cd FitnessApp
    ```

2.  **Configure Database**:
    Edit the `ConnectionStrings` section in `appsettings.json` according to your database server.
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=...;Database=FitnessAppDb;Trusted_Connection=True;..."
    }
    ```

3.  **Apply Migrations**:
    In the terminal, while in the project root directory:
    ```bash
    dotnet ef database update --project FitnessApp.Infrastructure --startup-project FitnessApp.API
    ```

4.  **Start the Project**:
    ```bash
    dotnet run --project FitnessApp.API
    ```

5.  **Test the API**:
    You can use the Swagger interface by navigating to `https://localhost:5001/swagger` (or the specified port) in your browser.


