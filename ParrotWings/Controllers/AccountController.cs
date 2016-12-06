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

    //[HttpPost("logout")]
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

    // POST: /account/register
    [Route("register")]
    [HttpPost]
    public async Task<IHttpActionResult> Register([FromBody] RegistrationViewModel user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        IHttpActionResult result = Ok();

        try
        {
            var _user = await _userProvider.CreateUser(user.Username, user.Email, user.Password);

            if (_user != null)
            {
                result = new BadRequestResult(this);
            }

        }
        catch (Exception ex)
        {
            result = new ExceptionResult(ex, this);
        }
       
        return result;
    }

    //
    // POST: /account/login
    [Route("login")]
    [HttpPost]
    [AllowAnonymous]
    public async Task<IHttpActionResult> Login(LoginViewModel model, string returnUrl)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
       
        var result = await _authenticationService.Login(model.Email, model.Password);

        if (result)
        {
            return Ok();
        }
        else
        {
            ModelState.AddModelError("", "Invalid login attempt.");
            return BadRequest(ModelState);
        }
    }}