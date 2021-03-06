using DataAbstraction.Connections;
using DataAbstraction.Interfaces;
using DataAbstraction.Models;
using Matrix.DataBase;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//// add child Quik API connections adress
//builder.Services.Configure<HttpClientConfig>(
//    builder.Configuration.GetSection("HttpClientConfig"));

// Matrix DataBase configure
builder.Services.AddTransient<IDataBaseRepository, DataBaseRepository>();
builder.Services.Configure<DataBaseConnectionConfiguration>(
    builder.Configuration.GetSection("DataBaseConfig"));

// Set portfolios type list, enabled for new non EDP QUIK user
builder.Services.Configure<PortfoliosAllowedForNonEDP>(
    builder.Configuration.GetSection("PortfoliosForNonEDP"));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
