using AutoMapper;
using MediatR;
using SocialMedia.Core.Aplication.Exceptions;
using SocialMedia.Core.Aplication.Wrappers;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Interfaces;
using System.Net;

namespace SocialMedia.Core.Aplication.Features.Posts.Queries.GetPostById
{
  public class GetPostByIdQuery : IRequest<Response<PostDto>>
  {
    public int Id { get; set; }
  }
  public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, Response<PostDto>>
  {
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    public GetPostByIdQueryHandler(IPostRepository postRepository, IMapper mapper)
    {
      _postRepository = postRepository;
      _mapper = mapper;
    }

    public async Task<Response<PostDto>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
      var postDto = await GetPostByIdAsync(request.Id);


      return new Response<PostDto>() { Data = postDto };
    }

    private async Task<PostDto> GetPostByIdAsync(int id)
    {
      var post = await _postRepository.GetByIdAsync(id);

      if (post == null) throw new ApiException("Post not foundt", (int)HttpStatusCode.NotFound);

      var postDto = _mapper.Map<PostDto>(post);
      return postDto;
    }

  }


}
