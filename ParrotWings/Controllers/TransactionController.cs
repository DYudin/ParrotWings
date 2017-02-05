using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Interfaces;
using ParrotWings.ViewModel;
using TransactionSubsystem.Entities;
using System.Threading.Tasks;
using AutoMapper;
using TransactionSubsystem.Infrastructure.UnitOfWork.Abstract;

namespace ParrotWings.Controllers
{
    [Route("api/[controller]")]
    public class TransactionController : ApiController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserProvider _userProvider;     
        private readonly ITransactionService _transactionService;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public TransactionController( 
            IAuthenticationService authenticationService,
            IUserProvider userProvider,            
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            if (authenticationService == null) throw new ArgumentNullException(("authenticationService"));
            if (userProvider == null) throw new ArgumentNullException(("userProvider"));           
            if (unitOfWorkFactory == null) throw new ArgumentNullException(("unitOfWorkFactory"));

            _authenticationService = authenticationService;
            _userProvider = userProvider;          
            _unitOfWorkFactory = unitOfWorkFactory;
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
                List<Transaction> transactionsList = null;
                await Task.Run(() =>
                {
                    transactionsList = _authenticationService.CurrentUser.Transactions.ToList();
                });

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
            catch (Exception ex)
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
            if (ModelState.IsValid)
            {
                try
                {
                    using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
                    {
                        User recepient = null;
                        await Task.Run(() =>
                            {
                                recepient = _userProvider.GetUserByName(transactionVM.CorrespondedUser);
                            });

                        if (recepient == null)
                        {
                            return BadRequest($"User with name: '{transactionVM.CorrespondedUser}' not found");
                        }

                        Mapper.Initialize(cfg => cfg.CreateMap<TransactionViewModel, Transaction>()
                            .ForMember(
                                dest => dest.Recepient,
                                opt => opt.MapFrom(
                                    src => recepient)));
                        var transaction = Mapper.Map<TransactionViewModel, Transaction>(transactionVM);
       
                        await Task.Run(() => _authenticationService.CurrentUser.ExecuteTransaction(transaction)); //_transactionService.CommitTransaction(transaction);

                        unitOfWork.Commit();
                    }                  
                }
                catch (Exception ex)
                {
                    // todo log
                    return BadRequest("Transaction failed");
                }
            }

            return Ok();
        }
    }
}
