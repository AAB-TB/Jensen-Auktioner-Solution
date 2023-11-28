using Jensen_Auktioner_Solution.Dto.Role;

namespace Jensen_Auktioner_Solution.Service
{
    public interface IRoleService
    {
        Task<bool> CreateRoleAsync(string roleName);
        Task<bool> DeleteRoleAsync(int roleId);
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
    }
}
