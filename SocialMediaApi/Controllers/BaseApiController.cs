using Microsoft.AspNetCore.Mvc;

namespace SocialMediaApi.Controllers
{
  [ApiController]
  [Produces("application/json")]
  [Route("api/v{version:apiVersion}/[controller]")]
  
  public abstract class BaseApiController : ControllerBase
  {

  }
}
