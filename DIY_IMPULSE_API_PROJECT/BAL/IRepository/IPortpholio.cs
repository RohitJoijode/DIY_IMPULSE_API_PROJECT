using DIY_IMPULSE_API_PROJECT.MODEL;

namespace DIY_IMPULSE_API_PROJECT.BAL.IRepository
{
    public interface IPortpholio
    {
        public Task<Responses> SaveDataFromPortfolio(PortpholioRequest portfolioRequestObj, string baseUrl, string Ip);
    }
}
