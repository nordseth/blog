using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Blog.Core;

namespace Test
{
    [TestClass]
    public class EncryptionTest
    {
        string message = @"The trick is to use MemoryStream.ToArray(). I also changed your code so that it uses the CryptoStream to Write, in both encrypting and decrypting. And you don't need to call CryptoStream.FlushFinalBlock() explicitly, because you have it in a using() statement, and that flush will happen on Dispose(). The following works for me.";

        string encKey = "8D4179A939E19030E7366475B8926D0EC25D2B53F06A7A162A98BA4E6C6E97C2";
        string hmacKey = "7BE6F5297D51ACE1BB3F46D783C7669039B54BF37272654E46BF9240326BA4035C093A6A059FC10F7DB0857C1DBE53E63D2410302F8979F5E490D44B39DA522B";

        [TestMethod]
        public void GenerateKey()
        {
            var enc = new Encryption();
            byte[] key = enc.GenerateEncryptionKey();
            byte[] key2 = enc.GenerateEncryptionKey();

            Assert.IsTrue(key.Length == key2.Length);
            Assert.IsFalse(Enumerable.SequenceEqual(key, key2));
            Console.WriteLine(key.ToHex());
        }

        [TestMethod]
        public void Hash()
        {
            var enc = new Encryption();

            byte[] hash = enc.Hash(message.ToBytes());
            string hexHash = hash.ToHex();
            string base64Hash = hash.ToBase64();

            byte[] hash2 = enc.Hash(message.ToBytes());
            string hexHash2 = hash2.ToHex();
            string base64Hash2 = hash.ToBase64();

            Assert.IsTrue(Enumerable.SequenceEqual(hash, hash2));
            Assert.IsTrue(Enumerable.SequenceEqual(hexHash, hexHash2));
            Assert.IsTrue(Enumerable.SequenceEqual(base64Hash, base64Hash2));
        }

        [TestMethod]
        public void Random()
        {
            var enc = new Encryption();
            var rng = enc.Random(64);
            var rng2 = enc.Random(64);

            Assert.IsTrue(rng.Length == rng2.Length);
            Assert.IsFalse(Enumerable.SequenceEqual(rng, rng2));
            Console.WriteLine(rng.ToHex());
        }

        [TestMethod]
        public void Hmac()
        {
            var enc = new Encryption();

            var hmac = enc.Hmac(message.ToBytes(), hmacKey.HexToBytes());
            string hexHmac = hmac.ToHex();
            string base64Hmac = hmac.ToBase64();

            var hmac2 = enc.Hmac(message.ToBytes(), hmacKey.HexToBytes());
            string hexHmac2 = hmac2.ToHex();
            string base64Hmac2 = hmac2.ToBase64();

            Assert.IsTrue(Enumerable.SequenceEqual(hmac, hmac2));
            Assert.IsTrue(Enumerable.SequenceEqual(hexHmac, hexHmac2));
            Assert.IsTrue(Enumerable.SequenceEqual(base64Hmac, base64Hmac2));
        }

        [TestMethod]
        public void EncryptDecrypt()
        {
            var enc = new Encryption();

            var encrypted = enc.Encrypt(message.ToBytes(), encKey.ToBytes());
            var encrypted2 = enc.Encrypt(message.ToBytes(), encKey.ToBytes());

            Assert.IsTrue(encrypted.Length == encrypted2.Length);
            Assert.IsFalse(Enumerable.SequenceEqual(encrypted, encrypted2));

            var decrypted = enc.Decrypt(encrypted, encKey.ToBytes());
            var decryptedString = decrypted.ToUtf8();
            var decrypted2 = enc.Decrypt(encrypted2, encKey.ToBytes());
            var decryptedString2 = decrypted2.ToUtf8();

            Assert.AreEqual(message, decryptedString);
            Assert.AreEqual(message, decryptedString2);
        }
    }
}
