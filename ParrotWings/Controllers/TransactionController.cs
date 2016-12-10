using System;
using System.Collections.Generic;
using System.Web.Http;
using Interfaces;
using ParrotWings.ViewModel;
using TransactionSubsystem.Entities;

namespace ParrotWings.Controllers
{
    [Route("api/[controller]")]
    public class TransactionController : ApiController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserProvider _userProvider;
        private readonly IAmountVerificationService _amountVerificationService;
        private readonly ITransactionService _transactionService;

        public TransactionController(
            IAuthenticationService authenticationService,
            IUserProvider userProvider,
            IAmountVerificationService amountVerificationService,
            ITransactionService transactionService)
        {
            if (authenticationService == null) throw new ArgumentNullException(("authenticationService"));
            if (userProvider == null) throw new ArgumentNullException(("userProvider"));
            if (amountVerificationService == null) throw new ArgumentNullException(("amountVerificationService"));
            if (transactionService == null) throw new ArgumentNullException(("transactionService"));

            _authenticationService = authenticationService;
            _userProvider = userProvider;
            _amountVerificationService = amountVerificationService;
            _transactionService = transactionService;
        }
        
        [Route("api/transaction/alltransactions")]
        [HttpGet]
        public IHttpActionResult GetTransactions()
        {
            var transactionsVM = new List<TransactionViewModel>();
            IEnumerable<Transaction> transactions = _transactionService.GetTransactionsByUserName(_authenticationService.CurrentUser.Name);

            foreach (var tr in transactions)
            {
                transactionsVM.Add(new TransactionViewModel()
                {
                    Amount = tr.Amount,
                    DateCommited = tr.Date,
                    RecepientName = tr.Recepient.Name,
                    ResultingBalance = tr.ResultingBalance
                });
            }

            return Ok(transactionsVM);
        }
        
        [Route("api/transaction/verifyuser")]
        [HttpPost]
        public bool VerifyRecepientUser(string userName)
        {
            bool result = false;

            var targetUser = _userProvider.GetUserByName(userName);

            if (targetUser != null)
            {
                _authenticationService.CurrentUser.PreparingTransaction.CommitAvailableState = true;
                _authenticationService.CurrentUser.PreparingTransaction.Recepient = targetUser;
                result = true;
            }
            else
            {
                _authenticationService.CurrentUser.PreparingTransaction.CommitAvailableState = false;
            }

            return result;
        }
        
        [Route("api/transaction/verifybalance")]
        [HttpPost]
        public bool VerifyDonorBalance(decimal transactionAmount)
        {
            bool result = false;

            var isAmountValid = _amountVerificationService.VerifyAmount(_authenticationService.CurrentUser, transactionAmount);
            _authenticationService.CurrentUser.PreparingTransaction.CommitAvailableState = isAmountValid;

            if (isAmountValid)
            {
                _authenticationService.CurrentUser.PreparingTransaction.Amount = transactionAmount;
                result = true;
            }

            return result;
        }

        [Route("api/transaction/send")]
        [HttpPost]
        public void CommitTransaction()
        {
            if (_authenticationService.CurrentUser.PreparingTransaction.CommitAvailableState)
            {
                _transactionService.CommitTransaction(_authenticationService.CurrentUser.PreparingTransaction);
            }
            else
            {
                // ERROR!!!
            }

            _authenticationService.CurrentUser.PrepareNewTransaction();
        }
    }
}
