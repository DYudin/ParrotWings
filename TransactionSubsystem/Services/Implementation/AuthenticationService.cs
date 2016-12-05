
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
            _userRepository = userRepository;
            _securityService = securityService;
        }

        public User CurrentUser
        {
            get { return _currentUser; }

        }

        public bool Login(string userName, string password)
        {
            bool loginResult = false;

            var user = _userRepository.GetSingle(x => x.Name == userName);
            if (user != null && isPasswordValid(user, password))
            {
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
