using AutoMapper;
using MediatR;
using SocialMedia.Core.Domain.Entities;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.Core.Aplication.Features.Posts.Commands.CreatePost
{
  public class CreatePostCommand : IRequest<PostDto>
  {
    public int PostId { get; set; }
    public int UserId { get; set; }
    public DateTime? Date { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
  }

  public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostDto>
  {
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePostCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
      _mapper = mapper;
      _unitOfWork = unitOfWork;
    }

    public async Task<PostDto> Handle(CreatePostCommand command, CancellationToken cancellationToken)
    {
      var postDto = new PostDto
      {
        UserId = command.UserId,
        Date = command.Date,
        Description = command.Description,
        Image = command.Image
      };

      var post = await InsertPost(postDto);

      return post;
    }

    private async Task<PostDto> InsertPost(PostDto postDto)
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

  }
}
