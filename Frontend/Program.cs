using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Blazorise;
using Frontend.Model.FavoriteMovies;
using Frontend.Model.Firebase;
using Frontend.Model.MovieDetail;
using Frontend.Model.MovieSearch;
using Frontend.Model.PersonSearch;
using Frontend.Model.Recommendations;
using Frontend.Model.User;
using Frontend.Model.UserProfiles;
using Frontend.Model.UserSearch;
using Frontend.Network.FavoriteMovies;
using Frontend.Network.Firebase;
using Frontend.Network.MovieDetail;
using Frontend.Network.MovieSearch;
using Frontend.Network.PersonSearch;
using Frontend.Network.Recommendations;
using Frontend.Network.User;
using Frontend.Network.UserProfiles;
using Frontend.Network.UserSearch;
using Frontend.Service;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<IFavoriteMoviesClient, FavoriteMoviesClient>();
builder.Services.AddScoped<IFirebaseClient, FirebaseClient>();
builder.Services.AddScoped<IMovieDetailClient, MovieDetailClient>();
builder.Services.AddScoped<IMovieSearchClient, MovieSearchClient>();
builder.Services.AddScoped<IUserClient, UserClient>();
builder.Services.AddScoped<IRecommendationsClient, RecommendationsClient>();
builder.Services.AddScoped<IPersonSearchClient, PersonSearchClient>();
builder.Services.AddScoped<IMovieSearchModel, MovieSearchModel>();
builder.Services.AddScoped<IMovieDetailModel, MovieDetailModel>();
builder.Services.AddScoped<IFirebaseModel, FirebaseModel>();
builder.Services.AddScoped<IFavoriteMoviesModel, FavoriteMoviesModel>();
builder.Services.AddScoped<IUserModel, UserModel>();
builder.Services.AddScoped<IPersonSearchModel, PersonSearchModel>();
builder.Services.AddScoped<IUserProfileClient, UserProfileClient>();
builder.Services.AddScoped<IUserProfilesModel, UserProfilesModel>();
builder.Services.AddScoped<IRecommendationsModel, RecommendationsModel>();

builder.Services.AddScoped<IUserSearchClient, UserSearchClient>();
builder.Services.AddScoped<IUserSearchModel, UserSearchModel>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddAuthorizationCore();

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

