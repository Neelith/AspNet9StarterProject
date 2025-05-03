# AspNet9StarterProject
Base project used as a starting template for ASP.NET Web Api application

To add a migration or run a database update you have to specify project and startup project, like this:
dotnet ef migrations add AddForecastsTable --project src/YourProjectName.Infrastructure/YourProjectName.Infrastructure.csproj --startup-project src/YourProjectName.WebApi/YourProjectName.WebApi.csproj -o Persistence/Migrations