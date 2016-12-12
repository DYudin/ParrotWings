using System;
using Interfaces;

namespace TransactionSubsystem
{
    public class ApiMock
    {
        private readonly IAuthenticationService _authMock;
        private readonly IUserProvider _userProvider;    
        private readonly ITransactionService _transactionService;

        public ApiMock(
            IAuthenticationService authMock,
            IUserProvider userProvider,          
            ITransactionService transactionService)
        {
            if (authMock == null) throw new ArgumentNullException(("authMock"));
            if (userProvider == null) throw new ArgumentNullException(("userProvider"));          
            if (transactionService == null) throw new ArgumentNullException(("transactionService"));

            _authMock = authMock;
            _userProvider = userProvider;            
            _transactionService = transactionService;
        }

        public void PrepareNewTransaction()
        {
            _authMock.CurrentUser.PrepareNewTransaction();
        }

        // Post request by editing user textBox
        public bool VerifyRecepientUser(string userName)
        {
            bool result = false;

            var targetUser = _userProvider.GetUserByName(userName);
           
            if (targetUser != null)
            {
                _authMock.CurrentUser.PreparingTransaction.CommitAvailableState = true;
                _authMock.CurrentUser.PreparingTransaction.Recepient = targetUser;
                result = true;
            }
            else
            {
                _authMock.CurrentUser.PreparingTransaction.CommitAvailableState = false;
            }

            return result;
        }       

        // Post request by clicking SEND button
        public void CommitTransaction()
        {
            if (_authMock.CurrentUser.PreparingTransaction.CommitAvailableState)
            {
                _transactionService.CommitTransaction(_authMock.CurrentUser.PreparingTransaction);
            }
            else
            {
                // ERROR!!!
            }
            
        }
    }
}
