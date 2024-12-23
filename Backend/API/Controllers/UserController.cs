using ApplicationLogic;
using ApplicationLogic.DTOs;
using ApplicationLogic.Exceptions;
using Models;

namespace API.Controllers;

public class UserController : BaseController {

    private UserLogic _userLogic;

    public UserController(UserLogic userLogic) {
        _userLogic = userLogic;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register([FromBody] UserRegisterDto userRegisterDto) {
        try {
            var user = await _userLogic.RegisterUser(userRegisterDto);
            return Ok(user);
        }
        catch (AlreadyExistsException ec) {
            return Conflict(ec.Message);
        }
        catch (Exception ec) {
            return BadRequest(ec.Message);
        }
    }

    // [HttpGet("{username}")]
    // public async Task<IActionResult> GetUser(string username) {
    //     return Ok(await UserRepository.GetOneByPropertyAsync(nameof(Models.User.Username), username));
    // }
}