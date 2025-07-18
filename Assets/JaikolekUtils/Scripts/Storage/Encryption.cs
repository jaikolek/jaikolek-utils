using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace JaikolekUtils.Storage
{
    public static class Encryption
    {
        public static string Encrypt(string input, string encryptionKey)
        {
            byte[] iv = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.Key = System.Text.Encoding.UTF8.GetBytes(encryptionKey);
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(input);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }
            return System.Convert.ToBase64String(array);
        }

        public static string Decrypt(string input, string encryptionKey)
        {
            byte[] iv = new byte[16];
            try
            {
                byte[] buffer = System.Convert.FromBase64String(input);
                using (Aes aes = Aes.Create())
                {
                    aes.Key = System.Text.Encoding.UTF8.GetBytes(encryptionKey);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                return null;
            }
        }
    }
}
