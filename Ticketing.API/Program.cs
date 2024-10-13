using Microsoft.EntityFrameworkCore;
using Ticketing.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add db context
builder.Services.AddDbContext<TicketingDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TicketingConnectionString")));
builder.Services.AddDbContext<TicketingAuthDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TicketingConnectionString")));


var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
