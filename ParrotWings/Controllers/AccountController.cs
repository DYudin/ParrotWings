using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ParrotWings.ViewModel;
using TransactionSubsystem.Infrastructure.Repositories.Abstract;
using TransactionSubsystem.Infrastructure.Services.Abstract;

namespace ParrotWings.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ApiController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserProvider _userProvider;

        public AccountController(IAuthenticationService authenticationService,
            IUserRepository userRepository,
            IUserProvider userProvider)
        {
            if (authenticationService == null) throw new ArgumentNullException(("authenticationService"));
            if (userProvider == null) throw new ArgumentNullException(("userProvider"));

            _authenticationService = authenticationService;
            _userProvider = userProvider;
        }
    
        [Route("api/account/register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register([FromBody] RegistrationViewModel user)
        {
            var _user = await Task.Run(() => _userProvider.CreateUser(user.Username, user.Email, user.Password));

            if (_user == null)
            {
                return BadRequest("Failed register user");
            }

            return Ok();
        }

        [Route("api/account/login")]
        [HttpPost]
        public async Task<IHttpActionResult> Login(LoginViewModel credentials)
        {
            var authResult = await Task.Run(() => _authenticationService.Login(credentials.Email, credentials.Password));
            return Ok(authResult);
        }

        [Route("api/account/logout")]
        [HttpPost]
        public IHttpActionResult Logout()
        {
            _authenticationService.LogOut();
            return Ok();
        }
    }
}