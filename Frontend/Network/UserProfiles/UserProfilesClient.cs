﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Frontend.Entities;
using Frontend.Pages;
using Frontend.Service;

namespace Frontend.Network.UserProfiles
{
    public class UserProfileClient : NSwagBaseClient, IUserProfileClient
    {
        public async Task<Entities.User> GetUserProfile(string userId)
        {
            UserProfileResponse? response;
            try
            {
                response = await _api.UserGETAsync(userId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

            if (response == null || response.UserProfile == null)
            {
                return null;
            }

            var userRatings = new List<UserRating>();

            var user = new Entities.User()
            {
                UserRatings = userRatings,
                Username = response.UserProfile.DisplayName,
                Email = response.UserProfile.Email,
                Bio = response.UserProfile.Bio,
                AverageOfUserRatings = (decimal)response.UserProfile.AverageOfUserRatings
            };

            return user;
        }
    }
}