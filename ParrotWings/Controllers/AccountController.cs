using System;
using System.Threading.Tasks;
using System.Web.Http;
using Interfaces;
using ParrotWings.ViewModel;
using TransactionSubsystem.Repositories.Abstract;

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
    
        // POST: /account/register
        [Route("api/account/register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register([FromBody] RegistrationViewModel user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var _user = await _userProvider.CreateUser(user.Username, user.Email, user.Password);

                    if (_user == null)
                    {
                        return BadRequest("Failed register user");
                    }
                }
                catch (Exception ex)
                {
                    // todo logging
                    return BadRequest("Failed register user");
                }
            }

            return Ok();
        }


        [Route("api/account/login")]
        [HttpPost]
        public async Task<IHttpActionResult> Login(LoginViewModel credentials) //, string returnUrl
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var authResult = await _authenticationService.Login(credentials.Email, credentials.Password);

                    if (!authResult)
                    {
                        return BadRequest("Failed login");
                    }
                }
                catch (Exception ex)
                {
                    // TODO: logging
                    return BadRequest("Failed login");
                }
            }

            return Ok();
        }

        [Route("api/account/logout")]
        [HttpPost]
        public IHttpActionResult Logout()
        {
            _authenticationService.CurrentUser = null;
            return Ok();
        }
    }
}