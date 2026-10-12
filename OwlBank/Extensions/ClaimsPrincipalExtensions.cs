using System.Security.Claims;

namespace OwlBank.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var value = user.FindFirstValue("User Id");

            if (!Guid.TryParse(value, out var id))
                throw new UnauthorizedAccessException("Invalid or missing user id.");

            return id;
        }
    }
}