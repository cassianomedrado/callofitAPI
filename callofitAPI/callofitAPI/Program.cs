using callofitAPI.Interfaces;
using callofitAPI.Security.DAO;
using callofitAPI.Security.Service;
using callofitAPI.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using netbullAPI.Middleware;
using netbullAPI.Security.MidwareDB;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("TokenConfigurations").GetSection("JwtKey").Value);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CallofITAPI",
        Version = "v1",
        Description = "ASP.NET Core Web API para controle de chamados de suporte de TI",
        TermsOfService = new Uri("https://example.com/terms")
    });

    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = @"Cabeçalho de autorização JWT usando o esquema Bearer.
                        Digite 'Bearer' [espaço] e então seu token na entrada de texto abaixo.
                        Exemplo:'Bearer 12345abcdef' "
    });

    s.OperationFilter<AuthResponsesOperationFilter>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy",
        builder => builder.WithOrigins("https://localhost:4200", "http://localhost:4200")
    .AllowAnyHeader()
    .AllowAnyMethod());
});

builder.Services.AddAuthentication(authOptions =>
{
    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(bearerOptions =>
{
    bearerOptions.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false
    };
});

// INJEÇÃO DE DEPENDENCIAS
builder.Services.AddScoped<TipoUsuarioMW>(); 
builder.Services.AddScoped<UsuarioMW>();
builder.Services.AddScoped<INotificador, NotificadorMW>();
builder.Services.AddTransient<TipoUsuarioDAO>();
builder.Services.AddTransient<UsuarioDAO>();
builder.Services.AddTransient<TokenService>();

var app = builder.Build();

app.UseCors(builder => builder
.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader()
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
