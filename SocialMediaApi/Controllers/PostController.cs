using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia.Core.Aplication.CustomEntities;
using SocialMedia.Core.Aplication.QueryFilters;
using SocialMedia.Core.Domain.Entities;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Interfaces;
using SocialMediaApi.Responses;
using System.Net;

namespace SocialMediaApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PostController : ControllerBase
  {
    private readonly IPostService _postService;
    private readonly IMapper _mapper;
    private readonly IValidator<PostDto> _validator;

    public PostController(IPostService postService, IMapper mapper, IValidator<PostDto> validator) {
      _postService = postService;
      _mapper = mapper;
      _validator = validator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public IActionResult GetPostsByFilters([FromQuery] PostQueryFilter filters)
    {
      var posts = _postService.GetPosts(filters);
      var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts);
      var apiResponse = new ApiResponse<IEnumerable<PostDto>>(postsDto);

      var metadata = new
      {
        posts.TotalCount,
        posts.PageSize,
        posts.TotalPages,
        posts.CurrentPage,
        posts.HasNextPage,
        posts.HasPreviousPage,
      };

      Response.Headers.Add( "X-Pagination", JsonConvert.SerializeObject(metadata) );
      return Ok(apiResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPost(int id)
    {
      var post = await _postService.GetPost(id);
      var postDto = _mapper.Map<PostDto>(post);
      var apiResponse = new ApiResponse<PostDto>(postDto);
      return Ok(apiResponse);
    }

    [HttpPost]
    public async Task<IActionResult> Post(PostDto postDto)
    {
      var result = await _validator.ValidateAsync(postDto);

      if (!result.IsValid)
      {
        return BadRequest(Results.ValidationProblem(result.ToDictionary()));
      }

      var postEntity = _mapper.Map<Post>(postDto);

      await _postService.InsertPost(postEntity);

      postDto = _mapper.Map<PostDto>(postEntity);

      var apiResponse = new ApiResponse<PostDto>(postDto);
      return Ok(apiResponse);
    }


    [HttpPut]
    public async Task<IActionResult> Put(int id, PostDto postDto)
    {
      var result = await _validator.ValidateAsync(postDto);

      if (!result.IsValid)
      {
        return BadRequest(Results.ValidationProblem(result.ToDictionary()));
      }

      var postEntity = _mapper.Map<Post>(postDto);
      postEntity.Id = id;

      bool response = await _postService.UpdatePost(postEntity);

      if (!response)
      {
        return NotFound();
      }

      var apiResponse = new ApiResponse<bool>(response);
      return Ok(apiResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      bool response = await _postService.DeletePost(id);

      if (!response)
      {
        return NotFound();
      }

      var apiResponse = new ApiResponse<bool>(response);
      return Ok(apiResponse);
    }

  }
}
