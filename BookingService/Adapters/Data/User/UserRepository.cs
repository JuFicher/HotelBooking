using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Data.User
{
    public class UserRepository : IUserRepository
    {
        private readonly HotelDbContext _hotelDbContext;
        public UserRepository(HotelDbContext hotelDbContext)
        {
            _hotelDbContext = hotelDbContext;
        }

        public async Task<string> Create(Domain.Entities.User user)
        {
            _hotelDbContext.Users.Add(user);
            await _hotelDbContext.SaveChangesAsync();
            return user.Name;
        }
        
        public Task<Domain.Entities.User> Get(string UserName)
        {
            return _hotelDbContext.Users
                .Where(user => user.Name == UserName).FirstOrDefaultAsync();
        }
    }
}