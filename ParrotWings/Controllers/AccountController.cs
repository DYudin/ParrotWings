using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Interfaces;
using ParrotWings.ViewModel;
using TransactionSubsystem.Repositories.Abstract;

[Route("api/[controller]")]
public class AccountController : ApiController
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserRepository _userRepository;
    private readonly IUserProvider _userProvider;

    public AccountController(IAuthenticationService authenticationService,
                             IUserRepository userRepository,
                             IUserProvider userProvider)
    {
        _authenticationService = authenticationService;
        _userRepository = userRepository;
        _userProvider = userProvider;
    }
    
    // POST: /account/register
    [Route("api/account/register")]
    [HttpPost]
    public async Task<IHttpActionResult> Register([FromBody] RegistrationViewModel user)
    {
        Result registerResult = null;

        try
        {
            var _user = await _userProvider.CreateUser(user.Username, user.Email, user.Password);

            if (_user != null)
            {
                registerResult = new Result()
                {
                    Succeeded = true,
                    Message = "Register succeeded"
                };
            }
            else
            {
                registerResult = new Result()
                {
                    Succeeded = true,
                    Message = "Register failed"
                };
            }
        }
        catch (Exception ex)
        {
            registerResult = new Result()
            {
                Succeeded = false,
                Message = ex.Message
            };
        }

        return Ok(registerResult); ;
    }


    [Route("api/account/login")]
    [HttpPost]
    public async Task<IHttpActionResult> Login(LoginViewModel model) //, string returnUrl
    {
        Result loginResult = null;

        try
        {

            var authResult = await _authenticationService.Login(model.Email, model.Password);

            if (authResult)
            {
                loginResult = new Result()
                {
                    Succeeded = true,
                    Message = "Authentication succeeded"
                };

                //return Ok(loginResult);
            }
            else
            {
                loginResult = new Result()
                {
                    Succeeded = false,
                    Message = "Authentication failed"
                };                
            }
        }
        catch (Exception ex)
        {
            loginResult = new Result()
            {
                Succeeded = false,
                Message = ex.Message
            };
        }

        return Ok(loginResult);
    }

    [Route("api/account/logout")]
    [HttpPost]
    public async Task<IHttpActionResult> Logout()
    {
        try
        {
            // TODO: await HttpContext.GetOwinContext().Authentication.SignOutAsync("Cookies");
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }
}