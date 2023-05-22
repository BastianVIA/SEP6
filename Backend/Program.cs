using Backend.Database;
using Backend.Database.TransactionManager;
using Backend.Middleware;
using Backend.Movie.Infrastructure;
using Backend.People.Infrastructure;
using Backend.Service;
using Backend.SocialFeed.Infrastructure;
using Backend.User.Infrastructure;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using FirebaseAdmin.Auth;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using NLog;
using LogLevel = NLog.LogLevel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddScoped<GlobalExceptionFilter>();

builder.Services.AddScoped<IImageService, TMDBService>();
builder.Services.AddScoped<IResumeService, TMDBService>();
builder.Services.AddScoped<IUserImageService, UserImageService>();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
});

var firebaseApp = FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile("fireBaseOptions.json")
});

builder.Services.AddSingleton<FirebaseAuth>(FirebaseAuth.DefaultInstance);


builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://securetoken.google.com/sep6-a072b";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://securetoken.google.com/sep6-a072b",
            ValidateAudience = true,
            ValidAudience = "sep6-a072b",
            ValidateLifetime = true
        };
    });

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("WebApiDatabase")));
var transactionSemaphore = new SemaphoreSlim(1, 1);

builder.Services.AddScoped<IDatabaseTransactionFactory>(sp =>
    new DatabaseTransactionFactory(sp.GetRequiredService<DataContext>(), transactionSemaphore,
        sp.GetRequiredService<IMediator>()));


builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPeopleRepository, PeopleRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ISocialUserRepository, SocialUserRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.UseInlineDefinitionsForEnums();
    options.SupportNonNullableReferenceTypes();
    options.UseAllOfToExtendReferenceSchemas();
    options.IncludeXmlComments("bin/Debug/Backend.xml");

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
});
builder.Services.AddSwaggerGenNewtonsoftSupport();

var app = builder.Build();
app.UseMiddleware<FirebaseTokenMiddleware>();

LogManager.Setup().LoadConfiguration(builder =>
{
    builder.ForLogger().FilterMinLevel(LogLevel.Info).WriteToConsole();
    builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToFile(fileName: "log.txt");
});

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        var exceptionHandler = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandler?.Error;
        var exceptionFilter = context.RequestServices.GetRequiredService<GlobalExceptionFilter>();

        var actionContext = new ActionContext(context, context.GetRouteData(), new ControllerActionDescriptor());
        var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata> { exceptionFilter })
        {
            Exception = exception
        };

        await exceptionFilter.OnExceptionAsync(exceptionContext);
    });
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();