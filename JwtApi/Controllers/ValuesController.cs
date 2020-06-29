namespace JwtApi.Controllers
{
    using System.Web.Http;

    public class ValuesController : ApiController
    {
        [Authorize(Roles = "administrator")]
        public string Get()
        {
            return $"Hello {this.User.Identity.Name}!";
        }
    }
}
