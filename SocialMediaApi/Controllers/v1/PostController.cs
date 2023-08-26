using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia.Core.Aplication.CustomEntities;
using SocialMedia.Core.Aplication.Enums;
using SocialMedia.Core.Aplication.Features.Posts.Commands.CreatePost;
using SocialMedia.Core.Aplication.Features.Posts.Commands.DeletePostById;
using SocialMedia.Core.Aplication.Features.Posts.Commands.UpdatePostById;
using SocialMedia.Core.Aplication.Features.Posts.Queries.GetAllPosts;
using SocialMedia.Core.Aplication.Features.Posts.Queries.GetPostById;
using SocialMedia.Core.Aplication.QueryFilters;
using SocialMedia.Core.Domain.Entities;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Validators;
using SocialMedia.Infrastructure.Services.Interfaces;
using SocialMediaApi.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace SocialMediaApi.Controllers.v1
{

  [ApiVersion("1.0")]
  [Authorize(Roles = nameof(Roles.Admin) )]

  public class PostController : BaseApiController
  {
    private readonly IUriService _uriService;

    public PostController(IUriService uriService)
    {
      _uriService = uriService;
    }

    [HttpGet(Name = nameof(GetPosts))]
    [SwaggerOperation(Summary = "Public: Get all pagedPost")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PostDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetPosts([FromQuery] GetAllPostParameters filters)
    {
      var postTuple = await Mediator.Send(new GetAllPostsQuery() { Parameters = filters });

      var (postsDto, pagedPost) = postTuple;

      var postQueryFilter = new PostQueryFilter
      {
        UserId = filters.UserId,
        Date = filters.Date,
        Description = filters.Description,
        PageSize = filters.PageSize,
        PageNumber = filters.PageNumber
      };

      var metadata = CreateMetadata(pagedPost, postQueryFilter);

      var apiResponse = new ApiResponse<IEnumerable<PostDto>>(postsDto){ Meta = metadata };

      Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
      return Ok(apiResponse);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<PostDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPost(int id)
    {
      return Ok(await Mediator.Send(new GetPostByIdQuery() { Id = id }));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PostDto>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post(CreatePostCommand command)
    {
      //var result = await _validator.ValidateAsync(command);

      //if (!result.IsValid)
      //{
      //  return BadRequest(Results.ValidationProblem(result.ToDictionary()));
      //}

      await Mediator.Send(command);

      return NoContent();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Put(int id, UpdatePostCommand command)
    {
      //var result = await _validator.ValidateAsync(command);

      //if (!result.IsValid)
      //{
      //  return BadRequest(Results.ValidationProblem(result.ToDictionary()));
      //}

      if (id != command.PostId)
      {
        return BadRequest();
      }

      return Ok(await Mediator.Send(command));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
      await Mediator.Send(new DeletePostByIdCommand() { Id = id });
      return NoContent();
    }

    private Metadata CreateMetadata(PagedList<Post> pagedPost, PostQueryFilter postQueryFilter)
    {
      var apiEndpoinRoute = Url.RouteUrl(nameof(GetPosts));
      var nextUri = _uriService.GetPostPaginationNextUrl(postQueryFilter, apiEndpoinRoute, pagedPost.HasNextPage).ToString();
      var prevUri = _uriService.GetPostPaginationPreviousUrl(postQueryFilter, apiEndpoinRoute, pagedPost.HasPreviousPage).ToString();

      return new Metadata
      {
        TotalCount = pagedPost.TotalCount,
        PageSize = pagedPost.PageSize,
        CurrentPage = pagedPost.CurrentPage,
        TotalPages = pagedPost.TotalPages,
        HasNextPage = pagedPost.HasNextPage,
        HasPreviousPage = pagedPost.HasPreviousPage,
        NextPageUrl = nextUri,
        PreviousPageUrl = prevUri
      };
    }

  }
}
