namespace Library.Core.UserGroup.Models;

public class UserRefreshTokenResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}