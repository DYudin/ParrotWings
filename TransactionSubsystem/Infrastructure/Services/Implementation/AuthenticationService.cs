using System.Threading.Tasks;
using TransactionSubsystem.Entities;
using System;
using System.Text;
using TransactionSubsystem.Infrastructure.Exceptions;
using TransactionSubsystem.Infrastructure.Repositories.Abstract;
using TransactionSubsystem.Infrastructure.Services.Abstract;

namespace TransactionSubsystem.Infrastructure.Services.Implementation
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

        //public User CurrentUser
        //{
        //    get
        //    {
        //        return _currentUser;
        //    }
        //    set
        //    {
        //        _currentUser = value;
        //    }

        //}

        public string Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException(email);
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException(password);

            string authIdentifier;

            _currentUser = _userRepository.GetSingle(x => x.Email == email);
            if (_currentUser != null && isPasswordValid(_currentUser, password))
            {
                authIdentifier = Convert.ToBase64String(Encoding.UTF8.GetBytes(_currentUser.Name));
            }
            else
            {
                _currentUser = null;
                throw new TransactionSubsystemException("Invalid credentials");
            }

            return authIdentifier;
        }

        public void LogOut()
        {
            //_currentUser = null;
        }

        private bool isPasswordValid(User user, string password)
        {
            return string.Equals(_securityService.EncryptPassword(password, user.Salt), user.HashedPassword);
        }
    }
}
