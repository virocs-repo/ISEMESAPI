using ISEMES.Models;

namespace ISEMES.Services
{
    public interface IUserService
    {
        Task<AppMenuResponse> GetRoles(string? email, string? userName, string? password);
    }
}

