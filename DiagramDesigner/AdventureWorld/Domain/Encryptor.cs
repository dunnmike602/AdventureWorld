using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DiagramDesigner.AdventureWorld.Domain
{
    public static class Encryptor
    {
        private static readonly byte[] Salt = new byte[] { 0x12, 0xff, 0xff, 0x012, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x07, 0xaf, 0x5d, 0x08, 0x22, 0xec };
        private const string Password = "783&*£$&%*)(*&$7";

        public static byte[] EncryptString(string sourceString)
        {
            sourceString = sourceString.Trim();

            if(sourceString.Length == 0)
            {
                return new byte[0];
            }

            var clearData = Encoding.UTF8.GetBytes(sourceString);

            using (var ms = new MemoryStream())
            {
                var alg = Rijndael.Create();
                var pdb = new Rfc2898DeriveBytes(Password, Salt);

                alg.Key = pdb.GetBytes(32);
                alg.IV = pdb.GetBytes(16); 

                using (var cs = new CryptoStream(ms,
                    alg.CreateEncryptor(), CryptoStreamMode.Write))
                {

                    cs.Write(clearData, 0, clearData.Length);
                    cs.Close();
                    var encryptedData = ms.ToArray();

                    return encryptedData;
                }
            }
        }

        public static string DecryptString(byte[] crypt)
        {
            if(crypt == null || crypt.Length == 0)
            {
                return string.Empty;
            }

            var rijndael = Rijndael.Create(); 
            var pdb = new Rfc2898DeriveBytes(Password, Salt); 
            rijndael.Key = pdb.GetBytes(32); 
            rijndael.IV = pdb.GetBytes(16); 
            using(var memoryStream = new MemoryStream())
            {
                using(var cryptoStream = new CryptoStream(memoryStream, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(crypt, 0, crypt.Length);
                    cryptoStream.FlushFinalBlock();

                    return Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
        }
    }
}