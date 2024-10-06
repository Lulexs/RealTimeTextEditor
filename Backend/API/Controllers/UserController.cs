using API.DTOs;
using Models;
using Persistence.Interfaces;

namespace API.Controllers;

public class UserController : BaseController {

    private IAsyncAccess<User> UserRepository { get; set; } 

    public UserController(IAsyncAccess<User> userRepository) {
        UserRepository = userRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]UserRegisterDto userRegisterDto) {
        await Task.Delay(1000);
        return BadRequest();
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetUser(string username) {
        return Ok(await UserRepository.GetOneByPropertyAsync(nameof(Models.User.Username), username));
    }
}