using Microsoft.EntityFrameworkCore;
using ProSpaceTest.Core.Abstractions;
using ProSpaceTest.DataAccess;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ShopDbContext>(options =>
options.UseSqlite("Data Source=shop.db"));
builder.Services.AddScoped<ICustomerRepository, ProSpaceTest.DataAccess.Repository.CustomerRepository>();
builder.Services.AddScoped<ICustomerService, ProSpaceTest.Application.Services.CustomerService>();

builder.Services.AddScoped<IItemRepository, ProSpaceTest.DataAccess.Repository.ItemRepository>();
builder.Services.AddScoped<IItemService, ProSpaceTest.Application.Services.ItemService>();

builder.Services.AddScoped<IOrderRepository, ProSpaceTest.DataAccess.Repository.OrderRepository>();
builder.Services.AddScoped<IOrderService, ProSpaceTest.Application.Services.OrderService>();

builder.Services.AddScoped<IUserRepository, ProSpaceTest.DataAccess.Repository.UserRepository>();
builder.Services.AddScoped<IUserService, ProSpaceTest.Application.Services.UserService>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(builder => builder
    .WithOrigins("http://localhost:57998") // Разрешить конкретный домен
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
