using Serilog;
using ShortRouteOptimizerAPI.DataAccess;
using ShortRouteOptimizerAPI.Middlewares;
using ShortRouteOptimizerAPI.Services;

var builder = WebApplication.CreateBuilder(args);

//Adding Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:8080")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
});

builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

// Add services to the container.
builder.Services.AddScoped<IShortestPathService, ShortestPathService>();
builder.Services.AddScoped<IShortestPathRepository, ShortestPathRepository>();
builder.Services.AddScoped<INodesService, NodesService>();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Logging Configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureCustomExceptionHandler();

app.UseHttpsRedirection();

app.UseCors("AllowVueApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
