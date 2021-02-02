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
## Setting up EntityFramwork ORM (Object Relational Mapper), install dotnet-ef tool & and Token Auth, and More...
### Install Libraries:
install some identity packages into projects from NuGet Manager, to get RunTime-SDK version, or any info: ```> dotnet --info``` 
### for test in Database :
- install ``Microsoft.EntityFramework.Core.Sqlite`` in **Infrastructure** Classlib, Select version the same Runtime machin/SDKs version

### Infrastructure Project:
- install ``Microsoft.AspNetCore.Identity`` in **Infrastructure** Classlib, Select Latest version 
- install ``Microsoft.AspNetCore.Identity.EntityFrameworkCore`` in **Infrastructure** Classlib, Select version the same Runtime machin/SDKs version
- install ``Microsoft.IdentityModel.Tokens`` in **Infrastructure** Classlib, Latest version
- install ``System.IdentityModel.Tokens.Jwt`` in **Infrastructure** Classlib, the same version of **Microsoft.IdentityModel.Tokens**

### API Project:
- install ``Microsoft.AspNetCore.Authentication.JwtBeare`` in **API** Project, Select version the same Runtime machin/SDKs version
- install ``Microsoft.EntityFrameworkCore.Design ``in **API** Project, Select version the same Runtime machin/SDKs version

### Core Project:
we needed to add a ``Microsoft.Extensions.Identity.Stores`` package inside Core projects, because some classes in Core Project will derive from an IdentityUser class, which is available inside this package:
- install ``Microsoft.Extensions.Identity.Stores`` in **Core** Classlib, Select version the same Runtime machin/SDKs version

### add Mapper Tooles
add a helper tool so that we don't need to manually convert an entity into a DTO every time cr vice versa.
- install ``AutoMapper.Extensions.Microsoft.DependencyInjection``, select last version and select the **API, Core and Infrastructure projects**.
- in **Core** create a **CoreHelppers** folder
- Create: ``Core/Helppers/MappingProfiles.cs``
- add **MappingProfiles** class as a Servises ``services.AddAutoMapper(typeof(MappingProfiles));`` in ``startup.cs`` in API

---
*note: maybe display an error if ``Microsoft.AspNetCore.Identity.EntityFrameworkCore`` 3.1.1 is not compatible witht netstandard2.0, to solve this Error inside ``Core.csproj`` and ``Infrastructure.csproj`` & ``Core.csproj``, change < TargetFramework > from **netstandard2.0** to **netstandard2.1** then restore the Solution

If ef command-line tool of EntityFramework not installed, Install it by this command line:  ```dotnet tool install --global  dotnet-ef --version <Runtime version/host>```


### install Swagger Tooles for APIs to Generate API documentation.
- ``Swashbuckle.AspNetCore.SwaggerGen``, select last version and in **API Project**
- ``Swashbuckle.AspNetCore.SwaggerUI``, select last version and in **API Project**

### then:
```
dotnet restore
```