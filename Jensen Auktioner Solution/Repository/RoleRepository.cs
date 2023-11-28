using AutoMapper;
using Dapper;
using Jensen_Auktioner_Solution.Data;
using Jensen_Auktioner_Solution.Dto.Role;
using Jensen_Auktioner_Solution.Model;
using Jensen_Auktioner_Solution.Service;
using System.Data;

namespace Jensen_Auktioner_Solution.Repository
{
    public class RoleRepository : IRoleService
    {
        private readonly DapperContext _dapperContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleRepository> _logger;


        public RoleRepository(
            DapperContext dapperContext,
            IMapper mapper,
            ILogger<RoleRepository> logger
            )
        {
            _dapperContext = dapperContext;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            try
            {
                using (var connection = _dapperContext.GetDbConnection())
                {
                    connection.Open();

                    // Create DynamicParameters
                    var parameters = new DynamicParameters();
                    parameters.Add("RoleName", roleName);

                    // Execute the stored procedure
                    var result = await connection.QueryFirstOrDefaultAsync<int>("sp_CreateRole", parameters, commandType: CommandType.StoredProcedure);

                    // Check the result to determine success or failure
                    return result == 1;
                }
            }
            catch (Exception ex)
            {
                // Log unexpected exceptions
                _logger.LogError($"Unexpected error while creating role: {ex.Message}");
                // Return false to indicate failure in case of an exception
                return false;
            }
        }

        public async Task<bool> DeleteRoleAsync(int roleId)
        {
            try
            {
                using (var connection = _dapperContext.GetDbConnection())
                {
                    connection.Open();

                    // Create DynamicParameters
                    var parameters = new DynamicParameters();
                    parameters.Add("RoleId", roleId);

                    // Execute the stored procedure
                    var result = await connection.QueryFirstOrDefaultAsync<int>("sp_DeleteRole", parameters, commandType: CommandType.StoredProcedure);

                    // Check the result to determine success or failure
                    return result == 1;
                }
            }
            catch (Exception ex)
            {
                // Log unexpected exceptions
                _logger.LogError($"Unexpected error while deleting role: {ex.Message}");
                // Rethrow the exception or handle it as appropriate for your application
                throw;
            }
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            try
            {
                using (var connection = _dapperContext.GetDbConnection())
                {
                    connection.Open();

                    // Execute the stored procedure
                    var roles = await connection.QueryAsync<Role>("sp_GetAllRoles", commandType: CommandType.StoredProcedure);

                    // Map Role to RoleDto using _mapper
                    var roleDtoList = _mapper.Map<IEnumerable<RoleDto>>(roles);

                    return roleDtoList;
                }
            }
            catch (Exception ex)
            {
                // Log unexpected exceptions
                _logger.LogError($"Unexpected error while retrieving roles: {ex.Message}");
                // Rethrow the exception or handle it as appropriate for your application
                throw;
            }
        }
    }
}
