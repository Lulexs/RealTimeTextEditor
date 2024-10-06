using API.DTOs;

namespace API.Controllers;

public class UserController : BaseController {
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]UserRegisterDto userRegisterDto) {
        await Task.Delay(1000);
        return BadRequest();
    }
}