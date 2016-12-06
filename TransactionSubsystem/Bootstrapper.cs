
using System;
using Interfaces;
using TransactionSubsystem.Repositories;
using TransactionSubsystem.Repositories.Abstract;
using TransactionSubsystem.Repositories.Implementation;
using TransactionSubsystem.Services.Implementation;
using TransactionSubsystem.Services.Mock;

namespace TransactionSubsystem
{
    public class Bootstrapper : IDisposable
    {
        private ApiMock _apiMock;
        private bool _isInitialized;
        private TransactionSubsystemContext _context;

        public ApiMock ApiInstance
        {
            get
            {
                if (!_isInitialized)
                {
                    Initialize();
                }

                return _apiMock;
            }
        }

        public void Initialize()
        {
            // low services
            _context = new TransactionSubsystemContext();
            
            ITransactionRepository transactionRepository = new TransactionRepository(_context);
            IUserRepository userRepository = new UserRepository(_context);

            // middle services
            IAmountVerificationService amountVerificationService = new AmountVerificationService(userRepository);
            ISecurityService securityService = new SecurityService();
            IUserProvider userProvider = new UserProvider(userRepository, securityService);
            IAuthenticationService authMock = new AuthMock(userRepository);
            ITransactionService transactionService = new TransactionService(userRepository,
                transactionRepository);

            // high services
            _apiMock = new ApiMock(authMock, userProvider, amountVerificationService,
                transactionService);

            _isInitialized = true;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
