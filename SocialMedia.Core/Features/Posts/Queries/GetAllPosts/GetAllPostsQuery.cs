using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using SocialMedia.Core.Aplication.CustomEntities;
using SocialMedia.Core.Domain.Entities;
using SocialMedia.Core.Domain.Settings;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.Core.Aplication.Features.Posts.Queries.GetAllPosts
{
  /// <summary>
  /// Paraments to filter the posts
  /// </summary>
  public class GetAllPostsQuery : IRequest<(IEnumerable<PostDto>, PagedList<Post>)>
  {
    public GetAllPostParameters? Parameters { get; set; }
  }

  public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, (IEnumerable<PostDto>, PagedList<Post>)>
  {
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;
    private readonly PaginationSettings _paginationSettings;

    public GetAllPostsQueryHandler(IPostRepository postRepository, IMapper mapper, IOptions<PaginationSettings> options)
    {
      _postRepository = postRepository;
      _mapper = mapper;
      _paginationSettings = options.Value;
    }

    public async Task<(IEnumerable<PostDto>, PagedList<Post>)> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
      var pagedPosts = GetPosts(request.Parameters);

      if (pagedPosts == null || pagedPosts.Count == 0) throw new Exception("Posts not found");

      var postsDto = _mapper.Map<IEnumerable<PostDto>>(pagedPosts);

      return (postsDto, pagedPosts);
    }

    #region private methods

    private PagedList<Post> GetPosts(GetAllPostParameters parameters)
    {
      parameters.PageNumber = parameters.PageNumber == 0 ? _paginationSettings.DefaultPageNumber : parameters.PageNumber;
      parameters.PageSize = parameters.PageSize == 0 ? _paginationSettings.DefaultPageSize : parameters.PageSize;

      var posts = _postRepository.GetAll();

      if (parameters.UserId != null)
      {
        posts = posts.Where(x => x.UserId == parameters.UserId);
      }

      if (parameters.Date != null)
      {
        //datetime usa horay minutos, por lo que si se quiere filtrar por fecha, se debe usar ToShortDateString()
        posts = posts.Where(x => x.Date.ToShortDateString() == parameters.Date?.ToShortDateString());
      }

      if (parameters.Description != null)
      {
        posts = posts.Where(x => x.Description.ToLower().Contains(parameters.Description.ToLower()));
      }

      var pagedPosts = PagedList<Post>.Create(posts, parameters.PageNumber, parameters.PageSize);

      return pagedPosts;
    }

    #endregion
  }
}
