# Base Projects

## Add Add main Solution Structure and First Projects:
```
dotnet new sln
dotnet new webapi -o API
dotnet new Classlib -o Infrastructure
dotnet new Classlib -o Core
```

### Add all projects to solution:
```
dotnet sln add webapi
dotnet sln add Infrastructure
dotnet sln add Core
```
---
### Create dependencies/Reference:
the API project need a Reference the Infrastructure on API, Infrastructure will contain Classes like DbContext 
```
dotnet add API/API.csproj reference Infrastructure
dotnet add Infrastructure/Infrastructure.csproj reference Core
```
### then :
```
dotnet restore
```
---