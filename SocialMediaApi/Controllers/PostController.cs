using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;

namespace SocialMediaApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PostController : ControllerBase
  {
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    public PostController(IPostRepository postRepository, IMapper mapper) { 
      _postRepository = postRepository;
      _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
      var posts = await _postRepository.GetPosts();
      var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts);
      var postLength = posts.Count();

      return Ok( new { results = postLength, postsDto });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPost(int id)
    {
      var post = await _postRepository.GetPost(id);
      var postDto = _mapper.Map<PostDto>(post);
      return Ok(postDto);
    }

    [HttpPost]
    public async Task<IActionResult> Post(PostDto post)
    {
      var postEntity = _mapper.Map<Post>(post);
      await _postRepository.InsertPost(postEntity);
      return Ok();
    }

  }
}
