using System.Security.Cryptography;

namespace UserApi.RSAModel
{
    public static class RSAService
    {
        public static RSA GetPublicKey()
        {
            var key = File.ReadAllText(@"..\UserApi\RSAModel\public_key.pem");
            var rsa = RSA.Create();
            rsa.ImportFromPem(key);
            return rsa;
        }

        public static RSA GetPrivateKey()
        {
            var key = File.ReadAllText(@"..\UserApi\RSAModel\private_key.pem");
            var rsa = RSA.Create();
            rsa.ImportFromPem(key);
            return rsa;
        }
    }
}
