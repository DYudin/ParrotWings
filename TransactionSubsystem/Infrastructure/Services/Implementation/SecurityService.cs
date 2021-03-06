﻿using System;
using System.Security.Cryptography;
using System.Text;
using TransactionSubsystem.Infrastructure.Services.Abstract;

namespace TransactionSubsystem.Infrastructure.Services.Implementation
{
    public class SecurityService : ISecurityService
    {
        public string CreateSalt()
        {
            var data = new byte[0x10];

            var cryptoServiceProvider = RandomNumberGenerator.Create();
            cryptoServiceProvider.GetBytes(data);
            return Convert.ToBase64String(data);
        }

        public string EncryptPassword(string password, string salt)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException(password);
            if (string.IsNullOrWhiteSpace(salt)) throw new ArgumentException(salt);

            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = string.Format("{0}{1}", salt, password);
                byte[] saltedPasswordAsBytes = Encoding.UTF8.GetBytes(saltedPassword);
                return Convert.ToBase64String(sha256.ComputeHash(saltedPasswordAsBytes));
            }
        }
    }
}
