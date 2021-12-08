using System.Collections.Generic;
using WebService.Models;

namespace WebService.Interfaces
{
    public interface IAuthenticatorService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
    }
}