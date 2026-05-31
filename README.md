# Client Manager - ASP.NET Core MVC

A robust study project focused on **ASP.NET Core MVC**, **Entity Framework Core**, and enterprise backend architectural patterns.

## 🚀 Technologies and Libraries

* **Framework:** ASP.NET Core MVC (with Razor Engines)
* **ORM:** Entity Framework Core
* **Database:** SQL Server
* **UI/UX Enhancements:** SweetAlert2 (Modern Modals) & Bootstrap

## ✨ Key Features

* **Client & User CRUD:** Full lifecycle management of client records paired with backend user account creation.
* **Role-Based Authorization:** Strict security implementation using ASP.NET Identity Claims and Cookies. UI actions (like deletion) are dynamically toggled based on the user's `Admin` role.
* **Many-to-Many Relationship Synchronization:** Dynamic access profile management (`User` ↔ `Profiles`) with state tracking to safely add or remove entity relationships without duplication.
* **Soft Delete Pattern:** Implemented a non-destructive delete workflow using EF Core **Global Query Filters** (`HasQueryFilter`) to automatically exclude deactivated records from standard queries.
* **Self-Deletion Protection (Anti-Autofagia):** Server-side and client-side validation logic that cross-checks the active session's `ClaimsPrincipal` to block logged-in administrators from deleting their own accounts.
* **Data Integrity & Security:** Built-in unique constraints (e.g., Duplicate Email Prevention) and full protection against Cross-Site Request Forgery (CSRF) via `[ValidateAntiForgeryToken]`.

## 🏗️ Architecture & Design Patterns

The project is designed with a strict separation of concerns, moving away from fat controllers to an enterprise-ready layered architecture:
* **Controller Layer:** Manages HTTP requests, handles UI workflows, and enforces initial state validations.
* **Service Layer:** Houses the core business logic, validation rules, and handles explicit Model-to-DTO data mapping.
* **Repository Layer:** Abstracted database access using the Repository Pattern to decouple the DB Context from business operations.

---

## ⚙️ How to run the project

1. **Clone the repository**
   ```bash
   git clone [https://github.com/JoaoPauloDonatoJ/CrudMvcClients.git](https://github.com/JoaoPauloDonatoJ/CrudMvcClients.git)
   ````
2. **Navigate to the project folder:**
   
   Example:
   ```bash
   cd ClientManager
    
3. **Configure the database connection (User Secrets)**
   
   This project utilizes User Secrets to safely isolate sensitive configuration parameters from the source code.
   
   Initialize and set your SQL Server connection string:
   ```bash
   dotnet user-secrets init
   ```
   Now set your connection string:
   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "YOUR_CONNECTION_STRING"
   ```
   Example:
   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=YOUR_SERVER;Database=YOUR_DB;Trusted_Connection=True;TrustServerCertificate=True"
   ```
4. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```
   This will create the database and tables automatically.
   
5. **Run the application**
   ```bash
   dotnet run
   ```
   The application will start locally.
   The URL will be displayed in the console.

   Example Default URL:
   ```bash
   https://localhost:5001
   ```

   
