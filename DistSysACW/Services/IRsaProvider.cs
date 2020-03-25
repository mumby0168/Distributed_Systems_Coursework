namespace DistSysACW.Services
{
    public interface IRsaProvider
    {
        string PublicKey { get; }

        string Sha1Sign(string message);
    }
}