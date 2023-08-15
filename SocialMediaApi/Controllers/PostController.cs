using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMediaApi.Responses;

namespace SocialMediaApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PostController : ControllerBase
  {
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<PostDto> _validator;

    public PostController(IPostRepository postRepository, IMapper mapper, IValidator<PostDto> validator) { 
      _postRepository = postRepository;
      _mapper = mapper;
      _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
      var posts = await _postRepository.GetPosts();

      var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts);
      var apiResponse = new ApiResponse<IEnumerable<PostDto>>(postsDto);
      return Ok(apiResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPost(int id)
    {
      var post = await _postRepository.GetPost(id);
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

      await _postRepository.InsertPost(postEntity);

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
      postEntity.PostId = id;

      bool response = await _postRepository.UpdatePost(postEntity);

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
      bool response = await _postRepository.DeletePost(id);

      if (!response)
      {
        return NotFound();
      }

      var apiResponse = new ApiResponse<bool>(response);
      return Ok(apiResponse);
    }

  }
}
