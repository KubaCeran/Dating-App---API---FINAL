using Dating_App___API.DTOs;
using Dating_App___API.Entities;
using Dating_App___API.Helpers;

namespace Dating_App___API.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);

    }
}
