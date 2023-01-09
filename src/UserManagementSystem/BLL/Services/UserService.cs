using UserManagementSystem.BLL.Models.Users;
using UserManagementSystem.DAL.Models;
using UserManagementSystem.DAL.Repositories;
using static System.Net.Mime.MediaTypeNames;

namespace UserManagementSystem.BLL.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly PhoneRepository _phoneRepository;

        public UserService(UserRepository userRepository, PhoneRepository phoneRepository)
        {
            _userRepository = userRepository;
            _phoneRepository = phoneRepository;
        }

        public async Task<GetUsersListResult[]> GetUsersList()
        {
            return (await _userRepository.GetUsersList())
                .Select(r => new GetUsersListResult()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Age = r.Age,
                    Email = r.Email
                })
                .ToArray();
        }

        public async Task<GetUsersListResult> GetUser(long id)
        {
            var dalResult = await _userRepository.GetUser(id);

            if (dalResult == null)
            {
                return null;
            }

            return new GetUsersListResult()
            {
                Id = dalResult.Id,
                Name = dalResult.Name,
                Age= dalResult.Age,
                Email = dalResult.Email,
            };
        }

        public async Task<GetUserFullDataResult> GetUserFullData(long id)
        {
            var userDalResult = await _userRepository.GetUser(id);

            if (userDalResult == null)
            {
                return null;
            }

            var phoneDalResult = await _phoneRepository.GetUserPhonesList(id);

            return new GetUserFullDataResult()
            {
                Name = userDalResult.Name,
                Age = userDalResult.Age,
                Email = userDalResult.Email,
                Phones = phoneDalResult
                .Select(r => new GetUserFullDataPhonesResult()
                {
                    PhoneNumber = r.PhoneNumber,
                })
                .ToList(),
            };
        }

        public async Task<AltGetUserFullDataResult[]> AltGetUserFullData(long id)
        {
            var dalResult = await _userRepository.AltGetUserFullData(id);

            if (dalResult.Length == 0)
            {
                return null;
            }

            return (dalResult
                .Select(r => new AltGetUserFullDataResult()
                {
                    Name = r.Name,
                    Age = r.Age,
                    Email = r.Email,
                    PhoneNumber = r.PhoneNumber,
                }))
                .ToArray();
        }

        public async Task CreateUser(CreateUserModel user)
        {
            var userDal = new UserDal()
            {
                Name = user.Name,
                Age = user.Age,
                Email = user.Email,
                CreatedAt = DateTime.Now,
            };
            await _userRepository.CreateUser(userDal);
        }

        public async Task UpdateUser(UpdateUserModel user)
        {
            var userDal = new UserDal()
            {
                Id = user.Id,
                Name = user.Name,
                Age = user.Age,
                Email = user.Email,
            };

            await _userRepository.UpdateUser(userDal);
        }

        public async Task DeleteUser(long id)
        {
            await _userRepository.DeleteUser(id);
        }
    }
}
