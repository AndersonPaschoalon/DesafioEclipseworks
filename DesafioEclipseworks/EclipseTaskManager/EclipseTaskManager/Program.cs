using EclipseTaskManager.Context;
using EclipseTaskManager.Extensions;
using EclipseTaskManager.Logging;
using EclipseTaskManager.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions
            .ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EclipseTaskManagerContext>(options =>
    options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

// repositories dependency injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectTaskRepository, ProjectTaskRepository>();
builder.Services.AddScoped<IProjectTaskCommentRepository, ProjectTaskCommentRepository>();
builder.Services.AddScoped<IProjectTaskUpdateRepository, ProjectTaskUpdateRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureDevelopmentExceptionHandler();
}
else
{
    app.ConfigureProductionExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
