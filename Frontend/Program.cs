using Frontend.Model.MovieDetailModel;
using Frontend.Model.MovieSearchModel;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Blazorise;
using Frontend.Model.FirebaseModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<IMovieSearchModel, MovieSearchModel>();
builder.Services.AddScoped<IMovieDetailModel, MovieDetailModel>();
builder.Services.AddScoped<IFirebaseModel, FirebaseModel>();
builder.Services.AddHttpClient();

builder.Services
    .AddBlazorise( options =>
    {
        options.Immediate = true;
    } )
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();

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

