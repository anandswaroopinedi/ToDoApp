using ToDoAppWebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder = builder
    .AddJwtAuthentication()
    .AddCorsPolicy()
    .AddRepositories()
    .AddManagers()
    .AddOtherServices()
    .AddSwaggerDocumentation();
var app = builder.Build();
app.UseCustomSwagger().ConfigureCustomMiddleware().Run();
