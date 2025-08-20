namespace DIY_IMPULSE_API_PROJECT.BAL.IRepository
{
    public interface IBaseUrlServiceRepo
    {
        string GetBaseUrl();
        string GetAbsoluteUrl(string relativePath);
    }
}
