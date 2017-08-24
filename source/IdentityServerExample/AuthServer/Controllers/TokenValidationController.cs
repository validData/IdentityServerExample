using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers
{
    [Authorize]
    [Route("api/v1/token-validation")]
    public class TokenValidationController : Controller
    {
        [HttpPost]
        public IActionResult ValidateToken()
        {
            var query = from claim in User.Claims
                group claim by claim.Type
                into claimsByType
                select new
                {
                    Type = claimsByType.Key,
                    Value = claimsByType.Count() == 1 ? (object) claimsByType.First().Value : (object) claimsByType.Select(c=>c.Value).ToArray()
                };

            var response = query.ToDictionary(c => c.Type, c => c.Value);
            return Ok(response);
        }
    }
}