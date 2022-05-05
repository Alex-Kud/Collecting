using Collecting.Data;
using Collecting.Services.Implimentations;
using Collecting.Services.Interfaces;
using Games.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<StickersContext>(options => options.UseSqlServer(configuration["ConnectionDB"]));
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Фруктотитулофилия",
        Description = "Демонстрация API для католога коллекционера",
        Contact = new OpenApiContact()
        {
            Name = "Александр Кудашов",
            Email = "sasha.kudaschov2014@yandex.ru"
        },
        License = new OpenApiLicense
        {
            Name = "Александр Кудашов",
            Url = new Uri("https://vk.com/alex_kudashov")
        }
    });
    // To Enable authorization using Swagger (JWT)
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Используется JWT авторизация. \r\n\r\n Введите 'Bearer' [пробел] и свой токен в текстовое поле ниже.\r\n\r\nНапример: \"Bearer 12345abcdef\"",
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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
});


// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,

        ValidAudience = configuration["Jwt:Audience"],
        ValidIssuer = configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
    };
});

builder.Services.AddTransient<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<JWTMiddleware>();

app.MapControllers();
/*
DefaultFilesOptions DefaultFile = new DefaultFilesOptions();
DefaultFile.DefaultFileNames.Clear();
DefaultFile.DefaultFileNames.Add("wwwroot/index.html");

app.UseDefaultFiles(DefaultFile);


app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new
     List<string> { "index.html" }
});

*/

app.UseStaticFiles();

app.Run();
