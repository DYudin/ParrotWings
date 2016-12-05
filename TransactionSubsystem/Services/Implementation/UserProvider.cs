using System;
using Interfaces;
using TransactionSubsystem.Entities;
using TransactionSubsystem.Repositories.Abstract;

namespace TransactionSubsystem.Services.Implementation
{
    public class UserProvider : IUserProvider
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecurityService _securityService;

        public UserProvider(IUserRepository userRepository, ISecurityService securityService)
        {
            _userRepository = userRepository;
            _securityService = securityService;
        }

        public User CreateUser(string userName, string email, string password, int[] roles)
        {
            //var existingUser = _userRepository.GetSingle(x=>x.Name == userName);
            var existingUser = _userRepository.GetSingle(x => x.Email == email);

            if (existingUser != null)
            {
                throw new Exception($"User with {email} email is already registered");
            }

            var passwordSalt = _securityService.CreateSalt();

            var user = new User()
            {
                Name = userName,
                Salt = passwordSalt,
                Email = email,
                HashedPassword = _securityService.EncryptPassword(password, passwordSalt)
            };

            _userRepository.Add(user);
            _userRepository.Commit();

            return user;
        }

        public User GetUser(int userId)
        {
            //TODO:
            return null; //_userRepository.GetSingle(userId);
        }
    }
}
