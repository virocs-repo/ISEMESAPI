using ISEMES.Models;

namespace ISEMES.Repositories
{
    public interface IUserRepository
    {
        Task<AppMenuResponse> GetRoles(string? email, string? userName, string? password);
    }
}



