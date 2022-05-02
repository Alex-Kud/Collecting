using Collecting.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<StickersContext>(options => options.UseSqlServer(configuration["ConnectionDB"]));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

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
