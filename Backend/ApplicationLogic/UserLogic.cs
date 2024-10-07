using ApplicationLogic.DTOs;
using ApplicationLogic.Exceptions;
using Models;
using Persistence.Interfaces;

namespace ApplicationLogic;

public class UserLogic {
    private readonly IAsyncAccess<User> _userRepository;

    public UserLogic(IAsyncAccess<User> userRepository) {
        _userRepository = userRepository;
    }

    public async Task<User> RegisterUser(UserRegisterDto userRegisterDto) {
        var user = await _userRepository.GetOneByPropertyAsync(nameof(User.Username), userRegisterDto.Username);

        if (user != null) {
            throw new AlreadyExistsException($"User with {userRegisterDto.Username} username already exists");
        }

        User newUser = new() {
            Username = userRegisterDto.Username,
            Password = userRegisterDto.Password,
            UserId = Guid.NewGuid()
        };

        await _userRepository.InsertOneAsync(newUser);

        return newUser;
    }
}