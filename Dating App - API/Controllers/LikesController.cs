using Dating_App___API.DTOs;
using Dating_App___API.Entities;
using Dating_App___API.Extensions;
using Dating_App___API.Helpers;
using Dating_App___API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dating_App___API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;

        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
            _userRepository = userRepository;
            _likesRepository = likesRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();
            var likedUser = await _userRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);

            if (likedUser == null) return NotFound();
            if (sourceUser.UserName == username) return BadRequest("Cannot like yourself");
            
            var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.AppUserId);

            if(userLike != null) return BadRequest("User already liked");

            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.AppUserId
            };

            sourceUser.LikedUsers.Add(userLike);
            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users = await _likesRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);
        }
    }
}
