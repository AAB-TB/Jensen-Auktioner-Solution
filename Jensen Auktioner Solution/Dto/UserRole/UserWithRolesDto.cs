namespace Jensen_Auktioner_Solution.Dto.UserRole
{
    public class UserWithRolesDto
    {
        public string UserName { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
