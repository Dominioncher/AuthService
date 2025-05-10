using AuthService.Controllers.DTO;
using AuthService.Core;
using AuthService.Services;
using AuthService.Services.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("GetActiveUsers")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetActiveUsers()
    {
        var users = _userService.GetActiveUsers();
        return Ok(users);
    }

    [HttpGet("GetUsersOlderThenAge/{age}")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetUsersOlderThenAge(int age)
    {
        if (age <= 0)
        {
            ModelState.AddModelError(nameof(age), "The age must be > 0");
            return BadRequest(ModelState);
        }

        var users = _userService.GetAllUsers().Where(x => x.BirthDay != null && x.BirthDay.Value.Age() >= age);
        return Ok(users);
    }

    [HttpGet("GetUser/{login}")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetUser(string login)
    {
        var user = _userService.GetUser(login);
        var response = new { user.Name, user.Gender, user.BirthDay, IsActive = user.RevokedOn == null };
        return Ok(response);
    }

    [HttpGet("GetUser/{login}/{password}")]
    public IActionResult GetUser(string login, string password)
    {
        var user = _userService.GetUser(login);
        var isCurrentUser = user.Id == Guid.Parse(User.Identity.Name);
        var isValidPassword = _userService.VerifyUser(user.Id, password);

        if (!isCurrentUser || !isValidPassword)
        {
            return UnprocessableEntity("Uncorrect login or password");
        }

        return Ok(user);
    }



    [HttpPost("RestoreUser")]
    [Authorize(Roles = "Admin")]
    public IActionResult RestoreUser([FromBody] RestoreUserRequest request)
    {
        var user = _userService.GetUser(request.Login);
        var initiator = Guid.Parse(User.Identity.Name);
        _userService.RestoreUser(initiator, user.Id);
        return Ok();
    }

    [HttpPost("CreateUser")]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateUser([FromBody] CreateUserRequest request)
    {
        var initiator = Guid.Parse(User.Identity.Name);
        var dto = new CreateUserDto()
        {
            Initiator = initiator,
            Login = request.Login,
            Password = request.Password
        };
        if (request.Name != null)
        {
            dto.Name = request.Name;
        }
        if (request.Gender != null)
        {
            dto.Gender = (Gender)request.Gender;
        }
        if (request.BirthDay != null)
        {
            dto.BirthDay = request.BirthDay;
        }
        if (request.Admin != null)
        {
            dto.Admin = (bool)request.Admin;
        }

        var user = _userService.CreateUser(dto);
        return Ok(user);
    }

    [HttpDelete("RemoveUser")]
    [Authorize(Roles = "Admin")]
    public IActionResult RemoveUser([FromBody] RemoveUserRequest request)
    {
        var initiator = Guid.Parse(User.Identity.Name);
        var user = _userService.GetUser(request.Login);
        _userService.RemoveUser(user.Id, initiator, request.Permanent ?? false);
        return Ok();
    }

    [HttpPut("ChangePassword")]
    public IActionResult ChangePassword([FromBody] ChangePasswordRequest request, [FromQuery] string? login = null)
    {
        var currentUser = _userService.GetUser(Guid.Parse(User.Identity.Name));
        var adminUse = currentUser.Admin && login != null;
        var user = adminUse ? _userService.GetUser(login) : currentUser;

        var dto = new ChangePasswordDto()
        {
            UserId = user.Id,
            Password = request.NewPassword,
            Initiator = adminUse ? currentUser.Id : null
        };

        _userService.ChangePassword(dto);
        return Ok();
    }

    [HttpPut("ChangeLogin")]
    public IActionResult ChangeLogin([FromBody] ChangeLoginRequest request, [FromQuery] string? login = null)
    {
        var currentUser = _userService.GetUser(Guid.Parse(User.Identity.Name));
        var adminUse = currentUser.Admin && login != null;
        var user = adminUse ? _userService.GetUser(login) : currentUser;

        var dto = new ChangeLoginDto()
        {
            UserId = user.Id,
            Login = request.NewLogin,
            Initiator = adminUse ? currentUser.Id : null
        };

        _userService.ChangeLogin(dto);
        return Ok();
    }

    [HttpPut("UpdateUser")]
    public IActionResult UpdateUser([FromBody] UpdateUserRequest request, [FromQuery] string? login = null)
    {
        if (request.Name == null && request.Gender == null & request.BirthDay == null)
        {            
            return ValidationProblem("No one parameter for update");
        }

        var currentUser = _userService.GetUser(Guid.Parse(User.Identity.Name));
        var adminUse = currentUser.Admin && login != null;
        var user = adminUse ? _userService.GetUser(login) : currentUser;

        var dto = new UpdateUserDto()
        {
            UserId = user.Id,
            Name = request.Name,
            Gender = request.Gender,
            BirthDay = request.BirthDay,
            Initiator = adminUse ? currentUser.Id : null
        };

        var updatedUser = _userService.UpdateUser(dto);        
        return Ok(updatedUser);

    }

}
