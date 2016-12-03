using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using TransactionSubsystem.Entities;
using TransactionSubsystem.Services.Implementation;

namespace TransactionSubsystem
{
    public class ApiMock
    {
        private readonly IAuthenticationService _authMock;
        private readonly UserVerificationService _userVerificationService;
        private readonly IAmountVerificationService _amountVerificationService;
        private readonly ITransactionCommitService _transactionCommitService;

        public ApiMock(
            IAuthenticationService authMock,
            UserVerificationService userVerificationService,
            IAmountVerificationService amountVerificationService,
            ITransactionCommitService transactionCommitService)
        {
            if (authMock == null) throw new ArgumentNullException(nameof(authMock));
            if (userVerificationService == null) throw new ArgumentNullException(nameof(userVerificationService));
            if (amountVerificationService == null) throw new ArgumentNullException(nameof(amountVerificationService));
            if (transactionCommitService == null) throw new ArgumentNullException(nameof(transactionCommitService));

            _authMock = authMock;
            _userVerificationService = userVerificationService;
            _amountVerificationService = amountVerificationService;
            _transactionCommitService = transactionCommitService;
        }

        public void PrepareNewTransaction()
        {
            _authMock.CurrentUser.PrepareNewTransaction();
        }

        // Post request by editing user textBox
        public bool VerifyRecepientUser(string userName)
        {
            bool result = false;

            var targetUser = _userVerificationService.GetUserByName(userName);
           
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

        // Post request by editing amount textBox
        public bool VerifyDonorBalance(decimal transactionAmount)
        {
            bool result = false;

            var isAmountValid = _amountVerificationService.VerifyAmount(_authMock.CurrentUser, transactionAmount);
            _authMock.CurrentUser.PreparingTransaction.CommitAvailableState = isAmountValid;

            if (isAmountValid)
            {
                _authMock.CurrentUser.PreparingTransaction.Amount = transactionAmount;
                result = true;
            }

            return result;
        }

        // Post request by clicking SEND button
        public void CommitTransaction()
        {
            if (_authMock.CurrentUser.PreparingTransaction.CommitAvailableState)
            {
                _transactionCommitService.CommitTransaction(_authMock.CurrentUser.PreparingTransaction);
            }
            else
            {
                // ERROR!!!
            }
            
        }
    }
}
