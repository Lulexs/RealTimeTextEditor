using ApplicationLogic;
using Models;
using Persistence.Interfaces;
using Persistence.Repositories.Cassandra;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAsyncAccess<User>, UserRepository>();
builder.Services.AddScoped<UserLogic>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    // app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseWebSockets();

app.MapControllers();

app.Run();
