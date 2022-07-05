using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dating_App___API.DTOs;
using Dating_App___API.Entities;
using Dating_App___API.Helpers;
using Dating_App___API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dating_App___API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
                //NORMAL WAY//
                /*.Where(x => x.UserName == username)
                .Select(user => new MemberDto
                {
                    AppUserId = user.AppUserId,
                    Username = user.UserName,
                    ... etc.
                }).SingleOrDefaultAsync();*/

                //USING AUTOMAPPER//
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

            var query = _context.Users
                .Where(u => u.UserName != userParams.CurrentUserName)
                .Where(u => u.Gender == userParams.Gender)
                .Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .AsNoTracking();

            return await PagedList<MemberDto>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
                .Include(p => p.Photos)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}
