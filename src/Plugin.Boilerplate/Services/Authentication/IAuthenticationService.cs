using System.Threading.Tasks;

namespace Plugin.Boilerplate.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<bool> UserIsAuthenticatedAndValidAsync();
    }
}
