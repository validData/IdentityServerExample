using System.Security.Claims;
using AuthServer.Model;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers
{
    [Authorize]
    [Route("api/v1/app-features")]
    public class AppFeaturesController : Controller
    {
        [HttpGet]
        public IActionResult GetAllFeatures()
        {
            var userId = User.GetSubjectId();
            // Todo determine features for user "userId"
            return Ok(new[] {new AppFeature("Feature1"), new AppFeature("Feature2")});
        }
    }
}