using Microsoft.EntityFrameworkCore;
using Ticketing.API.Data;
using Ticketing.API.Services;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load();
System.Diagnostics.Debug.WriteLine("Hello APP");

System.Diagnostics.Debug.WriteLine(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"));
builder.Configuration.AddEnvironmentVariables();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add db context
builder.Services.AddDbContext<TicketingDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TicketingConnectionString")));
builder.Services.AddDbContext<TicketingAuthDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TicketingConnectionString")));


//add app services
builder.Services.RegisterService(builder);

var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Welcome to Ticketing API");
app.Run();
