using Microsoft.EntityFrameworkCore;
using UzTelecom_Quiz.Data;
using UzTelecom_Quiz.Interface;
using UzTelecom_Quiz.Models;
using UzTelecom_Quiz.Repository;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUser, UserRepository>(); 
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
