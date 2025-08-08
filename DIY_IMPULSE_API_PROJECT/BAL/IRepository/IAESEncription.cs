namespace DIY_IMPULSE_API_PROJECT.BAL.IRepository
{
    public interface IAESEncription
    {
        string AESEncrypt(string plainText);
        string AESDecrypt(string cipherText);
    }
}
