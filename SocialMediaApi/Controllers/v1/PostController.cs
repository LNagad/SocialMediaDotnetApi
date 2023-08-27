using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia.Core.Aplication.DTOs.CustomEntities;
using SocialMedia.Core.Aplication.Enums;
using SocialMedia.Core.Aplication.Features.Posts.Commands.CreatePost;
using SocialMedia.Core.Aplication.Features.Posts.Commands.DeletePostById;
using SocialMedia.Core.Aplication.Features.Posts.Commands.UpdatePostById;
using SocialMedia.Core.Aplication.Features.Posts.Queries.GetAllPosts;
using SocialMedia.Core.Aplication.Features.Posts.Queries.GetPostById;
using SocialMedia.Core.Aplication.QueryFilters;
using SocialMedia.Core.Aplication.Wrappers;
using SocialMedia.Core.Domain.Entities;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Validators;
using SocialMedia.Infrastructure.Services.Interfaces;
using SocialMediaApi.Responses;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace SocialMediaApi.Controllers.v1
{
    [ApiVersion("1.0")]
  [Authorize(Roles = nameof(Roles.Admin) )]

  [SwaggerTag("Posts Maintenance")]
  public class PostController : BaseApiController
  {
    private readonly IUriService _uriService;
    private readonly IMapper _mapper;

    public PostController(IUriService uriService, IMapper mapper)
    {
      _uriService = uriService;
      _mapper = mapper;
    }

    [HttpGet(Name = nameof(GetPosts))]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PostDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
      Summary = "Posts list",
      Description = "Get all posts paginated and with query filters"
    )]
    public async Task<IActionResult> GetPosts([FromQuery] GetAllPostParameters filters)
    {
      var (postsDto, pagedPost) = await Mediator.Send(new GetAllPostsQuery() { Parameters = filters });

      //var (postsDto, pagedPost) = postTuple;

      var metadata = CreateMetadata(pagedPost, filters);

      var apiResponse = new Response<IEnumerable<PostDto>>(postsDto, meta: metadata);

      Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
      return Ok(apiResponse);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
      Summary = "Post by id",
      Description = "Get a post filtering by the post id"
    )]
    public async Task<IActionResult> GetPost(int id)
    {
      return Ok(await Mediator.Send(new GetPostByIdQuery() { Id = id }));
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
      Summary = "Post creating",
      Description = "Retrieve the necessary params to create a new post"
    )]
    public async Task<IActionResult> Post([FromBody] CreatePostCommand command)
    {
      //var result = await _validator.ValidateAsync(command);

      //if (!result.IsValid)
      //{
      //  return BadRequest(Results.ValidationProblem(result.ToDictionary()));
      //}

      return Ok(await Mediator.Send(command));
    }

    [HttpPut("{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
      Summary = "Post updating",
      Description = "Retrieve the necessary params to modify an existing post"
    )]
    public async Task<IActionResult> Put(int id, [FromBody] UpdatePostCommand command)
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
      Summary = "Post deleting",
      Description = "Retrieve the necessary params to delete an existing post"
    )]
    public async Task<IActionResult> Delete(int id)
    {
      await Mediator.Send(new DeletePostByIdCommand() { Id = id });
      return NoContent();
    }

    #region Private Methods
    private Metadata CreateMetadata(PagedList<Post> pagedPost, GetAllPostParameters filters)
    {
      var postQueryFilter = _mapper.Map<PostQueryFilter>(filters);

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
    #endregion
  }
}
