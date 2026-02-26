# Client Manager - ASP.NET Core MVC

A study project focused on **ASP.NET Core MVC**, **Entity Framework Core**, and backend best practices.

## 🚀 Technologies
- **Framework:** ASP.NET Core MVC
- **ORM:** Entity Framework Core
- **Database:** SQL Server
- **Frontend Engine:** Razor Pages

## ✨ Features
- **Client CRUD:** Full management (Create, Read, Update, Delete) of client records.
- **Data Validation:** Server-side and client-side validation for data integrity.
- **Duplicate Email Prevention:** Logic to ensure unique user registration.
- **Auto-logging:** Automatic registration of the creation date for each client.

## 🏗️ Architecture
The project follows the **MVC (Model-View-Controller)** pattern to ensure a clean separation of concerns.

## ⚙️ How to run the project
1. **Clone the repository:**
   ```bash
   git clone https://github.com/JoaoPauloDonatoJ/CrudMvcClients.git

2. **Navigate to the project folder:**
   
   Example:
   ```bash
   cd ClientManager
    
4. **Configure the database connection (User Secrets)**
   
   This project uses User Secrets to store sensitive information like connection strings.
   
   Initialize user secrets (if needed):
   ```bash
   dotnet user-secrets init
   ```
   Now set your connection string:
   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "YOUR_CONNECTION_STRING"
   ```
   Example:
   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=WebApplicationTest;Trusted_Connection=True;TrustServerCertificate=True"
   ```
6. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```
   This will create the database and tables automatically.
   
7. **Run the application**
   ```bash
   dotnet run
   ```
   The application will start locally.
   The URL will be displayed in the console.

   Example Default URL:
   ```bash
   https://localhost:5001
   ```

   
