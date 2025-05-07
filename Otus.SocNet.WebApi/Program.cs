using Otus.SocNet.DAL;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
#if DEBUG
builder.Configuration.AddJsonFile("appsettings.Debug.json", optional: true, reloadOnChange: true);
#else
    builder.Configuration.AddJsonFile("appsettings.Release.json", optional: true, reloadOnChange: true);
#endif



builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
var cs =
#if !DEBUG
    builder.Configuration.GetConnectionString("DBConnection");
#else
    builder.Configuration.GetConnectionString("DBConnectionDebug");
#endif
builder.Services.AddDal(cs);

var app = builder.Build();
// Configure the HTTP request pipeline.
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbInit = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await dbInit.InitializeAsync(cs);
}

app.Run();
