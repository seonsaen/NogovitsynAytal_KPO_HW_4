using gozon.services.orders.Consumers;
using gozon.services.orders.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PaymentProcessedConsumer>();

    x.AddEntityFrameworkOutbox<OrderDbContext>(o =>
    {
        o.UsePostgres();
        o.UseBusOutbox(); 
    });

    x.UsingRabbitMq((context, cfg) =>
    {

        cfg.Host(builder.Configuration["RabbitMq:Host"] ?? "localhost", "/", h => {
            h.Username("guest");
            h.Password("guest");
        });
        
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddControllers();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    var dbContext = services.GetRequiredService<OrderDbContext>(); 

    for (int i = 0; i < 10; i++)
    {
        try
        {
            dbContext.Database.EnsureCreated();
            logger.LogInformation("Database connected and created successfully.");
            break;
        }
        catch (Npgsql.NpgsqlException)
        {
            if (i == 9) throw;
            
            logger.LogWarning($"Database not ready yet. Retrying in 3 seconds...");
            System.Threading.Thread.Sleep(3000);
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();