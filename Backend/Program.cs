// using Backend.Database;
// using Backend.Enum;
// using Backend.Movie.Infrastructure;
// using Backend.Service;
// using Microsoft.OpenApi.Any;
// using Microsoft.OpenApi.Models;
// using Newtonsoft.Json.Converters;
//
// var builder = WebApplication.CreateBuilder(args);
//
// // Add services to the container.
//
// builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
//
// builder.Services.AddScoped<IImageService, TMDBService>();
// builder.Services.AddScoped<IResumeService, TMDBService>();
// builder.Services.AddControllers().AddNewtonsoftJson(options =>
// {
//     options.SerializerSettings.Converters.Add(new StringEnumConverter());
// });
//
// builder.Services.AddScoped<DataContext>();
// builder.Services.AddScoped<IMovieRepository, MovieRepository>();
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
//
// builder.Services.AddSwaggerGen(options =>
// {
//     options.UseInlineDefinitionsForEnums();
//     options.SupportNonNullableReferenceTypes();
//     options.UseAllOfToExtendReferenceSchemas();
//     
// });
// builder.Services.AddSwaggerGenNewtonsoftSupport();
//
// var app = builder.Build();
//
//
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
// else
// {
//     app.UseHttpsRedirection();
// }
//
// app.UseAuthorization();
//
// app.MapControllers();
//
// app.Run();


using Backend.Database;
using Backend.Enum;
using Backend.Movie.Infrastructure;
using Backend.Service;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

// Specify URLs your application will listen on
builder.WebHost.UseUrls("http://localhost:5276");

// Add services to the container.
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddScoped<IImageService, TMDBService>();
builder.Services.AddScoped<IResumeService, TMDBService>();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
});

builder.Services.AddScoped<DataContext>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.UseInlineDefinitionsForEnums();
    options.SupportNonNullableReferenceTypes();
    options.UseAllOfToExtendReferenceSchemas();
    
});
builder.Services.AddSwaggerGenNewtonsoftSupport();

builder.Logging.AddConsole();
builder.Logging.AddDebug();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

