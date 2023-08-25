# URL Goat - URL Shortener Project

URL Goat is a straightforward URL shortener project crafted with ASP.NET Core 6 on the backend and Angular on the frontend. This application empowers you to condense long URLs into concise, user-friendly links.

## Features

- URL shortening: Easily convert long URLs into short links.
- Validation: Validate URLs before shortening to ensure they are valid.
- Error handling: Gracefully handle errors and display user-friendly messages.
- Entity Framework Core: The project uses Entity Framework Core for database operations.
- AutoMapper: AutoMapper is used for object mapping.

## Prerequisites

Before you begin, make sure you have the following dependencies installed on your machine:

### For the Backend (ASP.NET Core 6)

1. **Visual Studio**: Download and install [Visual Studio](https://visualstudio.microsoft.com/vs/).

2. **SQL Server Management Studio (SSMS)**: Download and install [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16).

3. **Git**: Install Git by following the instructions [here](https://github.com/git-guides/install-git).

4. **Visual Studio Code**: Download and install [Visual Studio Code](https://code.visualstudio.com/).

5. .NET SDK 6.0: You need the .NET SDK 6.0 installed. You can download it from [here](https://dotnet.microsoft.com/download/dotnet/6.0).
### For the Frontend (Angular)

1. **Node.js and npm**: If you don't have Node.js and npm installed, download and install them from [here](https://nodejs.org/).

2. **Angular CLI**: Install Angular CLI globally using npm:

   ```sh
   npm install -g @angular/cli
   ```


## Getting Started

Follow these steps to set up and run the project locally:

### Backend (ASP.NET Core)

1. Clone this repository:

git clone https://github.com/your-username/url-goat.git


2. Navigate to the backend directory:

cd url-goat-backend

3. Configure the database

- Open the `appsettings.json` file and update the database connection string as needed. By default, it's configured for SQL Server.
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=your-server;Database=YourDatabase;User=YourUser;Password=YourPassword;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
}
```
4. Initialize the database with seed data:
- Open the Package Manager Console in Visual Studio IDE
- a.  Add-Migration InitialCreate 
- b. Add-Migration NewMigrationName
- c. Update-Database
- d. Go to a terminal 
  - i. Run the following command
  ```bash
      dotnet run seeddata
  ```

5. Restore packages and run the ASP.NET Core application:

dotnet restore
dotnet run

The API will be available at `https://localhost:7203`.

### Frontend (Angular)

1. Navigate to the frontend directory:


2. Install dependencies:

``` bash
        npm install
```

3. Start the Angular development server:
```
ng serve -o
```

The frontend will be available at `http://localhost:4200`.

Note: If you encounter any issues during the installation process, make sure you have Node.js and npm (Node Package Manager) installed. You can download them from [here](https://nodejs.org/).

## Usage

1. Access the frontend application at `http://localhost:4200`.
2. Enter a long URL and click the "Shorten" button.
3. The shortened URL will be displayed if the URL is valid.