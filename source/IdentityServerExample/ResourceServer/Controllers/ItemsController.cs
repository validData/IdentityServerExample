using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ResourceServer.Controllers
{
    [Route("api/v1/items")]
    [Authorize]
    public class ItemsController : SecuredController
    {
        private const string PermissionReadItems = "resource-server.ReadItems";
        
        public ItemsController(IOptions<AuthServerConfig> configuration) : base(configuration)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            // make sure the user has the required permissions
            await RequirePermissionAsync(PermissionReadItems);

            return Ok(new[] {"Item1", "Item2"});
        }
    }
}