//***************************************************************************************
// Writer: Stylish Esper
//***************************************************************************************

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Esper.ESave.Encryption
{
    /// <summary>
    /// Encrypt/decrypts data.
    /// </summary>
    public static class ESaveEncryption
    {
        /// <summary>
        /// Converts a byte array to a string using ASCII encoding.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>A string.</returns>
        public static string ToASCIIString(this byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }

        /// <summary>
        /// Converts a string to bytes using ASCII encoding.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>Byte array.</returns>
        public static byte[] ToASCIIBytes(this string s)
        {
            return Encoding.ASCII.GetBytes(s);
        }

        /// <summary>
        /// Converts to a base 64 string.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        /// <returns>The byte array as a base 64 string.</returns>
        public static string ToBase64String(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Converts to a byte array.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>The string as a byte array.</returns>
        public static byte[] ToBase64Bytes(this string s)
        {
            return Convert.FromBase64String(s);
        }

        /// <summary>
        /// Converts a byte array to an AES string if possible.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>The bytes as an AES string.</returns>
        public static string ToAesString(this byte[] bytes)
        {
            try
            {
                return ToBase64String(bytes);
            }
            catch
            {
                return ToASCIIString(bytes);
            }
        }

        /// <summary>
        /// Converts a string to an AES byte array if possible.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>The string as an AES byte array.</returns>
        public static byte[] ToAesBytes(this string s)
        {
            byte[] bytes = null;
            bool useFallback = false;

            try
            {
                bytes = ToBase64Bytes(s);

                if (bytes.Length != 16 && bytes.Length != 24 && bytes.Length != 32)
                {
                    useFallback = true;
                }
            }
            catch
            {
                useFallback = true;
            }

            if (useFallback)
            {
                bytes = ToASCIIBytes(s);
            }

            return bytes;
        }

        /// <summary>
        /// Generates an AES key.
        /// </summary>
        /// <returns>A randomly generated AES key.</returns>
        public static byte[] GenerateKey()
        {
            var aes = Aes.Create();
            aes.GenerateKey();
            return aes.Key;
        }

        /// <summary>
        /// Generates an AES IV.
        /// </summary>
        /// <returns>A randomly generated AES IV.</returns>
        public static byte[] GenerateIV()
        {
            var aes = Aes.Create();
            aes.GenerateIV();
            return aes.IV;
        }

        /// <summary>
        /// Encrypts this string.
        /// </summary>
        /// <param name="plainText">Text to encrypt.</param>
        /// <param name="key">The key for the AES algorithm.</param>
        /// <param name="iv">The IV for the AES algorithm.</param>
        /// <returns>Encrypted byte array from a string.</returns>
        public static byte[] AESEncrypt(this string plainText, byte[] key, byte[] iv)
        {
            byte[] encrypted;

            // Create an AES object with key and IV
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using MemoryStream msEncrypt = new();
                using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (StreamWriter swEncrypt = new(csEncrypt))
                {
                    // Write all data to the stream.
                    swEncrypt.Write(plainText);
                }
                encrypted = msEncrypt.ToArray();
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        /// <summary>
        /// Decrypts this byte array.
        /// </summary>
        /// <param name="cipher">Bytes to decrypt.</param>
        /// <param name="key">The key for the AES algorithm.</param>
        /// <param name="iv">The IV for the AES algorithm.</param>
        /// <returns>String decrypted from a byte array.</returns>
        public static string AESDecrypt(this byte[] cipher, byte[] key, byte[] iv)
        {
            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                MemoryStream msDecrypt = new(cipher);
                CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
                StreamReader srDecrypt = new(csDecrypt);

                // Read the decrypted bytes from the decrypting stream
                // and place them in a string.
                plaintext = srDecrypt.ReadToEnd();
            }

            return plaintext;
        }
    }
}

