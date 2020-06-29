namespace JwtApiCore.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [Authorize(Roles = "administrator")]
        [Route("Get")]
        [HttpGet]
        public string Get()
        {
            return $"Hello {this.User.Identity.Name}!";
        }
    }
}