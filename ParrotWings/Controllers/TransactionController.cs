using System;
using System.Collections.Generic;
using System.Web.Http;
using Interfaces;
using ParrotWings.ViewModel;
using TransactionSubsystem.Entities;
using System.Threading.Tasks;

namespace ParrotWings.Controllers
{
    [Route("api/[controller]")]
    public class TransactionController : ApiController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserProvider _userProvider;     
        private readonly ITransactionService _transactionService;

        public TransactionController(
            IAuthenticationService authenticationService,
            IUserProvider userProvider,            
            ITransactionService transactionService)
        {
            if (authenticationService == null) throw new ArgumentNullException(("authenticationService"));
            if (userProvider == null) throw new ArgumentNullException(("userProvider"));           
            if (transactionService == null) throw new ArgumentNullException(("transactionService"));

            _authenticationService = authenticationService;
            _userProvider = userProvider;          
            _transactionService = transactionService;
        }

        [Route("api/transaction/currentuserinfo")]
        [HttpGet]
        public IHttpActionResult GetCurrentUserInfo()
        {            
            UserViewModel userVM = new UserViewModel() {
                UserName = _authenticationService.CurrentUser.Name,
                CurrentBalance = _authenticationService.CurrentUser.CurrentBalance };
                   
            return Ok(userVM);
        }

        [Route("api/transaction/alltransactions")]
        [HttpGet]
        public async Task<IHttpActionResult> GetTransactions()
        {
            var transactionsVM = new List<TransactionViewModel>();
            Result getTransactionsResult = null;

            try
            {
                IEnumerable<Transaction> transactions = await _transactionService.GetTransactionsByUserName(_authenticationService.CurrentUser.Name);

                foreach (var tr in transactions)
                {
                    //TODO: CLear
                    if (tr.Recepient == null)
                    {
                        tr.Recepient = new User() { Name = "UnknownRecepient" };
                    }

                    if (tr.TransactionOwner == null)
                    {
                        tr.TransactionOwner = new User() { Name = "UnknownOwner" };
                    }

                    transactionsVM.Add(new TransactionViewModel()
                    {
                        Amount = tr.Amount,
                        Date = tr.Date,
                        CorrespondedUser = tr.Recepient.Name == _authenticationService.CurrentUser.Name
                        ? tr.TransactionOwner.Name
                        : tr.Recepient.Name,
                        ResultingBalance = tr.Recepient.Name == _authenticationService.CurrentUser.Name
                        ? tr.RecepientResultingBalance
                        : tr.OwnerResultingBalance,
                        Outgoing = tr.Recepient.Name != _authenticationService.CurrentUser.Name

                    });
                }

                getTransactionsResult = new Result()
                {
                    Succeeded = true,
                    Message = "Transaction succeeded"
                };

            }
            catch (Exception)
            {
                getTransactionsResult = new Result()
                {
                    Succeeded = false,
                    Message = "Error getting user's transactions"
                };
            }            

            return Ok(transactionsVM);
        }

        [Route("api/transaction/allusers")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUsers()
        {
            var usersVM = new List<UserViewModel>();
            Result getUsersResult = null;

            try
            {
                IEnumerable<User> users = await _userProvider.GetUsers();

                foreach (var user in users)
                {
                    usersVM.Add(new UserViewModel() { UserName = user.Name, Id = user.Id });
                }
            }
            catch (Exception)
            {
                getUsersResult = new Result()
                {
                    Succeeded = false,
                    Message = "Error getting users"
                };
            }

            return Ok(usersVM);
        } 

        [Route("api/transaction/sendmoney")]
        [HttpPost]
        public async Task<IHttpActionResult> CommitTransaction(TransactionViewModel transactionVM)
        {
            Result commitTransactionResult = null;
            
            try
            {
                var transactionOwner = _userProvider.GetUserByName(_authenticationService.CurrentUser.Name);
                var recepient = _userProvider.GetUserByName(transactionVM.CorrespondedUser);

                if (recepient == null)
                {
                    commitTransactionResult = new Result()
                    {
                        Succeeded = false,
                        Message = $"User with name: '{transactionVM.CorrespondedUser}' not found"
                    };
                }
                else
                {
                    var transactionToSend = new Transaction()
                    {
                        Recepient = recepient,
                        TransactionOwner = transactionOwner,
                        Amount = transactionVM.Amount,
                        Date = transactionVM.Date
                    };

                    await _transactionService.CommitTransaction(transactionToSend);

                    commitTransactionResult = new Result()
                    {
                        Succeeded = true,
                        Message = "Transaction succeeded"
                    };
                }  
            }
            catch (Exception ex)
            {
                commitTransactionResult = new Result()
                {
                    Succeeded = false,
                    Message = ex.Message
                };
            }

            return Ok(commitTransactionResult);
        }
    }
}
