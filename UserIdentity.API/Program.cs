using UserIdentity.API.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IConfigurationRoot config = builder.CreateAndBuildConfiguration();
builder.RegisterDbContext(config);
builder.RegisterAuthentication(config);
builder.RegisterDependencies(config);
WebApplication app = builder.Build();
app.MapEndpoints();
app.Run();
