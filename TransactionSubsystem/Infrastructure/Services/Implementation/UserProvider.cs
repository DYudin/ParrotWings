using System;
using System.Threading.Tasks;
using TransactionSubsystem.Entities;
using System.Collections.Generic;
using TransactionSubsystem.Infrastructure.Exceptions;
using TransactionSubsystem.Infrastructure.Repositories.Abstract;
using TransactionSubsystem.Infrastructure.Services.Abstract;

namespace TransactionSubsystem.Infrastructure.Services.Implementation
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
                throw new TransactionSubsystemException($"User with {email} email is already registered");
            }

            var passwordSalt = _securityService.CreateSalt();

            var user = new User(userName, email);
            user.FillSecurityProperties(passwordSalt, _securityService.EncryptPassword(password, passwordSalt));

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
