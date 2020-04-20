using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace DistSysACWClient.Services
{
    public class AesEncryptionService
    {
        private AesCryptoServiceProvider _encryptionService;
        public AesEncryptionService()
        {
            _encryptionService = new AesCryptoServiceProvider();
            _encryptionService.GenerateKey();
            _encryptionService.GenerateIV();
            InitializationVector = BitConverter.ToString(_encryptionService.IV);
            Key = BitConverter.ToString(_encryptionService.Key);
        }
        
        public string InitializationVector { get; } 
        
        public string Key { get; }

        public byte[] KeyBytes => _encryptionService.Key;
        
        public byte[] InitializationVectorBytes => _encryptionService.IV;


        public string DecryptInteger(string hexBytes)
        {
            byte[] data = hexBytes.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();
            var decryptor = _encryptionService.CreateDecryptor();
            string final;
            using (MemoryStream ms = new MemoryStream(data))
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (StreamReader sr = new StreamReader(cs))
                        final = sr.ReadToEnd();

            return final;
        }
    }
}