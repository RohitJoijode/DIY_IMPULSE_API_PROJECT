namespace DIY_IMPULSE_API_PROJECT.MODEL
{
    public class LoginResponse
    {
        public bool IsLogedIn { get; set; } = false;
        public string? JwtToken { get; set; }
        public string? RefreshToken { get; internal set; }
    }
}
