﻿namespace Library.Core.DataServices.UserGroup.Models;

public class UserLoginResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}