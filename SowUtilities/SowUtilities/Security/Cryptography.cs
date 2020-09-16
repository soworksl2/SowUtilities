using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SowUtilities.Security
{
    public static class Cryptography
    {
        public const int MAX_SIZE_SUPPORT = 20000000;

        private const string SIZE_ERROR_MESSAGE = "El archivo es demasiado grande para realizar la operacion";

        private static byte[] BuilderKey(string key)
        {
            byte[] output = new byte[32];
            byte[] paddingKey = new byte[32]; new Random(key.Length).NextBytes(paddingKey);
            byte[] workKey = Encoding.UTF8.GetBytes(key);

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = i < workKey.Length ? workKey[i] : paddingKey[i];
            }

            return output;
        }

        private static byte[] BuilderIV(string key)
        {
            byte[] output = new byte[16];
            int seed = 0;

            for (int i = 0; i < key.Length; i++)
            {
                seed += key[i];
            }

            new Random(seed).NextBytes(output);

            return output;
        }

        public static string EncryptText(string clearText, string key = "[default]")
        {
            if (string.IsNullOrEmpty(clearText) || string.IsNullOrEmpty(key))
            {
                throw new Exception("Texto o llave nulos o vacios");
            }

            byte[] byteClearText = Encoding.UTF8.GetBytes(clearText);
            byte[] encrypted;

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.KeySize = 256;
                aes.Key = BuilderKey(key);
                aes.IV = BuilderIV(key);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform cryptoTransform = aes.CreateEncryptor())
                {
                    encrypted = cryptoTransform.TransformFinalBlock(byteClearText, 0, byteClearText.Length);
                }

            }

            return Convert.ToBase64String(encrypted);
        }

        public static string DecryptText(string cipherText, string key = "[default]")
        {
            if (string.IsNullOrWhiteSpace(cipherText) || string.IsNullOrWhiteSpace(key))
            {
                throw new Exception("Texto o llave nulos o vacios");
            }

            byte[] byteCipherText = Convert.FromBase64String(cipherText);
            byte[] clearText;

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.KeySize = 256;
                aes.Key = BuilderKey(key);
                aes.IV = BuilderIV(key);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform crypto = aes.CreateDecryptor())
                {
                    clearText = crypto.TransformFinalBlock(byteCipherText, 0, byteCipherText.Length);
                }
            }

            return Encoding.UTF8.GetString(clearText);
        }

        public static void EncryptFile(string path, string key = "[default]", string outPath = "")
        {
            if (new FileInfo(path).Length > MAX_SIZE_SUPPORT)
            {
                throw new Exception(SIZE_ERROR_MESSAGE);
            }

            byte[] clearFile = File.ReadAllBytes(path);

            if (string.IsNullOrEmpty(outPath))
            {
                outPath = path;
            }

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.KeySize = 256;
                aes.Key = BuilderKey(key);
                aes.IV = BuilderIV(key);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform cryptoTransform = aes.CreateEncryptor())
                {
                    File.WriteAllBytes(outPath, cryptoTransform.TransformFinalBlock(clearFile, 0, clearFile.Length));
                }

            }
        }

        public static void DecryptFile(string path, string key = "[default]", string outPath = "")
        {
            if (new FileInfo(path).Length > MAX_SIZE_SUPPORT)
            {
                throw new Exception(SIZE_ERROR_MESSAGE);
            }

            byte[] cipherFile = File.ReadAllBytes(path);

            if (string.IsNullOrEmpty(outPath))
            {
                outPath = path;
            }

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.KeySize = 256;
                aes.Key = BuilderKey(key);
                aes.IV = BuilderIV(key);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform cryptoTransform = aes.CreateDecryptor())
                {
                    File.WriteAllBytes(outPath, cryptoTransform.TransformFinalBlock(cipherFile, 0, cipherFile.Length));
                }

            }

        }

    }
}
