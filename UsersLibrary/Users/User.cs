using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using UsersLibrary.Settings;
using UsersLibrary.Services;
using System.IO;

namespace UsersLibrary
{
    public class User
    {
        readonly Random _random = new Random();

        private string _email;
        public string Email
        {
            get => _email;
            internal set
            {
                _email = value;
                HashEmail = GetSHA256Hash(value);
            }
        }
        public string HashEmail { get; private set; }


        public List<Service> Services { get; private set; }


        private string _hashPassword;
        public string AuthPassword { get; private set; }
        public string HashAuthPassword
        {
            get => _hashPassword;
            private set
            {
                value += Salt;
                _hashPassword = GetSHA256Hash(value);
            }
        }

        public string Salt { get; set; }
        private void CreateSalt()
        {
            if (string.IsNullOrEmpty(Salt))
                Salt = string.Join("", Enumerable.Range(0, 20).Select(_ => (char)_random.Next(65, 90)));
        }


        public User() { }
        public User(string email, string password)
        {
            CreateSalt();

            Email = email;
            AuthPassword = password;
            HashAuthPassword = password;

            Services = new List<Service>();
        }


        public CryptedService EncryptService(Service service)
        {
            string encryptLogin, encryptPasswrod;
            using (Aes myAes = Aes.Create())
            {
                myAes.Key = Encoding.ASCII.GetBytes(AuthPassword);

                byte[] encrypted = EncryptStringToBytes_Aes(service.Login, myAes.Key, myAes.IV);
                encryptLogin = Convert.ToBase64String(encrypted);

                encrypted = EncryptStringToBytes_Aes(service.Password, myAes.Key, myAes.IV);
                encryptPasswrod = Convert.ToBase64String(encrypted);
            }

            return new CryptedService(service.Name, encryptLogin, encryptPasswrod) { Id = service.Id};
        }
        
        public Service DecryptService(CryptedService service)
        {
            string decryptLogin, decryptPasswrod;
            using (Aes myAes = Aes.Create())
            {
                myAes.Key = Encoding.ASCII.GetBytes(AuthPassword);

                decryptLogin = DecryptStringFromBytes_Aes(Convert.FromBase64String(service.Login), myAes.Key, myAes.IV);
                decryptPasswrod = DecryptStringFromBytes_Aes(Convert.FromBase64String(service.Password), myAes.Key, myAes.IV);
            }

            return new Service(service.Name, decryptLogin, decryptPasswrod) { Id = service.Id };
        }

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        public static User CreateAlreadyExistUser(string email, string password)
        {
            return new User()
            {
                Email = email,
                AuthPassword = password,
            };
        }
        public void SetHashAuthPassword(string salt)
        {
            Salt = salt;
            HashAuthPassword = AuthPassword;
        }

        public static string GetSHA256Hash(string inputString)
        {
            string outString;
            using (SHA256 sHA256 = SHA256.Create())
            {
                byte[] hash = sHA256.ComputeHash(Encoding.UTF8.GetBytes(inputString));
                outString = string.Join("", hash.Select(c => c.ToString("x2")));
            }

            return outString;
        }

        public static implicit operator User(SignUpSettings settings)
        {
            return new User()
            {
                Email = settings.Email,
                AuthPassword = settings.AuthPassword,
            };

        }
    }
}