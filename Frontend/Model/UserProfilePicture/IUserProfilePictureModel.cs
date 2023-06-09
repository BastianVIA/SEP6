﻿namespace Frontend.Model.UserProfilePicture;

public interface IUserProfilePictureModel
{
    public Task UploadProfilePicture(string userToken, Byte[] profilePicture);
    public Task<byte[]> GetProfilePicture(string userId);
}