namespace DIY_IMPULSE_API_PROJECT.MODEL
{
    public class ResponseList<T>
    {
        public List<T>? Data { get; set; }
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }
        public string? JWTToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
