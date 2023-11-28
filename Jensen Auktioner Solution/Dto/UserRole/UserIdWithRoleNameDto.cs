namespace Jensen_Auktioner_Solution.Dto.UserRole
{
    public class UserIdWithRoleNameDto
    {
        public int UserId { get; set; }
        public string PasswordHash { get; set; }
        public string RoleName { get; set; }
    }
}
