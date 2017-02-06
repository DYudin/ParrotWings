using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ParrotWings.ViewModel;
using TransactionSubsystem.Entities;
using System.Threading.Tasks;
using AutoMapper;
using TransactionSubsystem.Infrastructure.Services.Abstract;
using TransactionSubsystem.Infrastructure.UnitOfWork.Abstract;

namespace ParrotWings.Controllers
{
    [Authorize]
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

            List<Transaction> transactionsList = null;
            await Task.Run(() =>
            {
                transactionsList = _authenticationService.CurrentUser.Transactions.ToList();
            });

            Mapper.Initialize(cfg => cfg.CreateMap<Transaction, TransactionViewModel>()
                .ForMember(
                    dest => dest.CorrespondedUser,
                    opt => opt.MapFrom(
                        src => src.Recepient.Name == _authenticationService.CurrentUser.Name
                            ? src.TransactionOwner.Name
                            : src.Recepient.Name))
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

            return Ok(transactionsVM);
        }

        [Route("api/transaction/allusers")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUsers()
        {
            List<UserViewModel> usersVM;

            var users = await _userProvider.GetUsers();
            Mapper.Initialize(cfg => cfg.CreateMap<User, UserViewModel>());
            usersVM = Mapper.Map<IEnumerable<User>, List<UserViewModel>>(users); //todo unitOfWork.Drinks.GetAll()
            
            return Ok(usersVM);
        }

        [Route("api/transaction/sendmoney")]
        [HttpPost]
        public async Task<IHttpActionResult> CommitTransaction(TransactionViewModel transactionVM)
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

                await Task.Run(() => _authenticationService.CurrentUser.ExecuteTransaction(transaction));

                unitOfWork.Commit();
            }

            return Ok();
        }
    }
}
