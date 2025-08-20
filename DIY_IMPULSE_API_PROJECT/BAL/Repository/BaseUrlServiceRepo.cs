using DIY_IMPULSE_API_PROJECT.BAL.IRepository;

namespace DIY_IMPULSE_API_PROJECT.BAL.Repository
{
    public class BaseUrlServiceRepo : IBaseUrlServiceRepo
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseUrlServiceRepo(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null) return string.Empty;

            return $"{request.Scheme}://{request.Host}{request.PathBase}";
        }

        public string GetAbsoluteUrl(string relativePath)
        {
            var baseUrl = GetBaseUrl();
            relativePath = relativePath.TrimStart('/');
            return $"{baseUrl}/{relativePath}";
        }
    }
}
