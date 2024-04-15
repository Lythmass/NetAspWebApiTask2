using Microsoft.EntityFrameworkCore;
using Reddit;
using Reddit.Filters;
using Reddit.Mapper;
using Reddit.Middlewares;
using Reddit.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => options.Filters.Add<ModelValidationActionFilter>())
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteDb"));
    options.UseLazyLoadingProxies();
    options.LogTo(Console.WriteLine, LogLevel.Information);
    options.EnableSensitiveDataLogging();
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
               builder => builder.AllowAnyOrigin()
                                 .AllowAnyMethod()
                                 .AllowAnyHeader());
});
builder.Services.AddSingleton<IMapper, Mapper>();
builder.Services.AddScoped<ICommunitiesRepository, CommunitiesRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
