using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SeamsApp.Data;
using SeamsApp.Interfaces.Services.Commands;
using SeamsApp.Interfaces.Services.Helper;
using SeamsApp.Models;
using SeamsApp.Seeders;
using SeamsApp.Services.Commands;
using SeamsApp.Services.Helper;
using SeamsApp.Utilities;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.Scan(scan => scan
    .FromAssemblyOf<Program>()

    .AddClasses(classes => classes.AssignableTo<IServiceCollection>())
        .AsImplementedInterfaces()
        .WithScopedLifetime()

    .AddClasses(classes => classes
        .WithAttribute<TransientServiceAttribute>())
        .AsImplementedInterfaces()
        .WithScopedLifetime()

     .AddClasses(classes => classes
        .WithAttribute<ScopedServiceAttribute>())
        .AsImplementedInterfaces()
        .WithScopedLifetime()

     .AddClasses(classes => classes
        .WithAttribute<SingletonServiceAttribute>())
        .AsImplementedInterfaces()
        .WithScopedLifetime()
 );

builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(15); 
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("https://seamsweb.vercel.app")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,    
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? ""))
        };
    });
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SEAMS",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste JWT token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "SeamsApp.xml"));
});
builder.Services.AddAutoMapper(option =>
{
    option.AddProfile<AutoMapperProfiles>();
});

builder.Services.AddDbContext<SeamsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddProblemDetails();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        AdminSeeder.SeedAdmin(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Admin seeding failed");
    }
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseOutputCache();
app.MapControllers();
app.Run();
