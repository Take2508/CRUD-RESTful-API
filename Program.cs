using Microsoft.EntityFrameworkCore;
using FastDeliveryAPI.Data;
using FastDeliveryAPI.Repositories.Interfaces;
using FastDeliveryAPI.Repositories;
using FastDeliveryAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUnitOfWorks, UnitOfWork>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();




var connectionString = builder.Configuration.GetConnectionString("MyDbPgsql");

builder.Services.AddDbContext<FastDeliveryDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        b => b.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
