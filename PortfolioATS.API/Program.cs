using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PortfolioATS.Core.Interfaces;
using PortfolioATS.Core.Models;
using PortfolioATS.Infrastructure;
using PortfolioATS.Infrastructure.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configura��o do CORS - ADICIONAR ESTA SE��O
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularLocalhost", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",    // Angular dev server padr�o
                "https://localhost:4200",   // Angular com HTTPS
                "http://localhost:4201",    // Porta alternativa
                "https://localhost:4201"    // Porta alternativa com HTTPS
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });

    // Pol�tica mais permissiva para desenvolvimento
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configura��o do MongoDB
builder.Services.AddInfrastructure(builder.Configuration);

// Configura��o do JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);

// Servi�os de aplica��o
builder.Services.AddScoped<IAuthService, AuthService>();

// Configura��o da autentica��o JWT
var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Configura��o do Swagger com JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PortfolioATS API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PortfolioATS API v1");
        c.RoutePrefix = string.Empty; // Coloca o Swagger na raiz
    });

    // Usar CORS mais permissivo em desenvolvimento
    app.UseCors("AllowAll");
}
else
{
    // Em produ��o, usar pol�tica mais restrita
    app.UseCors("AllowAngularLocalhost");
}

app.UseHttpsRedirection();

// Adicionar CORS - IMPORTANTE: Deve vir antes de Authentication e Authorization
app.UseCors("AllowAngularLocalhost");

// Adicionar autentica��o e autoriza��o
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();