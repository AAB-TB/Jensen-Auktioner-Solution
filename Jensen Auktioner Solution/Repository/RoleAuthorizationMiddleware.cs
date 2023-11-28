using System.Security.Claims;

namespace Jensen_Auktioner_Solution.Repository
{
    public class RoleAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Exclude the authentication path from role authorization
            if (context.Request.Path.Equals("/api/user/login", StringComparison.OrdinalIgnoreCase) ||
    context.Request.Path.Equals("/api/user/Register", StringComparison.OrdinalIgnoreCase) ||
     context.Request.Path.Equals("/api/User/allUsers", StringComparison.OrdinalIgnoreCase) ||
     context.Request.Path.Equals("/api/Role/GetAllRoles", StringComparison.OrdinalIgnoreCase) ||
    context.Request.Path.Equals("/api/UserRole/GetAllUsersWithRoles", StringComparison.OrdinalIgnoreCase) ||
    context.Request.Path.Equals("/api/UserRole/user-roles", StringComparison.OrdinalIgnoreCase)||
    context.Request.Path.Equals("/api/Auction/GetAllAuctions", StringComparison.OrdinalIgnoreCase)||
    context.Request.Path.Equals("/api/Auction/SpeceficAuction", StringComparison.OrdinalIgnoreCase))

            {
                await _next.Invoke(context);
                return;
            }


            var user = context.User;
            foreach (var claim in user.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }


            // Check if the user has the "Admin" or "Customer" role.
            if (user.HasClaim(c => c.Type == ClaimTypes.Role && (c.Value == "Admin" || c.Value == "Customer")))
            {
                // User has either "Admin" or "Customer" role, allow access to the endpoint.
                await _next.Invoke(context);
            }
            else
            {
                // User doesn't have the required role, return unauthorized.
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
            }
        }
    }
}
