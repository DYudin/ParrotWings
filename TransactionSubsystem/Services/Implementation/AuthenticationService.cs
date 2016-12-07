
using System.Threading.Tasks;
using Interfaces;
using TransactionSubsystem.Entities;
using TransactionSubsystem.Repositories.Abstract;

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
            get { return _currentUser; }

        }

        public Task<bool> Login(string email, string password)
        {
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
