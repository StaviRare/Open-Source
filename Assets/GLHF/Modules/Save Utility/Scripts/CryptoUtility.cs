using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace GLHF.SaveUtility
{
    /// <summary>
    /// Encryption utility that uses RijndaelManaged algorithm 
    /// </summary>
    public static class CryptoUtility
    {
        /// Key lengths are 128, 192, or 256 bits; defaulting to 256 bits
        /// Block sizes are 128, 192, or 256 bits; defaulting to 128 bits (Aes-compatible)
        /// In .NET Core, it is the same as AES and supports only a 128-bit block size.
        private const int KeySize = 256;
        private const int BlockSize = 128;
        private const CipherMode Mode = CipherMode.CBC;
        private const PaddingMode Padding = PaddingMode.PKCS7;

        // Leaving the key and IV hard-coded is a bad idea
        // Custom key and IV should be stored elsewhere
        private static string StringIV = "c4zB+IW9wGRzt30VLA+BrA==";
        private static string StringKey = "0kVo47dBvaxgbwdlkhouxQu7cG4f2dYRdRKiFmPV6uE=";


        #region Usage

        public static string EncryptString(string textToEncrypt)
        {
            var textAsByteArray = ASCIIEncoding.UTF8.GetBytes(textToEncrypt);
            var encryptedByteArray = EncryptByteArray(textAsByteArray);
            var encryptedText = Convert.ToBase64String(encryptedByteArray);
            return encryptedText;
        }

        public static string DecryptString(string encryptedText)
        {
            var encryptedByteArray = Convert.FromBase64String(encryptedText);
            var textAsByteArray = DecryptByteArray(encryptedByteArray);
            var decryptedText = ASCIIEncoding.UTF8.GetString(textAsByteArray);
            return decryptedText;
        }

        public static byte[] EncryptByteArray(byte[] byteArray)
        {
            var rijndael = GetRijndael();
            var crypto = rijndael.CreateEncryptor();
            var cipherText = crypto.TransformFinalBlock(byteArray, 0, byteArray.Length);

            crypto.Dispose();
            return cipherText;
        }

        public static byte[] DecryptByteArray(byte[] byteArray)
        {
            var rijndael = GetRijndael();
            var decrypto = rijndael.CreateDecryptor();
            var enc = decrypto.TransformFinalBlock(byteArray, 0, byteArray.Length);

            decrypto.Dispose();
            return enc;
        }

        private static RijndaelManaged GetRijndael()
        {
            var IV = Convert.FromBase64String(StringIV);
            var Key = Convert.FromBase64String(StringKey);

            var rijndael = new RijndaelManaged
            {
                KeySize = KeySize,
                BlockSize = BlockSize,
                Mode = Mode,
                Padding = Padding,
                IV = IV,
                Key = Key
            };

            return rijndael;
        }

        #endregion


        #region Custom Options

        public static string GenerateCustomIV()
        {
            var rijndael = GetRijndael();
            rijndael.GenerateIV();

            var customIV = rijndael.IV;
            var ivString = Convert.ToBase64String(customIV);
            
            return ivString;
        }

        public static string GenerateCustomKey()
        {
            var rijndael = GetRijndael();
            rijndael.GenerateKey();

            var customKey = rijndael.Key;
            var keyString = Convert.ToBase64String(customKey);

            return keyString;
        }
        
        public static string GenerateCustomCode()
        {
            var customIV = GenerateCustomIV();
            var customKey = GenerateCustomKey();

            Debug.Log("Get IV: " + customIV);
            Debug.Log("Get Key: " + customIV);

            var combinedKeyAndIV = customIV + "," + customKey;
            var customCodeAsByteArray = ASCIIEncoding.UTF8.GetBytes(combinedKeyAndIV);
            var customCode = Convert.ToBase64String(customCodeAsByteArray);
            return customCode;
        }

        public static void SetCustomIV(string customIV)
        {
            StringIV = customIV;
        }

        public static void SetCustomKey(string customKey)
        {
            StringKey = customKey;
        }

        public static void SetCustomCode(string customCode)
        {
            var customCodeAsByteArray = Convert.FromBase64String(customCode);
            var combinedKeyAndIV = ASCIIEncoding.UTF8.GetString(customCodeAsByteArray);

            var customIV = combinedKeyAndIV.Split(',')[0];
            var customKey = combinedKeyAndIV.Split(',')[1];

            Debug.Log("Set IV: " + customIV);
            Debug.Log("Set Key: " + customKey);

            SetCustomIV(customIV);
            SetCustomKey(customKey);
        }

        #endregion
    }
}