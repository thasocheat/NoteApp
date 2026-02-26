## Demo

### Application Demo

![Watch Demo Part01](https://github.com/user-attachments/assets/c208bb3c-3042-4657-839c-283a117838a8)

![Watch Demo Part02](https://github.com/user-attachments/assets/146fd7bb-9089-444f-a90e-ce1ee5b754d7)

Or

![Releases Demo-Videos](https://github.com/thasocheat/NoteApp/releases/tag/Demo-Videos)

---

### Screenshots

<p align="center">
  <img src="Demo%20images/1.jpg" width="45%">
  <img src="Demo%20images/2.jpg" width="45%">
</p>

<p align="center">
  <img src="Demo%20images/3.jpg" width="45%">
  <img src="Demo%20images/4.jpg" width="45%">
</p>

<p align="center">
  <img src="Demo%20images/5.jpg" width="45%">
  <img src="Demo%20images/6.jpg" width="45%">
</p>

<p align="center">
  <img src="Demo%20images/7.jpg" width="45%">
  <img src="Demo%20images/8.jpg" width="45%">
</p>


# NoteApp

## Getting Started
## NuGet Packages Installed (Backend: framework net8.0)
```bash
dotnet add package Dapper &&
dotnet add package BCrypt.Net-Next &&
dotnet add package Microsoft.Data.SqlClient &&
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.13 &&
dotnet add package System.IdentityModel.Tokens.Jwt --version 8.0.2 &&
```

1. Run
```bash
cd backend/NotesApp.API
dotnet restore && dotnet build && dotnet run
```

## Frontend
1. Install npm packages
```bash
cd frontend/notes-frontend
npm install && npm run build && npm run dev
```

## Option A

1. Docker SQL Server
2. Install Docker Desktop
3. Verify in terminal:
   ```bash
   docker --version
   ```
4. Pull and run SQL Server in Docker
5. Open a terminal (PowerShell or CMD) and run:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrongPWS" -p 1433:1433 --name notesapp-sql -d mcr.microsoft.com/mssql/server:2022-latest
```

6. Verify container is running
```bash
docker ps
```

7. If the container already exists (every day use)

```bash
docker start notesapp-sql
```
8. Connect VS Code mssql Extension to Docker
    - Open the mssql connection panel
    - Create a new connection profile
    - Run the Migration (Create Database + Tables)
    - Open the migration file
    - In VS Code, open:
    ```
    NotesApp/backend/migrate.sql
    ```
9. Connect to master database first
10. Reconnect mssql to NotesAppDb

## Configure the backend
- Open `backend/NotesApp.API/appsettings.json` and update:

**For local SQL Server:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=NotesAppDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Secret": "YourVeryStrongSecretKey_NotesApp_2025!",
    "Issuer": "NotesApp",
    "Audience": "NotesApp",
    "ExpiryHours": "24"
  },
  "AllowedOrigins": "http://localhost:5173"
}
```

**For Docker SQL Server:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=NotesAppDb;User Id=sa;Password=YourStrongPWS;TrustServerCertificate=True;"
  }
}
```


    
## Option B

1. Clone the project.

```bash
  git clone https://github.com/thasocheat/NoteApp.git
```

2. Install SQL Server Locally && Download SQL Server Management Studio (SSMS)
   - Open SSMS → connect with:
   - Server name: `localhost` or `localhost\SQLEXPRESS`
   - Authentication: `Windows Authentication`
   - Verify SQL Server is running
3. Connection string for local SQL Server

```json
"DefaultConnection": "Server=localhost;Database=NotesAppDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

If you installed the Express edition (named instance):
```json
"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=NotesAppDb;Trusted_Connection=True;TrustServerCertificate=True;"
```
