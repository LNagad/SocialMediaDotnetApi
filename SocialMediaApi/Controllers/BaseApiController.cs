using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SocialMediaApi.Controllers
{
  [ApiController]
  [Produces("application/json")]
  [Route("api/v{version:apiVersion}/[controller]")]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public abstract class BaseApiController : ControllerBase
  {
    private IMediator _mediator;
    //protected IMediator Mediator => _mediator ??= (IMediator)HttpContext.RequestServices.GetService(typeof(IMediator));

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
  }
}
