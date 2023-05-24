using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Blazorise;
using Frontend.Authentication;
using Frontend.Events;
using Frontend.Model.FavoriteMovies;
using Frontend.Model.Firebase;
using Frontend.Model.MovieDetail;
using Frontend.Model.MovieSearch;
using Frontend.Model.PersonSearch;
using Frontend.Model.Person;
using Frontend.Model.Recommendations;
using Frontend.Model.SearchFilter;
using Frontend.Model.SocialFeed;
using Frontend.Model.Top100;
using Frontend.Model.User;
using Frontend.Model.UserProfilePicture;
using Frontend.Model.UserProfiles;
using Frontend.Model.UserSearch;
using Frontend.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<IAlertAggregator, AlertAggregator>();
builder.Services.AddScoped<IMovieSearchModel, MovieSearchModel>();
builder.Services.AddScoped<IMovieDetailModel, MovieDetailModel>();
builder.Services.AddScoped<IFirebaseModel, FirebaseModel>();
builder.Services.AddScoped<IFavoriteMoviesModel, FavoriteMoviesModel>();
builder.Services.AddScoped<IUserModel, UserModel>();
builder.Services.AddScoped<IPersonSearchModel, PersonSearchModel>();
builder.Services.AddScoped<IUserProfilesModel, UserProfilesModel>();
builder.Services.AddScoped<IRecommendationsModel, RecommendationsModel>();
builder.Services.AddScoped<IPersonModel, PersonModel>();
builder.Services.AddScoped<ISocialFeedModel, SocialFeedModel>();
builder.Services.AddScoped<ITop100Model, Top100Model>();
builder.Services.AddScoped<IUserProfilePictureModel, UserProfilePictureModel>();
builder.Services.AddScoped<IUserSearchModel, UserSearchModel>();
builder.Services.AddScoped<ISearchFilterModel, SearchFilterModel>();
builder.Services.AddScoped<ProtectedSessionStorage>();

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

