// using Frontend.Model.MovieDetailModel;
// using Microsoft.AspNetCore.Components;
// using Microsoft.AspNetCore.Components.Web;
// using Frontend.Model.MovieSearchModel;
// using Frontend.Service;
//
// using Blazorise.Bootstrap;
// using Blazorise.Icons.FontAwesome;
// using Blazorise;
//
// var builder = WebApplication.CreateBuilder(args);
//
// // Add services to the container.
// builder.Services.AddRazorPages();
// builder.Services.AddServerSideBlazor();
// builder.Services.AddScoped<IMovieSearchModel, MovieSearchModel>();
// builder.Services.AddScoped<IMovieDetailModel, MovieDetailModel>();
// builder.Services.AddHttpClient();
//
// builder.Services
//     .AddBlazorise( options =>
//     {
//         options.Immediate = true;
//     } )
//     .AddBootstrapProviders()
//     .AddFontAwesomeIcons();
//
// var app = builder.Build();
// // Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Error");
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
// }
//
// app.UseHttpsRedirection();
//
// app.UseStaticFiles();
//
// app.UseRouting();
//
// app.MapBlazorHub();
// app.MapFallbackToPage("/_Host");
//
// app.Run();



using Frontend.Model.MovieDetailModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Frontend.Model.MovieSearchModel;
using Frontend.Service;

using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Blazorise;

var builder = WebApplication.CreateBuilder(args);

// Specify URLs your application will listen on
builder.WebHost.UseUrls("http://localhost:5233");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<IMovieSearchModel, MovieSearchModel>();
builder.Services.AddScoped<IMovieDetailModel, MovieDetailModel>();
builder.Services.AddHttpClient();

builder.Services
    .AddBlazorise( options =>
    {
        options.Immediate = true;
    } )
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();

builder.Logging.AddConsole();
builder.Logging.AddDebug();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
