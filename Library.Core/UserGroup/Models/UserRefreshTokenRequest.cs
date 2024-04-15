namespace Library.Core.UserGroup.Models;

public class UserRefreshTokenRequest
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}