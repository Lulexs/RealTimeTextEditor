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
    public IActionResult GetUser(string username) {
        return Ok(UserRepository.GetOneByProperty(x => x.Username == username));
    }
}