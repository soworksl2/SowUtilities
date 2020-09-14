using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using System.IO;
using System.Runtime.CompilerServices;

namespace SowUtilities.Security
{
    public static class Cryptography
    {

        private static byte[] GetKey(string key)
        {
            if(key.Length > 16)
            {
                throw new Exception("La llave no debe superar los 16 caracteres");
            }

            byte[] output = new byte[32];
            byte[] tempKey = Encoding.UTF8.GetBytes(key);
            byte[] paddingKey = new byte[32]; new Random(key.Length).NextBytes(paddingKey);

            for(int i = 0; i < output.Length; i++)
            {
                output[i] = i < tempKey.Length ? tempKey[i] : paddingKey[i];
            }

            return output;

        }

        private static byte[] GetIV(string key)
        {
            byte[] output = new byte[16];
            int seed = 0;
            for(int i = 0; i < key.Length; i++)
            {
                seed += key[i];
            }
            new Random(seed).NextBytes(output);

            return output;

        }

        public static string Encript(string Text, string key = "[default]")
        {
            byte[] PlainText = Encoding.Default.GetBytes(Text);
            byte[] encryptText;

            using(AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.KeySize = 256;
                aes.Key = GetKey(key.Equals("[default]") ? Environment.MachineName : key);
                aes.IV = GetIV(key);
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform crypto = aes.CreateEncryptor())
                {
                    encryptText = crypto.TransformFinalBlock(PlainText, 0, PlainText.Length);
                }

            }

            return Encoding.Default.GetString(encryptText);

        }

        public static void Encript(string path, bool Override, string key = "[default]")
        {
            byte[] plainfile = File.ReadAllBytes(path);
            string writePath = Override ? path : path.Replace(Path.GetExtension(path), ".saes");

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.KeySize = 256;
                aes.Key = GetKey(key.Equals("[default]") ? Environment.MachineName : key);
                aes.IV = GetIV(key);
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform crypto = aes.CreateEncryptor())
                {
                    File.WriteAllBytes(writePath, crypto.TransformFinalBlock(plainfile, 0, plainfile.Length));
                }
            }

        }

        public static string Decrypt(string crypText, string key = "[default]")
        {
            byte[] cryptText = Encoding.Default.GetBytes(crypText);
            string output;

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.KeySize = 256;
                aes.Key = GetKey(key.Equals("[default]") ? Environment.MachineName : key);
                aes.IV = GetIV(key);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform crypto = aes.CreateDecryptor())
                {
                    output = Encoding.Default.GetString(crypto.TransformFinalBlock(cryptText, 0, cryptText.Length));
                }
            }

            return output;
        }

    }
}
