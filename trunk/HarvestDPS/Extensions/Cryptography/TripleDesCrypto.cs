using System;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace HarvestDPS.Extensions.Cryptography
{
    public class TripleDesCrypto : ICrypto
    {
        public string Encrypt<T>(T content, string passPhrase)
        {
            var json= JsonConvert.SerializeObject(content);

            var jsonArray = Encoding.UTF8.GetBytes(json);

            var hashprovider = new MD5CryptoServiceProvider();

            var keyArray = hashprovider.ComputeHash(Encoding.UTF8.GetBytes(passPhrase));

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray

            byte[] resultArray = cTransform.TransformFinalBlock(jsonArray, 0, jsonArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public T Decrypt<T>(string cipherString, string passPhrase)
        {
            var cipherArray = Convert.FromBase64String(cipherString);

            var hashprovider = new MD5CryptoServiceProvider();

            var keyArray = hashprovider.ComputeHash(Encoding.UTF8.GetBytes(passPhrase));

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(cipherArray, 0, cipherArray.Length);
            //Release resources held by TripleDes Encryptor                

            tdes.Clear();
            //return the Clear decrypted TEXT
            //return UTF8Encoding.UTF8.GetString(resultArray);

            var jsonArray = Encoding.UTF8.GetString(resultArray);

            return JsonConvert.DeserializeObject<T>(jsonArray);
        }
    }
}