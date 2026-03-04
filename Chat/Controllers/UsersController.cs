using Application.ModelsDTO;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserResponseDTO>> GetMe(
            [FromHeader] string JWT)
        {
            //var handler = new JwtSecurityTokenHandler();
            //var token = handler.ReadJwtToken(JWT);
            throw new NotImplementedException();
        }

        [HttpPost("signin")]
        [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserResponseDTO>> SignIn([FromBody] CredentialsDTO credentials)
        {
            try
            {
                var result = await _usersService.SignInAsync(credentials);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("refresh")]
        [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserResponseDTO>> Refresh([FromBody] TokenRefreshDTO dto)
        {
            try
            {
                var result = await _usersService.RefreshTokenAsync(dto.RefreshToken);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponseDTO>> Register([FromBody] RegisterDTO dto)
        {
            try
            {
                var result = await _usersService.RegisterAsync(dto);
                return CreatedAtAction(nameof(GetMe), result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}