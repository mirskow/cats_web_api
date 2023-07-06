var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

var app = builder.Build();

app.Map("/", (context) => context.Response.SendFileAsync("html/index.html"));

app.MapControllers();

app.Run();
