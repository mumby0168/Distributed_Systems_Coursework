using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using CoreExtensions;

namespace DistSysACWClient.Services
{
    public class RsaEncryptionService
    {
        private readonly RSACryptoServiceProvider _cryptoService;
        public RsaEncryptionService()
        {
            _cryptoService = new RSACryptoServiceProvider();
        }

        public string EncryptAsHex(byte[] data)
        {
            _cryptoService.FromXmlStringCore22(User.PublicKey);
            var res = _cryptoService.Encrypt(data, true);
            return BitConverter.ToString(res);
        }
    }
}