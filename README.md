# Discover Galle: Smart Tourism System for Personalized Travel and Booking

This repository contains the backend API developed using **ASP.NET Core (.NET C#)** for the Smart Tourism System. The backend supports features like **user authentication**, **booking management**, **tourism resource handling**, and **personalized tour planning**.

---

## Getting Started with Creating a .NET C# App

To get started, make sure you have the following installed:

- .NET SDK (latest LTS)
- Visual Studio or any C# compatible IDE

**Clone the repository:**
```
git clone https://github.com/your-repo-name/smart-tourism-backend.git
cd smart-tourism-backend
```

---

## Available Scripts

In the project directory, you can run:

### Build the Project
```
dotnet build
```

### Run the API
```
dotnet run
```
The default URL is typically:  
`https://localhost:5001` or `http://localhost:5000`

### Run Tests
```
dotnet test
```

### Swagger API Testing

Navigate to:  
`https://localhost:5001/swagger`  
Use this interface to interact with and test API endpoints.

---

## Project Structure (MVC Pattern)

- `Models/` – Define the data structure and entities (e.g., User, Booking, Hotel)  
- `Controllers/` – Handle HTTP requests and route them to appropriate services  
- `Services/` – Business logic and interactions with the database  
- `DTOs/` – Used to structure the data transferred between client and server  
- `Data/` – Contains the database context and migration files

---

## Learn More

### Code Splitting

Use separate services and controllers for features like:

- User management  
- Booking system  
- Tour packages  

### Analyzing the Bundle Size
To analyze performance and memory usage:
```
dotnet-trace collect --process-id <pid>
```
Or use the Visual Studio Diagnostic Tools during debugging.

### Making a Progressive Web App

While the backend is not a PWA itself, ensure your frontend uses REST APIs and supports offline data with service workers.

### Advanced Configuration

Customize your `appsettings.json` for:

- Database connection strings  
- JWT token settings  
- Email configurations  

**Dependency Injection Example (in Program.cs):**
```
builder.Services.AddScoped<IUserService, UserService>();
```

---

## Deployment

To publish the backend:
```
dotnet publish -c Release -o ./publish
```

Deploy the output from the `publish/` folder to:

- IIS  
- Azure App Services  
- Docker Container  
- Linux with Nginx/Apache

---

## Additional .NET Concepts

- **Model** – Represents the data (e.g., `User`, `Booking`)  
- **Controller** – Defines endpoints like `POST /api/bookings`  
- **Service** – Business logic (e.g., validating bookings, interacting with DB)  
- **Dependency Injection** – Services are injected into controllers for loose coupling  
- **Entity Framework Core** – ORM used to access and manage the database
