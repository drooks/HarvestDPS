namespace HarvestDPS.Extensions.Cryptography
{
    public interface ICrypto
    {
        string Encrypt<T>(T content, string passPhrase);
        T Decrypt<T>(string content, string passPhrase);
    }
}