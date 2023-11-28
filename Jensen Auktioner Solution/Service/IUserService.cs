using Jensen_Auktioner_Solution.Dto.User;

namespace Jensen_Auktioner_Solution.Service
{
    public interface IUserService
    {
        Task<bool> UserRegistrationAsync(UserRegistrationDto registrationDto);
        Task<bool> UserDeleteAsync(int userId);
        Task<bool> UserUpdateAsync(int userId, UpdateUserDto updateUserDto, string oldPassword);
        Task<IEnumerable<UserInfoDto>> GetAllUsersAsync();
        Task<string> LoginUserAsync(string username, string password);
    }
}
