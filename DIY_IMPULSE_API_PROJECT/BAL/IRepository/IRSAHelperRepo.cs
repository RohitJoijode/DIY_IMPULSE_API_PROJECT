namespace DIY_IMPULSE_API_PROJECT.BAL.IRepository
{
    public interface IRSAHelperRepo
    {
        string Encrypt(string text);
        string Decrypt(string encrypted);
    }
}
