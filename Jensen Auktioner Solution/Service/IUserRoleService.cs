using Jensen_Auktioner_Solution.Dto.UserRole;

namespace Jensen_Auktioner_Solution.Service
{
    public interface IUserRoleService
    {
        Task<UserWithRolesDto> UserRolesCheckAsync(string userName);
        Task<bool> AssignRoleToUserAsync(int userId, int roleId);
        Task<IEnumerable<UserRoleDto>> GetAllUsersWithRolesAsync();
        Task<bool> UpdateUserRolesAsync(int userId, IEnumerable<int> roleIds);
        Task<bool> RemoveUserRolesAsync(int userId, IEnumerable<int> roleIds);
    }
}
