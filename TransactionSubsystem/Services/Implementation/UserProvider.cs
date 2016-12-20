using System;
using System.Threading.Tasks;
using Interfaces;
using TransactionSubsystem.Entities;
using TransactionSubsystem.Repositories.Abstract;
using System.Collections.Generic;

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

        public User GetUserByName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentException((userName));

            return _userRepository.GetSingle(x => x.Name == userName);
        }

        public Task<IEnumerable<User>> GetUsers()
        {
            return Task.Run(() => _userRepository.GetAll());
        }

        public Task<User> CreateUser(string userName, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentException(userName);
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException(email);
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException(password);

            return Task.Run(() => CreateUserInternal(userName, email, password));
        }

        private User CreateUserInternal(string userName, string email, string password)
        {
            var existingUser = _userRepository.GetSingle(x => x.Email == email);

            if (existingUser != null)
            {
                throw new Exception(string.Format("User with {0} email is already registered", email));
            }

            var passwordSalt = _securityService.CreateSalt();

            var user = new User()
            {
                Name = userName,
                CurrentBalance = 500,
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
