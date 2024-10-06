using Models;
using Persistence.Interfaces;
using Persistence.Repositories.Cassandra;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IAsyncAccess<User>, UserRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseWebSockets();

app.MapControllers();

app.Run();
