using Collecting.Data;
using Collecting.Data.Models;
using Collecting.Services.Interfaces;

namespace Collecting.Services.Implimentations
{
    public class UserService : IUserService
    {
        private readonly StickersContext _context;
        public UserService(StickersContext context)
        {
            _context = context;
        }
        public bool IsValidUserInformation(Login model)
        {
            User? user = _context.UsersDb
                .Where(u => u.Email == model.UserEmail)
                .FirstOrDefault();

            if (user == null)
            {
                return false;
            }

            return model.Password.Equals(user.Password);
        }

        public User? GetUserDetails(string accountEmail)
        {
            User? user = _context.UsersDb
                .Where(u => u.Email == accountEmail)
                .FirstOrDefault();

            return user;
        }
    }
}
