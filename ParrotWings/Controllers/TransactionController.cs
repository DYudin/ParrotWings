using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using ParrotWings.ViewModel;
using TransactionSubsystem.Entities;
using System.Threading.Tasks;
using AutoMapper;
using ParrotWings.Filters;
using TransactionSubsystem.Infrastructure.Services.Abstract;
using TransactionSubsystem.Infrastructure.UnitOfWork.Abstract;

namespace ParrotWings.Controllers
{
    [CustomAuthentication]
    //[Authorize(Users = "Dmitriy, dima, alex, Alex, Ivan")]
    [Authorize(Roles = "user")]
    [Route("api/[controller]")]
    public class TransactionController : ApiController
    {
        private readonly IUserProvider _userProvider;     
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public TransactionController( 
            IAuthenticationService authenticationService,
            IUserProvider userProvider,            
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            if (userProvider == null) throw new ArgumentNullException(("userProvider"));           
            if (unitOfWorkFactory == null) throw new ArgumentNullException(("unitOfWorkFactory"));

            _userProvider = userProvider;          
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        [Route("api/transaction/currentuserinfo")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCurrentUserInfo()
        {
            string userName = RequestContext.Principal.Identity.Name;
            User currentUser = null;
            await Task.Run(() => currentUser = _userProvider.GetUserByName(userName));
            if (currentUser == null)
                return BadRequest("Internal server error. Please, contact with developers");

            var userVM = new UserViewModel {
                UserName = currentUser.Name,
                CurrentBalance = currentUser.CurrentBalance };
                   
            return Ok(userVM);
        }

        [Route("api/transaction/alltransactions")]
        [HttpGet]
        public async Task<IHttpActionResult> GetTransactions()
        {
            List<TransactionViewModel> transactionsVM;

            List<Transaction> transactionsList = null;
            User currentUser = null;
            string userName = RequestContext.Principal.Identity.Name;

            await Task.Run(() => currentUser = _userProvider.GetUserByName(userName));

            if (currentUser == null)
                return BadRequest("Internal server error. Please, contact with developers");

            await Task.Run(() => transactionsList = currentUser.Transactions.ToList());
          
            Mapper.Initialize(cfg => cfg.CreateMap<Transaction, TransactionViewModel>()
                .ForMember(
                    dest => dest.CorrespondedUser,
                    opt => opt.MapFrom(
                        src => src.Recepient.Name == currentUser.Name
                            ? src.TransactionOwner.Name
                            : src.Recepient.Name))
                .ForMember(
                    dest => dest.ResultingBalance,
                    opt => opt.MapFrom(
                        src => src.Recepient.Name == currentUser.Name
                            ? src.RecepientResultingBalance
                            : src.OwnerResultingBalance))
                .ForMember(
                    dest => dest.Outgoing,
                    opt => opt.MapFrom(
                        src => src.Recepient.Name != currentUser.Name)));

            transactionsVM = Mapper.Map<IEnumerable<Transaction>, List<TransactionViewModel>>(transactionsList);

            return Ok(transactionsVM);
        }

        [Route("api/transaction/allusers")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUsers()
        {
            List<UserViewModel> usersVM;

            var users = await Task.Run(() => _userProvider.GetUsers());
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
                string userName = RequestContext.Principal.Identity.Name;
                
                User recepient = null;
                User currentUser = null;
                await Task.Run(() =>
                {
                    recepient = _userProvider.GetUserByName(transactionVM.CorrespondedUser);
                    currentUser = currentUser = _userProvider.GetUserByName(userName);
                });

                if (recepient == null)
                    return BadRequest($"User with name: '{transactionVM.CorrespondedUser}' not found");
                
                if (currentUser == null)
                    return BadRequest("Internal server error. Please, contact with developers");

                Mapper.Initialize(cfg => cfg.CreateMap<TransactionViewModel, Transaction>()
                    .ForMember(
                        dest => dest.Recepient,
                        opt => opt.MapFrom(
                            src => recepient)));
                var transaction = Mapper.Map<TransactionViewModel, Transaction>(transactionVM);

                await Task.Run(() => currentUser.ExecuteTransaction(transaction));

                unitOfWork.Commit();
            }

            return Ok();
        }
    }
}
