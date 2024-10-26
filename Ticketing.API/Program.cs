using Microsoft.EntityFrameworkCore;
using Ticketing.API.Data;
using Ticketing.API.Services;
using DotNetEnv;
using Ticketing.API.Data.Seeder;

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load();
System.Diagnostics.Debug.WriteLine("Hello APP");

System.Diagnostics.Debug.WriteLine(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"));
builder.Configuration.AddEnvironmentVariables();
// Add services to the container.

//Enable Cors
builder.Services.AddCors( options =>
{
    options.AddPolicy("AllowAllPolicy", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

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

app.UseCors("AllowAllPolicy");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Welcome to Ticketing API");
if (args.Contains("seed"))
{
    // Run the seeding process
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        //seed user first
        var userContext = services.GetRequiredService<TicketingAuthDbContext>();
        userContext.Database.Migrate();
        await UserDbSeed.Initialize(services);

        //seed data for other domains
        var context = services.GetRequiredService<TicketingDbContext>();
        context.Database.Migrate(); // Applies any pending migrations
        DbSeed.Initialize(context);  // Calls the seed method

        
    }

    Console.WriteLine("Database seeding completed.");
    Environment.Exit(0); // Ensure app exits after seeding
}
app.Run();
