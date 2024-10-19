using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using NEWS_App.Models.IRepository;
using System.Text;

namespace NEWS_App.Models.IRepositoryImpl
{

    public class Userrepository : IUserRepository
    {
        AppDbContext _context;
        public Userrepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Set<User>().FindAsync(id);
        }
        public async Task<User> GetByUsernameAsync(String email)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User u)
        {
            _context.Set<User>().Add(u);
            _context.SaveChanges();
        }

        public async Task<bool> ValidateUser(string email, string password)
        {
            // Fetch the user by username
            var user = await _context.Set<User>()
                .FirstOrDefaultAsync(u => u.Email == email);

            // If no user found, return false
            if (user == null)
            {
                return false;
            }

            else
            {
                if (user.PasswordHash == password)
                    return true;
                return false;
            }
        }

    }
}
