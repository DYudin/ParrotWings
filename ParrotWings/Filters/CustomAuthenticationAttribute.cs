using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using System.Security.Principal;

namespace ParrotWings.Filters
{
    class CustomAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public Task AuthenticateAsync(HttpAuthenticationContext context,
            CancellationToken cancellationToken)
        {
            context.Principal = null;
            AuthenticationHeaderValue authentication = context.Request.Headers.Authorization;
            if (authentication != null && authentication.Scheme == "Basic")
            {
                string login = Encoding.ASCII.GetString(Convert.FromBase64String(authentication.Parameter));
                string[] roles = new string[] {"user"};
                
                context.Principal = new GenericPrincipal(new GenericIdentity(login), roles);
            }
            if (context.Principal == null)
            {
                context.ErrorResult
                    = new UnauthorizedResult(new AuthenticationHeaderValue[]
                    {
                        new AuthenticationHeaderValue("Basic")
                    }, context.Request);
            }
            return Task.FromResult<object>(null);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<object>(null);
        }

        public bool AllowMultiple
        {
            get { return false; }
        }
    }
}