using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;
using UserIdentity.API.DatabaseContexts;
using UserIdentity.API.Extensions;
using UserIdentity.Core.DataTransferObjects;
using UserIdentity.Core.Entities;
using UserIdentity.Core.UserSecurity;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IConfigurationRoot config = builder.CreateAndBuildConfiguration();
builder.RegisterDbContext(config);
builder.RegisterAuthentication(config);
builder.RegisterDependencies(config);
WebApplication app = builder.Build();
app.MapGet("/", () => "Welcome!");

app.MapGet("/role/{id:int}", Results<Ok<GetRoleDTO>, NotFound> (AppDbContext dbContext, int id) => dbContext.Roles.SingleOrDefault(r => r.RoleId == id) is Role role
    ? TypedResults.Ok(role.MapGetRoleToDTO())
    : TypedResults.NotFound());
app.MapGet("/user/{id:int}", Results<Ok<GetUserDTO>, NotFound> (AppDbContext dbContext, int id) => dbContext.Users.Include(u => u.Roles).SingleOrDefault(u => u.UserId == id) is User user
    ? TypedResults.Ok(user.MapGetUserToDTO())
    : TypedResults.NotFound());

app.MapGet("/user/login", Results<UnauthorizedHttpResult, Ok<GetUserDTO>> (AppDbContext dbContext, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator, LoginUserDTO dto) =>
{
    //TODO: Validate login dto!
    User? user = dbContext.Users.Include(u => u.Roles)
       .SingleOrDefault(u => u.Username == dto.Username);

    if (user == null)
    {
        return TypedResults.Unauthorized();
    }

    bool isHashMatch = passwordHasher.VerifyHashMatch(user.PasswordHash, dto.Password, user.Salt);
    if (!isHashMatch)
    {
        return TypedResults.Unauthorized();
    }
    string token = tokenGenerator.GenerateToken(user.UserId, user.Username, user.Roles.Select(r => r.Role1));

    GetUserDTO authenticatedUser = user.MapGetUserToDTO();
    authenticatedUser.Token = token;
    
    return TypedResults.Ok(authenticatedUser);
});

app.MapGet("/role", Results<Ok<IEnumerable<GetRoleDTO>>, NotFound> (AppDbContext dbContext, ITokenGenerator tokenGenerator) =>
{
    IEnumerable<Role> roles = dbContext.Roles.AsEnumerable();
    return roles.Any() ? TypedResults.Ok(roles.MapGetRolesToDTO()) : TypedResults.NotFound();
});
app.MapGet("/user", Results<Ok<IEnumerable<GetUserDTO>>, NotFound> (AppDbContext dbContext) =>
{    
    IEnumerable<User> users = dbContext.Users.Include(u => u.Roles).AsEnumerable();
    return users.Any() ? TypedResults.Ok(users.MapGetUsersToDTO()) : TypedResults.NotFound();
});

app.MapPost("/role", Results<BadRequest<string>, Created<GetRoleDTO>> (AppDbContext dbContext, IValidator<CreateRoleDTO> validator, CreateRoleDTO dto) =>
{
    FluentValidation.Results.ValidationResult validationResult = validator.Validate(dto);
    if (!validationResult.IsValid)
    {
        return TypedResults.BadRequest(validationResult.ToString());
    }
    Role role = dto.MapCreateRoleDTOToEntity();
    dbContext.Roles.Add(role);
    dbContext.SaveChanges();
    return TypedResults.Created($"/role/{role.RoleId}", role.MapGetRoleToDTO());
});
app.MapPost("/user", Results<BadRequest<string>, Created<GetUserDTO>> (AppDbContext dbContext, IValidator<CreateUserDTO> validator, IPasswordHasher passwordHasher, CreateUserDTO dto) =>
{
    FluentValidation.Results.ValidationResult validationResult = validator.Validate(dto);
    if (!validationResult.IsValid)
    {
        return TypedResults.BadRequest(validationResult.ToString());
    }
    User user = dto.MapCreateUserDTOToEntity();
    PasswordHash hash = passwordHasher.ComputeHash(dto.Password);
    user.PasswordHash = hash.Password;
    user.Salt = hash.Salt;
    user.Roles = [.. dbContext.Roles.Where(r => dto.Roles.Select(r => r.RoleId).Contains(r.RoleId))];
    dbContext.Users.Add(user);
    dbContext.SaveChanges();
    return TypedResults.Created($"/user/{user.UserId}", user.MapGetUserToDTO());
});
app.MapPut("/role/{id:int}", Results<BadRequest<string>, NotFound, NoContent> (AppDbContext dbContext, IValidator<UpdateRoleDTO> validator, UpdateRoleDTO dto, int id) =>
{
    FluentValidation.Results.ValidationResult validationResult = validator.Validate(dto);
    if (!validationResult.IsValid)
    {
        return TypedResults.BadRequest(validationResult.ToString());
    }
    Role entity = dbContext.Roles.SingleOrDefault(r => r.RoleId == id)!;
    if (entity == null)
    {
        return TypedResults.NotFound();
    }
    dto.MapUpdateRoleDTOToEntity(entity);
    dbContext.SaveChanges();
    return TypedResults.NoContent();
});
app.MapPut("/user/{id:int}", Results<BadRequest<string>, NotFound, NoContent> (AppDbContext dbContext, IValidator<UpdateUserDTO> validator, IPasswordHasher passwordHasher, UpdateUserDTO dto, int id) =>
{
    FluentValidation.Results.ValidationResult validationResult = validator.Validate(dto);
    if (!validationResult.IsValid)
    {
        return TypedResults.BadRequest(validationResult.ToString());
    }
    User user = dbContext.Users.SingleOrDefault(u => u.UserId == id)!;
    if (user == null)
    {
        return TypedResults.NotFound();
    
    }
    dto.MapUpdateUserDTOToEntity(user);
    PasswordHash hash = passwordHasher.ComputeHash(dto.Password);
    user.PasswordHash = hash.Password;
    user.Salt = hash.Salt;
    user.Roles = [.. dbContext.Roles.Where(r => dto.Roles.Select(r => r.RoleId).Contains(r.RoleId))];
    dbContext.SaveChanges();
    return TypedResults.NoContent();
});
app.MapDelete("/role/{id:int}", Results<NotFound, NoContent> (AppDbContext dbContext, int id) =>
{
    Role role = dbContext.Roles.SingleOrDefault(r => r.RoleId == id)!;
    if (role == null)
    {
        return TypedResults.NotFound();
    }
    dbContext.Roles.Remove(role);
    dbContext.SaveChanges();
    return TypedResults.NoContent();
});
app.MapDelete("/user/{id:int}", Results<NotFound, NoContent> (AppDbContext dbContext, int id) =>
{
    User user = dbContext.Users.SingleOrDefault(u => u.UserId == id)!;
    if (user == null)
    {
        return TypedResults.NotFound();
    }
    dbContext.Users.Remove(user);
    dbContext.SaveChanges();
    return TypedResults.NoContent();
});



//app.MapCrudEndpoints();
app.Run();
