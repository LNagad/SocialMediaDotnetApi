using AutoMapper;
using Microsoft.Extensions.Options;
using SocialMedia.Core.Aplication.CustomEntities;
using SocialMedia.Core.Aplication.Interfaces.Services;
using SocialMedia.Core.Aplication.QueryFilters;
using SocialMedia.Core.Domain.Entities;
using SocialMedia.Core.Domain.Settings;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.Core.Services
{
    public class PostService : IPostService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly PaginationSettings _paginationSettings;

    public PostService(IUnitOfWork unitOfWork, IOptions<PaginationSettings> options, IMapper mapper)
    {
      _unitOfWork = unitOfWork;
      _paginationSettings = options.Value;
      _mapper = mapper;
    }

    public (PagedList<Post>, IEnumerable<PostDto>) GetPosts(PostQueryFilter filters)
    {
      filters.PageNumber = filters.PageNumber == 0 ? _paginationSettings.DefaultPageNumber : filters.PageNumber;
      filters.PageSize = filters.PageSize == 0 ? _paginationSettings.DefaultPageSize : filters.PageSize;

      var posts = _unitOfWork.PostRepository.GetAll();

      if (filters.UserId != null)
      {
        posts = posts.Where(x => x.UserId == filters.UserId);
      }

      if (filters.Date != null)
      {
        //datetime usa horay minutos, por lo que si se quiere filtrar por fecha, se debe usar ToShortDateString()
        posts = posts.Where(x => x.Date.ToShortDateString() == filters.Date?.ToShortDateString());
      }

      if (filters.Description != null)
      {
        posts = posts.Where(x => x.Description.ToLower().Contains(filters.Description.ToLower()));
      }

      var pagedPosts = PagedList<Post>.Create(posts, filters.PageNumber, filters.PageSize);
      var postsDto = _mapper.Map<IEnumerable<PostDto>>(pagedPosts);
      return (pagedPosts, postsDto);
    }

    public async Task<PostDto> GetPost(int id)
    {
      var post = await _unitOfWork.PostRepository.GetByIdAsync(id);
      var postDto = _mapper.Map<PostDto>(post);
      return postDto;
    }

    public async Task<PostDto> InsertPost(PostDto postDto)
    {
      var post = _mapper.Map<Post>(postDto);

      var user = await _unitOfWork.UserRepository.GetByIdAsync(post.UserId);

      if (user == null)
      {
        throw new BusinessException("User doesn't exist");
      }

      if (post.Description.Contains("Sexo"))
      {
        throw new BusinessException("Content not allowed");
      }

      var userPosts = await _unitOfWork.PostRepository.GetPostsByUser(post.UserId);

      if (userPosts.Count() < 10)
      {
        var lastPost = userPosts.OrderByDescending(x => x.Date).FirstOrDefault();

        // han transcurrido menos de 7 días desde la fecha de la última publicación ?
        if ((DateTime.Now - lastPost.Date).TotalDays < 7)
        {
          throw new BusinessException("You are not able to publish the post");
        }
      }

      await _unitOfWork.PostRepository.AddAsync(post);
      await _unitOfWork.SaveChangesAsync();

      postDto = _mapper.Map<PostDto>(post);
      return postDto;
    }

    public async Task<bool> UpdatePost(PostDto postDto, int id)
    {
      var entity = _mapper.Map<Post>(postDto);
      var existingPost = await _unitOfWork.PostRepository.GetByIdAsync(id);

      existingPost.Image = entity.Image;
      existingPost.Description = entity.Description;

      _unitOfWork.PostRepository.Update(existingPost);

      await _unitOfWork.SaveChangesAsync();

      return true;
    }

    public async Task<bool> DeletePost(int id)
    {
      await _unitOfWork.PostRepository.DeleteAsync(id);

      await _unitOfWork.SaveChangesAsync();

      return true;
    }
  }
}
