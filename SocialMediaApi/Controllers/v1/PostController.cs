using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia.Core.Aplication.CustomEntities;
using SocialMedia.Core.Aplication.Interfaces.Services;
using SocialMedia.Core.Aplication.QueryFilters;
using SocialMedia.Core.DTOs;
using SocialMedia.Infrastructure.Services.Interfaces;
using SocialMediaApi.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace SocialMediaApi.Controllers.v1
{

    [ApiVersion("1.0")]
  //[Authorize]

  public class PostController : BaseApiController
  {
    private readonly IPostService _postService;
    private readonly IValidator<PostDto> _validator;
    private readonly IUriService _uriService;

    public PostController(IPostService postService,
      IValidator<PostDto> validator, IUriService uriService)
    {
      _postService = postService;
      _validator = validator;
      _uriService = uriService;
    }

    [HttpGet(Name = nameof(GetPosts))]
    [SwaggerOperation(Summary = "Public: Get all posts")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PostDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetPosts([FromQuery] PostQueryFilter filters)
    {
      var postTuple = _postService.GetPosts(filters);
      var posts = postTuple.Item1;
      var postsDto = postTuple.Item2;

      var metadata = new Metadata
      {
        TotalCount = posts.TotalCount,
        PageSize = posts.PageSize,
        CurrentPage = posts.CurrentPage,
        TotalPages = posts.TotalPages,
        HasNextPage = posts.HasNextPage,
        HasPreviousPage = posts.HasPreviousPage,
        NextPageUrl = _uriService.GetPostPaginationUrl(filters, Url.RouteUrl(nameof(GetPosts))).ToString(),
        PreviousPageUrl = _uriService.GetPostPaginationUrl(filters, Url.RouteUrl(nameof(GetPosts))).ToString()
      };

      var apiResponse = new ApiResponse<IEnumerable<PostDto>>(postsDto)
      {
        Meta = metadata
      };

      Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
      return Ok(apiResponse);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<PostDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPost(int id)
    {
      var post = await _postService.GetPost(id);

      var apiResponse = new ApiResponse<PostDto>(post);
      return Ok(apiResponse);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PostDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post(PostDto postDto)
    {
      var result = await _validator.ValidateAsync(postDto);

      if (!result.IsValid)
      {
        return BadRequest(Results.ValidationProblem(result.ToDictionary()));
      }
      await _postService.InsertPost(postDto);

      var apiResponse = new ApiResponse<PostDto>(postDto);
      return Ok(apiResponse);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Put(int id, PostDto postDto)
    {
      var result = await _validator.ValidateAsync(postDto);

      if (!result.IsValid)
      {
        return BadRequest(Results.ValidationProblem(result.ToDictionary()));
      }

      bool response = await _postService.UpdatePost(postDto, id);

      if (!response)
      {
        return NotFound();
      }

      return Ok(response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
      bool response = await _postService.DeletePost(id);

      if (!response)
      {
        return NotFound();
      }

      return Ok(response);
    }
  }
}
