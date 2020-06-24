namespace JwtApi.Controllers
{
    using System.Web.Http;

    public class ValuesController : ApiController
    {
        // GET api/values
        [Authorize(Roles = "administrator")]
        public string Get()
        {
            return $"Hello {this.User.Identity.Name}!";
        }

      
    }
}
