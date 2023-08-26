using AutoMapper;
using MediatR;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.Core.Aplication.Features.Posts.Queries.GetPostById
{
  public class GetPostByIdQuery : IRequest<PostDto>
  {
    public int Id { get; set; }
  }
  public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostDto>
  {
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    public GetPostByIdQueryHandler(IPostRepository postRepository, IMapper mapper)
    {
      _postRepository = postRepository;
      _mapper = mapper;
    }

    public async Task<PostDto> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
      var postDto = await GetPostByIdAsync(request.Id);

      if (postDto == null ) throw new Exception("Post not found");

      return postDto;
    }

    private async Task<PostDto> GetPostByIdAsync(int id)
    {
      var post = await _postRepository.GetByIdAsync(id);
      var postDto = _mapper.Map<PostDto>(post);
      return postDto;
    }

  }


}
