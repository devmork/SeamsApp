using System.Security.Claims;

namespace SeamsApp.Utilities
{
    public static class ClaimsUtility
    {
        /// <summary>
        /// Extracts and validates the UserId from the JWT token claims.
        /// </summary>
        /// <param name="httpContext">The current HttpContext.</param>
        /// <returns>The UserId as an integer, or throws UnauthorizedAccessException if invalid.</returns>
        public static int GetUserIdFromClaims(HttpContext httpContext)
        {
            var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("Invalid user token.");
            }
            return userId;
        }

        /// <summary>
        /// Extracts the user's roles from the JWT token claims.
        /// </summary>
        /// <param name="httpContext">The current HttpContext.</param>
        /// <returns>A list of role names.</returns>
        public static List<string> GetUserRolesFromClaims(HttpContext httpContext)
        {
            var roleClaims = httpContext.User.FindAll(ClaimTypes.Role);
            return roleClaims.Select(c => c.Value).ToList();
        }

        /// <summary>
        /// Checks if the user has a specific role.
        /// </summary>
        /// <param name="httpContext">The current HttpContext.</param>
        /// <param name="role">The role to check.</param>
        /// <returns>True if the user has the role, otherwise false.</returns>
        public static bool HasRole(HttpContext httpContext, string role)
        {
            return httpContext.User.IsInRole(role);
        }

        /// <summary>
        /// Extracts the user's email from the JWT token claims.
        /// </summary>
        /// <param name="httpContext">The current HttpContext.</param>
        /// <returns>The user's email, or null if not found.</returns>
        public static string? GetUserEmailFromClaims(HttpContext httpContext)
        {
            return httpContext.User.FindFirstValue(ClaimTypes.Email);
        }
    }
}
