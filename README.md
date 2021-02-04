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

### install SendGrid & Twilio tooles to send emails and SMS
- Install **SendGrid package** from *NuGet* , last version and in **Infrastructure project**.
- install **Twilio package** from *NuGet*, last version in **Infrastructure project**.


### install package to work with Files
- ``Microsoft.AspNetCore.Http``, select last version in **Core Project**

### then:
```
dotnet restore
```
---

## Create Database contain all  Identities  Tables of EntityFrameworkCore Library
First Create a new class ``AppUser`` that inherits from ``IdentityUser`` class and add some columns we need inside **Core Project**, and creadte ``Address`` Table with one to one relation.

In **Infrastructure Project** create ``AppDbContext``, then inside ``setting.development.json`` add Connection Strings data:
```
"ConnectionStrings": {
"DbConnection": "Data source=standardDB.db"
}
```
then add connection service in ``startup.cs``  inside ``ConfigureServices`` methode in **API project**
```
services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlite(Configuration.GetConnectionString("DbConnection"));
});
```

###  ef Commands to work with database:
```
dotnet ef migrations add InitDB -p Infrastructure -s API -o Data/Migrations -c AppDbContext    // Create new Migration:

dotnet ef database update  -p Infrastructure -s API -c AppDbContext         // Update Database

dotnet ef migrations Remove -p Infrastructure -s API -c AppDbContext        // Remove last Migration

dotnet ef database drop -p Infrastructure -s API -c AppDbContext            // remove Database
```
---


## Add default Roles, languages, uploadTypes, SuperAdmin data. Add Models UploadType, Upload,... and some Services
in ``startup.cs`` class add :
```
services.AddIdentityCore<AppUser>() /*Add service for type 'Microsoft.AspNetCore.Identity.UserManager:*/
        .AddRoles<IdentityRole>() /*Add IdentityRole service in Application:*/
        .AddEntityFrameworkStores<AppDbContext>() /*to avoid error :Unable to resolve service for type 'Microsoft.AspNetCore.Identity.IUserStore`1 */ ; 
```
then:
- create ``Infrastructure.Identity.InitializeDefaultData`` class to Initialize Roles, SuperAdmin user
- in ``API/program.cs`` change ``CreateHostBuilder(args).Build().Run()`` structure to create Migrations then update database, and add Default Data like Roles, languages, uploadTypes and SuperAdmin.
- and change the ``Main`` method in ``API/program.cs`` project to **Task**

---

## Adding AddDefaultTokenProviders, AddSignInManager and Token service JWT(Json Web Token)
- inside ``startup.cs`` or inside extention file add these Services then add this file inside ``startup.cs`` file.

### Add token generation to generat token:
- first create ``ITokenService.cs`` inside **Core Project** then the ``TokenService.cs`` inside **Infrastructure.Services Project**,

### to work JWT:
- add ``app.UseAuthentication()``; pipline(Configure) method inside ``startup.cs``, directly before ``app.UseAuthorization();`` to work Authorization service

---

## Add Email(SendGrid API) and SMS(Twilio API) Sender Services

### SendGrid API) Send Verification, ResetPassword, ... -email
Create an account in SendGrud website 
- Create a key 
- Confirm an email to send messages from this email, using another email will not send messages
- add Key and email in ``appSettings.json`` for Securety:
```
"SendGridAPI":{ 
    "SendGridApiKey": "SG.s4zGLBIKRoqg1HosQtGrvg.IXi -...", 
    "OutputEmail": "example@example.com" 
}
```

### Twilio API) Send SMS to Mobile Number
- Create accoute in https://www.twilio.com/
- add Key and email in ``appSettings.json`` for Securety:
```
"SendSmsTwilioAPI":{
    "AccountSID":"AC61eb3b69558ead4...",
    "AuthToken": "912409bf521b9516c...",
    "TwilioPhoneNumber":"+1251359..."
}
```
- Create ``EmailSmsSenderService`` class inside **Infrastructure Project** then add this class as a Service in ``Startup.cs`` class: ``services.AddScoped<EmailSmsSenderService>();``

---
## add AddSwagger services and pipline
create ``AddSwaggerDocumentation.cs`` inside ApiServices folder
add inside ``appsettings.Development.json`` file:
```
  "ConstantNames": {
    "APIProjectName": "WebApplication",
    "APIVersion": "v1",
    "SwaggerAPINameAndVirsion": "WebApplication API v1",
    "SwaggerURL": "/swagger/v1/swagger.json"
  }
