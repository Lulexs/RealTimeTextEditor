using API.DTOs;
using Models;
using Persistence.Repositories;

namespace API.Controllers;

public class UserController : BaseController {

    private UserRepository UserRepository { get; set; } 

    public UserController(UserRepository userRepository) {
        UserRepository = userRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]UserRegisterDto userRegisterDto) {
        await Task.Delay(1000);
        return BadRequest();
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUser(Guid userId) {
        return Ok(await UserRepository.GetOneByIdAsync<User>(userId));
    }
}