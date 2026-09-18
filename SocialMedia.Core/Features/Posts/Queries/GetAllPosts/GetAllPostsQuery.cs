using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using SocialMedia.Core.Aplication.DTOs.CustomEntities;
using SocialMedia.Core.Aplication.Exceptions;
using SocialMedia.Core.Domain.Entities;
using SocialMedia.Core.Domain.Settings;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Interfaces;
using System.Net;

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
      var pagedPosts = await GetPostsAsync(request.Parameters);

      var hasFilters = request.Parameters.UserId != null
        || request.Parameters.Date != null
        || request.Parameters.Description != null;

      if (hasFilters && pagedPosts.Count == 0)
      {
        throw new ApiException("No posts found with the specified filters", (int)HttpStatusCode.NotFound);
      }

      var postsDto = _mapper.Map<IEnumerable<PostDto>>(pagedPosts);

      return (postsDto, pagedPosts);
    }

    #region private methods

    private async Task<PagedList<Post>> GetPostsAsync(GetAllPostParameters parameters)
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
        posts = posts.Where(x => x.Date.Date == parameters.Date.Value.Date);
      }

      if (parameters.Description != null)
      {
        posts = posts.Where(x => x.Description.ToLower().Contains(parameters.Description.ToLower()));
      }

      var pagedPosts = await PagedList<Post>.CreateAsync(posts, parameters.PageNumber, parameters.PageSize);

      return pagedPosts;
    }

    #endregion
  }
}
