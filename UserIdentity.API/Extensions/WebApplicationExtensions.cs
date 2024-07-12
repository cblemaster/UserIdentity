using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using UserIdentity.Core.DataTransferObjects;
using UserIdentity.Core.Entities;
using UserIdentity.Core.Interfaces;
using UserIdentity.Core.UserSecurity;
using UserIdentity.Core.Validation;
using DbContext = UserIdentity.API.DatabaseContexts.AppDbContext;

namespace UserIdentity.API.Extensions;

internal static class WebApplicationExtensions
{
    internal static IConfigurationRoot CreateAndBuildConfiguration
        (this WebApplicationBuilder builder) =>
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .Build();

    internal static void RegisterDbContext(this WebApplicationBuilder builder, IConfigurationRoot config)
    {
        string connectionString = config.GetConnectionString("Project") ?? "Error retrieving connection string!";
        builder.Services.AddDbContext<DbContext>(options => options.UseSqlServer(connectionString));
    }

    internal static void RegisterAuthentication(this WebApplicationBuilder builder, IConfigurationRoot config)
    {
        string jwtSecret = config.GetValue<string>("JwtSecret") ?? "Error retreiving jwt config!";
        byte[] key = Encoding.ASCII.GetBytes(jwtSecret);
        builder.Services.AddAuthentication(x =>
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[JwtRegisteredClaimNames.Sub] = "sub";

            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                NameClaimType = "name"
            };
        })
        .Services.AddAuthorizationBuilder()     // TODO: Ideally these roles should be read from the db
            .AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"))
            .AddPolicy("CustomerPolicy", policy => policy.RequireRole("Customer"))
            .AddPolicy("VendorPolicy", policy => policy.RequireRole("Vendor"))
            .AddPolicy("SalesPolicy", policy => policy.RequireRole("Sales"))
            .AddPolicy("MarketingPolicy", policy => policy.RequireRole("Marketing"))
            .AddPolicy("ReportingPolicy", policy => policy.RequireRole("Reporting"))
            .AddPolicy("SupportPolicy", policy => policy.RequireRole("Support"));
    }

    internal static void RegisterDependencies(this WebApplicationBuilder builder, IConfigurationRoot config)
    {
        string jwtSecret = config.GetValue<string>("JwtSecret") ?? "Error retreiving jwt config!";
        builder.Services
            .AddSingleton<ITokenGenerator>(tk => new JwtGenerator(jwtSecret))
            .AddSingleton<IPasswordHasher, PasswordHasher>()
            .AddTransient<ICreateUserDTOValidator, CreateUserDTOValidator>()
            .AddTransient<IUpdateUserDTOValidator, UpdateUserDTOValidator>()
            .AddTransient<ICreateRoleDTOValidator, CreateRoleDTOValidator>()
            .AddTransient<IUpdateRoleDTOValidator, UpdateRoleDTOValidator>();
    }

    private static GetUserDTO MapUserEntityToGetUserDTO(User user)
    {
        List<GetRoleDTO> roles = [];
        foreach (Role role in user.Roles)
        {
            roles.Add(new(role.RoleId, role.Role1));
        }
        return new GetUserDTO(user.UserId, user.Username, user.FirstName, user.LastName, user.Email, user.Phone, user.CreateDate, user.UpdateDate, roles.AsEnumerable());
    }
}
