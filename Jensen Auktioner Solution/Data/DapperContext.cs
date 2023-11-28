using Microsoft.Data.SqlClient;
using System.Data;

namespace Jensen_Auktioner_Solution.Data
{
    public class DapperContext
    {
        private readonly string _connectionString;
      

        public DapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection GetDbConnection()
        {
            return new SqlConnection(_connectionString);
        }

    }
}