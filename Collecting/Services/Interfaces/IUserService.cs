using Collecting.Data.Models;

namespace Collecting.Services.Interfaces
{
    public interface IUserService
    {
        public bool IsValidUserInformation(Login model);
        public User? GetUserDetails(string accountEmail);
    }
}
