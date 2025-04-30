 "Discover Galle:Smart Tourism System for Personalized Travel and Booking"– .NET C# API
This repository contains the backend API developed using ASP.NET Core (.NET C#) for the Smart Tourism System. The backend supports features like user authentication, booking management, tourism resource handling, and personalized tour planning.

🚀 Getting Started with Creating a .NET C# App
To get started, make sure you have the following installed:

.NET SDK (latest LTS)

Visual Studio or any C# compatible IDE

Clone the repository:

bash
Copy
Edit
git clone https://github.com/your-repo-name/smart-tourism-backend.git
cd smart-tourism-backend
📦 Available Scripts
In the project directory, you can run:

🔨 Build the Project
bash
Copy
Edit
dotnet build
▶️ Run the API
bash
Copy
Edit
dotnet run
The default URL is typically: https://localhost:5001 or http://localhost:5000

✅ Run Tests
bash
Copy
Edit
dotnet test
🧪 Swagger API Testing
Once running, go to:

bash
Copy
Edit
https://localhost:5001/swagger
Use this interface to interact with and test API endpoints.

📁 Project Structure (MVC Pattern)
Models/ – Define the data structure and entities (e.g., User, Booking, Hotel).

Controllers/ – Handle HTTP requests and route them to appropriate services.

Services/ – Business logic and interactions with the database.

DTOs/ – Used to structure the data transferred between client and server.

Data/ – Contains the database context and migration files.

📚 Learn More
🧩 Code Splitting
Use separate services and controllers for features like:

User management

Booking system

Tour packages

This enhances modularity and testability.

📊 Analyzing the Bundle Size
To analyze performance and memory usage:

bash
Copy
Edit
dotnet-trace collect --process-id <pid>
Or use the Visual Studio Diagnostic Tools during debugging.

📱 Making a Progressive Web App
While the backend itself is not a PWA, ensure your frontend communicates with this backend using REST APIs and handles offline data via service workers (PWA).

⚙️ Advanced Configuration
Customize your appsettings.json for:

Database connection strings

JWT token settings

Email configurations

For dependency injection, register services in Program.cs or Startup.cs:

csharp
Copy
Edit
builder.Services.AddScoped<IUserService, UserService>();
🚀 Deployment
To publish the backend:

bash
Copy
Edit
dotnet publish -c Release -o ./publish
Deploy the output from the publish/ folder to:

IIS

Azure App Services

Docker Container

Linux with Nginx/Apache

📖 Additional .NET Concepts
Model – Represents the data (e.g., User, Booking)

Controller – Defines endpoints like POST /api/bookings

Service – Business logic (e.g., validating bookings, interacting with DB)

Dependency Injection – Services are injected into controllers for loose coupling

Entity Framework Core – ORM used to access and manage the database

