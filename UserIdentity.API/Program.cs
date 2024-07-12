using UserIdentity.API.DatabaseContexts;
using UserIdentity.API.Extensions;
using UserIdentity.Core.DataTransferObjects;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IConfigurationRoot config = builder.CreateAndBuildConfiguration();
builder.RegisterDbContext(config);
builder.RegisterAuthentication(config);
builder.RegisterDependencies(config);
WebApplication app = builder.Build();
app.MapGet("/", () => "Welcome!");

// get role
app.MapGet("/role/{id:int}", (AppDbContext dbContext, int id) => { });
// get user
app.MapGet("/user/{id:int}", (AppDbContext dbContext, int id) => { });

// list role
app.MapGet("/role", (AppDbContext dbContext) => { });
// list user
app.MapGet("/user", (AppDbContext dbContext) => { });

// create role
app.MapPost("/role", (AppDbContext dbContext, CreateRoleDTO dto) => { });
// create user
app.MapPost("/user", (AppDbContext dbContext, CreateUserDTO dto) => { });

// update role
app.MapPut("/role/{id:int}", (AppDbContext dbContext, UpdateRoleDTO dto, int id) => { });
// create user
app.MapPut("/user/{id:int}", (AppDbContext dbContext, UpdateUserDTO dto, int id) => { });

// update role
app.MapDelete("/role/{id:int}", (AppDbContext dbContext, int id) => { });
// create user
app.MapDelete("/user/{id:int}", (AppDbContext dbContext, int id) => { });

// register
// login

//app.MapCrudEndpoints();
app.Run();
