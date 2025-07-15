<<<<<<< HEAD
ChatApp Build
1. Dowload thu vien:
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.Cors
dotnet add package Serilog.AspNetCore
dotnet add package MailKit
2. Conect DB and Create Models or DbContext
	1. Create floder Models
	2. Add on Appsetting: 
	"ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ChatAppDb;User Id=sa;Password=sa;TrustServerCertificate=True;"
    }
	3. Run : Scaffold-DbContext Name=DefaultConnection Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context AppDbContext -DataAnnotations



=======
# chat-app
>>>>>>> c4593634a2fd39046752d728ef76b0fdbbeb4684