```
then above ``app.UseEndpoints(endpoints`` inside ``startup.cs`` file add ``app.UseSwaggerDocumention(Configuration);``
---


## Handling API Response, Validation, Server500, NotFound & BadRequest -Errors
inside ErrorsHandlers
- create BuggyController to test all Errors
- create ``ApiResponse.cs`` to return  NotFound(), BadRequest() errors response throw it.

### to handel Endpoint not found
- create a ``ErrorController.cs`` from ``startup.cs`` redirect the error to this controller by adding:
```
app.UseStatusCodePagesWithReExecute("/errors/{0}");
```
if request commes into API Server don't have and EndPoint match that request, this middleware redirect to ``ErrorController.cs ``

### handel Server Error 500
- create ``ApiException.cs`` class inside ErrorsHandlers Folder.
- Create ``ExceptionMiddleware.cs`` class to handel Exception in **prduction** or **development** mode  inside ErrorsHandlers Folder.
- add in Middleware the new ``ExceptionMiddleware.cs`` instead the old one:
```
// handling exceptions just in developer mode.
// if (env.IsDevelopment())
// {
//     app.UseDeveloperExceptionPage();
// }
app.UseMiddleware<ExceptionMiddleware>();
```

### Handling Error, Validation Error, override the [ApiContoller] error response behvior
hese validation responses are typically going to be generated when the user **submitting a form** with some data on it, and some data are missed or the not right type.

for that what we want to do is flatten these Errors out, that we have our status code our message, and then an array, for example:
```
"errors": [
        "field (Password) is required",
        "field (Username or Email) is required"
],
"statusCode": 400,
"message": "you have made a bad request!"
```

create ApiValidationErrorResponse.cs inherent from ApiResponse then override the behavior of ``[ ApiController ]``, then inside ``startup.cs`` or inside extention file add these Service:
```
services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage).ToArray();
        var errorResponse = new ApiValidationErrorResponse
        {
            Errors = errors
        };
        return new BadRequestObjectResult(errorResponse);
    };
});
```
---
## Add Gineric Rrpository patern and specification patern for all Entities in Applecation
Gineric Repository consider an Anti-pattern, to get around this proplem we use specification patern, this pattern allow as to do
- Describes a query in an object. Instead ofjust passing an expression to defind a sync method, we could define a specification object which contains the query we want to send to that Method
- Retuns an IQueryable<T>.
- Generic List method takes specification as parameter.
- Specification can have meaningful name.

### how it works:
we would create a specification and that says in our specification: we needed all of the Blogs with 'read' in the Blog's name and include the Blog Category.

This specification would return and ``IQueryable<T>`` in this case if the **T** would be Blog and we passed that specification as a parameter to a method in our generic repository called for example ListAsync(), that instead of taking a generic expression it takes a specification class, and because we can name this specification with a meaningful name, this allows us to use our generic repository without needing to derive from this repository additional derived more specific repositories, 
and instead, we can control the data that we're returning from our database with specifications.

And even if we've got 100 entities or three entities we still don't need to create any more additional repositories. 
But when we do need a specific subset of data from our database, we simply create a specification and then pass that as a parameter to our list. 

### after create interface and class for Generic Blog Repository, create:
- ``ISpecification`` Interface
- ``BaseSpecification.cs``
- ``BlogsWithCategoriesSpecification.cs``




---

