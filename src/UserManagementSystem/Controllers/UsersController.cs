using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.BLL.Models.Users;
using UserManagementSystem.BLL.Services;
using UserManagementSystem.Models.Users.Requests;
using UserManagementSystem.Models.Users.Responses;
using UserManagementSystem.Validators;

namespace UserManagementSystem.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly CreateUserRequestValidator _validator;

        public UsersController(UserService userService, CreateUserRequestValidator validator)
        {
           _userService = userService;
           _validator = validator;
        }

        [HttpGet]
        public async Task<GetUsersResponse[]> Get()
        {
           return (await _userService.GetUsersList())
                .Select(r => new GetUsersResponse()
                {
                    Name = r.Name,
                    Age = r.Age,
                    Email = r.Email,
                })
                .ToArray();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(long id)
        {
            var serviceResult = await _userService.GetUser(id);

            if (serviceResult == null)
            {
                return NotFound();
            }

            return Ok (new GetUsersResponse()
            {
                Name = serviceResult.Name,
                Age= serviceResult.Age,
                Email = serviceResult.Email,
            });
        }

        [HttpGet("userfulldata/{id}")]
        public async Task<IActionResult> GetUserFullData(long id)
        {
            var serviceResult = await _userService.GetUserFullData(id);
            
            if (serviceResult == null)
            {
                return NotFound();
            }

            return Ok(new GetUserFullDataResponse()
            {
                Name = serviceResult.Name,
                Age = serviceResult.Age,
                Email = serviceResult.Email,
                Phones = serviceResult.Phones
                .Select(r => new GetUserFullDataPhonesResponse()
                {
                    PhoneNumber = r.PhoneNumber,
                })
                .ToList()
            });
        }

        [HttpGet("altuserfulldata/{id}")]
        public async Task<IActionResult> AltGetUserFullData(long id)
        {
            var serviceResult = await _userService.AltGetUserFullData(id);

            if (serviceResult == null)
            {
                return NotFound();
            }

            return Ok(serviceResult);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequest user)
        {
            var validationResult = _validator.Validate(user);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult);
            }

            var userModel = new CreateUserModel()
            {
                Name = user.Name,
                Age = user.Age,
                Email = user.Email,
            };

            await _userService.CreateUser(userModel);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserRequest user)
        {
            var userModel = new UpdateUserModel()
            {
                Id = user.Id,
                Name = user.Name,
                Age = user.Age,
                Email= user.Email,
            };

            await _userService.UpdateUser(userModel);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(long id)
        {
            await _userService.DeleteUser(id);

            return Ok();
        }
    }
}
