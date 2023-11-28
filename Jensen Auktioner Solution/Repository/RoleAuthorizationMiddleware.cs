using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Jensen_Auktioner_Solution.Repository
{
    public class RoleAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            // Paths to exclude from role authorization
            var excludedPaths = new[]
            {
                "/api/user/login",
                "/api/user/Register",
                "/api/User/allUsers",
                "/api/Role/GetAllRoles",
                "/api/UserRole/GetAllUsersWithRoles",
                "/api/UserRole/user-roles",
                "/api/Auction/GetAllAuctions",
                "/api/Auction/SpeceficAuction",
                "/api/Bid/winning",
            };

            // Check if the request path is in the exclusion list
            if (IsPathExcluded(context.Request.Path, excludedPaths))
            {
                await _next.Invoke(context);
                return;
            }

            // Your role authorization logic goes here
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

        private static bool IsPathExcluded(PathString requestPath, string[] excludedPaths)
        {
            foreach (var excludedPath in excludedPaths)
            {
                if (requestPath.Equals(excludedPath, StringComparison.OrdinalIgnoreCase) ||
                    requestPath.StartsWithSegments(excludedPath, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
