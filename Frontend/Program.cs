using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Frontend.Service;

using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Blazorise;
using Frontend.Authentication;
using Frontend.Events;
using Frontend.Model.FavoriteMovies;
using Frontend.Model.Firebase;
using Frontend.Model.GetAllUsers;
using Frontend.Model.MovieDetail;
using Frontend.Model.MovieSearch;
using Frontend.Model.PersonSearch;
using Frontend.Model.Person;
using Frontend.Model.Recommendations;
using Frontend.Model.SocialFeed;
using Frontend.Model.Top100;
using Frontend.Model.User;
using Frontend.Model.UserProfilePicture;
using Frontend.Model.UserProfiles;
using Frontend.Model.UserSearch;
using Frontend.Network.FavoriteMovies;
using Frontend.Network.Firebase;
using Frontend.Network.GetAllUsers;
using Frontend.Network.MovieDetail;
using Frontend.Network.MovieSearch;
using Frontend.Network.PersonDetail;
using Frontend.Network.PersonSearch;
using Frontend.Network.Recommendations;
using Frontend.Network.SocialFeed;
using Frontend.Network.Top100;
using Frontend.Network.User;
using Frontend.Network.UserProfilePicture;
using Frontend.Network.UserProfiles;
using Frontend.Network.UserSearch;
using Frontend.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

// Specify URLs your application will listen on
builder.WebHost.UseUrls("http://localhost:5233");

builder.Services.AddScoped<IMovieSearchClient, MovieSearchClient>();
builder.Services.AddScoped<IMovieDetailClient, MovieDetailClient>();
builder.Services.AddScoped<IFirebaseClient, FirebaseClient>();
builder.Services.AddScoped<IFavoriteMoviesClient, FavoriteMoviesClient>();
builder.Services.AddScoped<IUserClient, UserClient>();
builder.Services.AddScoped<IPersonSearchClient, PersonSearchClient>();
builder.Services.AddScoped<IUserProfileClient, UserProfileClient>();
builder.Services.AddScoped<IRecommendationsClient, RecommendationsClient>();
builder.Services.AddScoped<IPersonDetailClient, PersonDetailClient>();
builder.Services.AddScoped<ISocialFeedClient, SocialFeedClient>();
builder.Services.AddScoped<ITop100Client, Top100Client>();
builder.Services.AddScoped<IUserProfilePictureClient, UserProfilePictureClient>();
builder.Services.AddScoped<IUserSearchClient, UserSearchClient>();
builder.Services.AddScoped<IGetAllUsersClient, GetAllUsersClient>();



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
builder.Services.AddScoped<IGetAllUsersModel,GetAllUsersModel>();

builder.Services.AddScoped<ProtectedLocalStorage>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddAuthorizationCore();

var configuration = builder.Configuration;
var backendApiUrl = configuration.GetValue<string>("BackendApiUrl");

builder.Services.AddHttpClient("BackendApi", client => 
{
    client.BaseAddress = new Uri(backendApiUrl);
});

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