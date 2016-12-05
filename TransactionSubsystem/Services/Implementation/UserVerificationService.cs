using System;
using TransactionSubsystem.Entities;
using TransactionSubsystem.Repositories.Abstract;

namespace TransactionSubsystem.Services.Implementation
{
    public class UserVerificationService
    {
        private readonly IUserRepository _userRepository;
        public UserVerificationService(IUserRepository userRepository)
        {
            if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));

            _userRepository = userRepository;
        }

        public User GetUserByName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentException(nameof(userName));

            return  _userRepository.GetSingle(x => x.Name == userName);
        }

        public bool VerifyUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            bool result = false;

            //todo change to email. or ID
            var currentUser = _userRepository.GetSingle(x => x.Name == user.Name);

            if (currentUser != null)
            {
                result = true;
            }

            return result;
        }
    }
}
