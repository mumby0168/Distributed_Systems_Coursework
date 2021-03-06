using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CoreExtensions;

namespace DistSysACW.Services
{
    public class RsaProvider : IRsaProvider
    {
        private readonly RSACryptoServiceProvider _provider;
        private readonly string _publicKey;
        
        public RsaProvider()
        {
            _provider = new RSACryptoServiceProvider();
            _publicKey = _provider.ToXmlStringCore22();
        }


        public string PublicKey => _publicKey;
        
        public string Sha1Sign(string message)
        {
            var signed = _provider.SignData(Encoding.ASCII.GetBytes(message), new SHA1CryptoServiceProvider());
            return BitConverter.ToString(signed);
        }

        public byte[] Decrypt(string hexValue)
        {
            byte[] data = hexValue.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();
            return _provider.Decrypt(data, true);
        }
    }
}