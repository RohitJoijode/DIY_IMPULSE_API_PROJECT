using DIY_IMPULSE_API_PROJECT.BAL.IRepository;

namespace DIY_IMPULSE_API_PROJECT.BAL.Repository
{
    public class IpServiceRepo : IIpServiceRepo
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IpServiceRepo(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetClientIpAddress()
        {
            return GetClientIpAddress(_httpContextAccessor.HttpContext);
        }

        public string GetClientIpAddress(HttpContext context)
        {
            if (context == null) return "No HTTP Context";

            // Check for forwarded headers (behind proxy/load balancer)
            var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                // The X-Forwarded-For header can contain a comma-separated list of IPs.
                // The client IP is typically the first one.
                return forwardedFor.Split(',')[0].Trim();
            }

            // Check for other common proxy headers
            var remoteIp = context.Connection.RemoteIpAddress;

            // Handle IPv6 loopback for localhost scenarios
            if (remoteIp != null && remoteIp.IsIPv4MappedToIPv6)
            {
                remoteIp = remoteIp.MapToIPv4();
            }

            return remoteIp?.ToString() ?? "Unknown";
        }
    }
}
