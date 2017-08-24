using AuthServer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers
{
    [Authorize]
    [Route("api/v1/permissions")]
    public class PermissionsController : Controller
    {
        [HttpGet]
        public IActionResult Get(string userId)
        {
            // determine if client can access user permissions
            var canAccessPermissions = User.FindFirst("client_permcl");
            if (canAccessPermissions == null || canAccessPermissions.Value != "true")
                return Forbid();
            var client = User.FindFirst("client_id")?.Value;
            if (client == null)
                return Forbid();

            // Todo: determine permissions for user "userId" in client "client"

            return Ok(new PermissionResponse
            {
                ClientId = client,
                UserId = userId,
                Permissions = new[]
                {
                    new Permission($"{client}.ReadItems"),
                    new Permission($"{client}.WriteItems")
                }
            });
        }
    }
}