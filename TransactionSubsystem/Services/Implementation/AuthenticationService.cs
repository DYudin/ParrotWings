
using System.Threading.Tasks;
using Interfaces;
using TransactionSubsystem.Entities;
using TransactionSubsystem.Repositories.Abstract;
using System;

namespace TransactionSubsystem.Services.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        private User _currentUser;
        private readonly IUserRepository _userRepository;
        private readonly ISecurityService _securityService;

        public AuthenticationService(IUserRepository userRepository, ISecurityService securityService)
        {   
            if (userRepository == null) throw new ArgumentNullException("userRepository");
            if (securityService == null) throw new ArgumentNullException("securityService");
            _userRepository = userRepository;
            _securityService = securityService;
        }

        public User CurrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                _currentUser = value;
            }

        }

        public Task<bool> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException(email);
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException(password);

            return Task.Run(() => LoginInternal(email, password));
        }

        private bool LoginInternal(string email, string password)
        {
            bool loginResult = false;

            _currentUser = _userRepository.GetSingle(x => x.Email == email);
            if (_currentUser != null && isPasswordValid(_currentUser, password))
            {
                _currentUser.PrepareNewTransaction();
                loginResult = true;
            }

            return loginResult;
        }

        private bool isPasswordValid(User user, string password)
        {
            return string.Equals(_securityService.EncryptPassword(password, user.Salt), user.HashedPassword);
        }
    }
}
