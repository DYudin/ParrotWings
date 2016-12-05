using System;
using Interfaces;
using TransactionSubsystem.Entities;
using TransactionSubsystem.Repositories.Abstract;

namespace TransactionSubsystem.Services.Implementation
{
    public class AmountVerificationService : IAmountVerificationService
    {
        private readonly IUserRepository _userRepository;
        public AmountVerificationService(IUserRepository userRepository)
        {
            if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));

            _userRepository = userRepository;
        }

        public bool VerifyAmount(User user, decimal Amount)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            bool result = false;

            //todo change to email. or ID
            var currentUser = _userRepository.GetSingle(x => x.Name == user.Name);

            if (currentUser != null)
            {
                if (currentUser.CurrentBalance >= Amount)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
