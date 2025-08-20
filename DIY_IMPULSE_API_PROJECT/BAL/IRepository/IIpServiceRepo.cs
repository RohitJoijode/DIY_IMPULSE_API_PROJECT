namespace DIY_IMPULSE_API_PROJECT.BAL.IRepository
{
    public interface IIpServiceRepo
    {
        public string GetClientIpAddress();
        public string GetClientIpAddress(HttpContext context);

    }
}
