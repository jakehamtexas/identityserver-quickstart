using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Api
{
    [Route("identity")]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public JsonResult Get()
        {
            var claimJson = User.Claims.Select(claim => new { claim.Type, claim.Value });
            return new JsonResult(claimJson);
        }
    }
}
