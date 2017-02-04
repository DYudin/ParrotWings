using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Interfaces;
using ParrotWings.ViewModel;
using TransactionSubsystem.Entities;
using System.Threading.Tasks;
using AutoMapper;

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
            var userVM = new UserViewModel() {
                UserName = _authenticationService.CurrentUser.Name,
                CurrentBalance = _authenticationService.CurrentUser.CurrentBalance };
                   
            return Ok(userVM);
        }

        [Route("api/transaction/alltransactions")]
        [HttpGet]
        public async Task<IHttpActionResult> GetTransactions()
        {
            List<TransactionViewModel> transactionsVM;            

            try
            {
                var transactions = await _transactionService.GetTransactionsByUserName(_authenticationService.CurrentUser.Name); //todo //unitOfWork.Drinks.GetAll()
                var transactionsList = transactions.ToList();

                Mapper.Initialize(cfg => cfg.CreateMap<Transaction, TransactionViewModel>()
                .ForMember(
                    dest => dest.CorrespondedUser, 
                    opt => opt.MapFrom(
                        src => src.Recepient.Name == _authenticationService.CurrentUser.Name ?
                        src.TransactionOwner.Name :
                        src.Recepient.Name))
                .ForMember(
                    dest => dest.ResultingBalance, 
                    opt => opt.MapFrom(
                        src => src.Recepient.Name == _authenticationService.CurrentUser.Name
                        ? src.RecepientResultingBalance
                        : src.OwnerResultingBalance))
                .ForMember(
                    dest => dest.Outgoing, 
                    opt => opt.MapFrom(
                        src => src.Recepient.Name != _authenticationService.CurrentUser.Name)));
                transactionsVM = Mapper.Map<IEnumerable<Transaction>, List<TransactionViewModel>>(transactionsList); 
             
            }
            catch (Exception)
            {
                //TODO: logging
                return BadRequest("Error getting transactions");
            }

            return Ok(transactionsVM);
        }

        [Route("api/transaction/allusers")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUsers()
        {
            List<UserViewModel> usersVM;
           
            try
            {
                var users = await _userProvider.GetUsers();
                Mapper.Initialize(cfg => cfg.CreateMap<User, UserViewModel>());
                usersVM = Mapper.Map<IEnumerable<User>, List<UserViewModel>>(users); //todo unitOfWork.Drinks.GetAll()
            }
            catch (Exception)
            {
                //TODO: logging
                return BadRequest("Error getting users");
            }

            return Ok(usersVM);
        } 

        [Route("api/transaction/sendmoney")]
        [HttpPost]
        public async Task<IHttpActionResult> CommitTransaction(TransactionViewModel transactionVM)
        {
            Result commitTransactionResult = null;

            if (ModelState.IsValid)
            {
                try
                {
                    var transactionOwner = _userProvider.GetUserByName(_authenticationService.CurrentUser.Name); //todo unitOfWork.Drinks.GetAll() // todo await
                    var recepient = _userProvider.GetUserByName(transactionVM.CorrespondedUser); //todo unitOfWork.Drinks.GetAll()// todo await

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
                        Mapper.Initialize(cfg => cfg.CreateMap<TransactionViewModel, Transaction>()
                        .ForMember(
                            dest => dest.Recepient,
                            opt => opt.MapFrom(
                                src => recepient))
                        .ForMember(
                            dest => dest.TransactionOwner,
                            opt => opt.MapFrom(
                                src => transactionOwner)));
                        var transaction = Mapper.Map<TransactionViewModel, Transaction>(transactionVM);

                        await _transactionService.CommitTransaction(transaction);

                        commitTransactionResult = new Result()
                        {
                            Succeeded = true,
                            Message = "Transaction succeeded"
                        };
                    }
                }
                catch (Exception)
                {
                    return BadRequest("Transaction failed");
                }
            }

            return Ok(commitTransactionResult);
        }
    }
}
