using Microsoft.EntityFrameworkCore;
using WakeTest.API.Configurations;
using WakeTest.Domain.Entities;
using WakeTest.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
    );
// Add services to the container.
builder.Services.AddApplicationServices();
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

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dbContext.Database.Migrate();
    InsertingDataInDatabase(dbContext);
}

app.Run();

void InsertingDataInDatabase(DataContext dbContext)
{
    var test = !dbContext.Products.Any();
    if (test)
    {
        dbContext.Products.AddRange(new List<Product>
        {
            new Product { Name = "Sabão FEBO Laranja", Stock = 999, Value = (float)1.99 },
            new Product { Name = "CD de Samba - Maguila", Stock = 100, Value = (float)100.10 },
            new Product { Name = "Manteiga Presidente", Stock = 1200, Value = (float)10.50 },
            new Product { Name = "Pote 50gramas Nutella", Stock = 600, Value = (float)750.01 },
            new Product { Name = "Caderno Tilibra Surf", Stock = 10000, Value = (float)5.50 }
        }
        );
        dbContext.SaveChanges();
    }
}