using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Blog.Core
{
    public class Encryption
    {
        /// <summary>
        /// Uses sha1,
        /// </summary>
        /// <param name="key">recommended size of the secret key is 64 bytes</param>
        public byte[] Hmac(byte[] message, byte[] key)
        {
            HMAC hmac = GetHmac(key);
            return hmac.ComputeHash(message);
        }

        /// <summary>
        /// Uses sha1,
        /// </summary>
        /// <param name="key">recommended size of the secret key is 64 bytes</param>
        public byte[] Hmac(Stream message, byte[] key)
        {
            HMAC hmac = GetHmac(key);
            return hmac.ComputeHash(message);
        }

        private HMAC GetHmac(byte[] key)
        {
            return new HMACSHA256(key);
        }

        public byte[] GenerateEncryptionKey()
        {
            var enc = GetEncryptor();
            enc.GenerateKey();
            return enc.Key;
        }

        public byte[] Encrypt(byte[] input, byte[] key)
        {
            using (var stream = new MemoryStream(input))
            {
                return Encrypt(stream, key);
            }
        }
        public byte[] Decrypt(byte[] input, byte[] key)
        {
            using (var stream = new MemoryStream(input))
            {
                return Decrypt(stream, key);
            }
        }

        public byte[] Encrypt(Stream input, byte[] key)
        {
            byte[] buffer = GetBuffer();
            using (var encryptor = GetEncryptor())
            using (var output = new MemoryStream())
            {
                encryptor.Key = GetKey(key, encryptor);
                encryptor.GenerateIV();
                output.Write(encryptor.IV, 0, encryptor.IV.Length);
                using (var cryptoStream = new CryptoStream(output, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    int bytesRead;
                    do
                    {
                        bytesRead = input.Read(buffer, 0, buffer.Length);
                        cryptoStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead > 0);
                }
                return output.ToArray();
            }
        }
        public byte[] Decrypt(Stream input, byte[] key)
        {
            byte[] buffer = GetBuffer();
            using (var encryptor = GetEncryptor())
            {
                encryptor.Key = GetKey(key, encryptor);
                byte[] iv = new byte[encryptor.IV.Length];
                input.Read(iv, 0, iv.Length);
                encryptor.IV = iv;
                using (var output = new MemoryStream())
                using (var cryptoStream = new CryptoStream(input, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    int bytesRead;
                    do
                    {

                        bytesRead = cryptoStream.Read(buffer, 0, buffer.Length);
                        output.Write(buffer, 0, bytesRead);
                    } while (bytesRead > 0);

                    return output.ToArray();
                }
            }
        }

        private byte[] GetBuffer()
        {
            return new byte[256];
        }

        private byte[] GetIV(byte[] key, SymmetricAlgorithm encryptor)
        {
            int length = encryptor.IV.Length;

            if (key.Length > length)
            {
                return key.Take(length).ToArray();
            }
            else if (key.Length < length)
            {
                byte[] newKey = new byte[length];
                Array.Copy(key, newKey, key.Length);
                return newKey;
            }
            else
            {
                return key;
            }
        }

        private byte[] GetKey(byte[] key, SymmetricAlgorithm encryptor)
        {
            if (encryptor.LegalKeySizes == null || encryptor.LegalKeySizes.Length == 0)
                return key;

            int minSize = encryptor.LegalKeySizes[0].MinSize / 8;
            int maxSize = encryptor.LegalKeySizes[0].MaxSize / 8;

            if (key.Length > maxSize)
            {
                return key.Take(maxSize).ToArray();
            }
            else if (key.Length < minSize)
            {
                byte[] newKey = new byte[minSize];
                Array.Copy(key, newKey, key.Length);
                return newKey;
            }
            else
            {
                return key;
            }
        }

        private SymmetricAlgorithm GetEncryptor()
        {
            var encryptor = Aes.Create();
            encryptor.Padding = PaddingMode.PKCS7;
            return encryptor;
        }

        public byte[] Hash(byte[] message)
        {
            HashAlgorithm hasher = GetHasher();
            return hasher.ComputeHash(message);
        }

        public byte[] Hash(Stream message)
        {
            HashAlgorithm hasher = GetHasher();
            return hasher.ComputeHash(message);
        }

        private HashAlgorithm GetHasher()
        {
            return SHA256.Create();
        }

        private static RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();

        public byte[] Random(int length)
        {
            var data = new byte[length];
            _rng.GetBytes(data);
            return data;
        }
    }
}
