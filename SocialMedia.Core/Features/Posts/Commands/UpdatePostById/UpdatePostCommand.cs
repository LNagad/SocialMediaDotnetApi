using AutoMapper;
using MediatR;
using SocialMedia.Core.Domain.Entities;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.Core.Aplication.Features.Posts.Commands.UpdatePostById
{
  public class UpdatePostCommand : IRequest<PostUpdateResponse>
  {
    public int PostId { get; set; }
    public int UserId { get; set; }
    public DateTime? Date { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
  }

  public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, PostUpdateResponse>
  {
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePostCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
      _mapper = mapper;
      _unitOfWork = unitOfWork;
    }


    public async Task<PostUpdateResponse> Handle(UpdatePostCommand command, CancellationToken cancellationToken)
    {
      var entity = _mapper.Map<Post>(command);

      var postUpdated = await UpdatePost(entity, command.PostId);
      
      var response = new PostUpdateResponse()
      {
        Date = postUpdated.Date,
        Description = postUpdated.Description,
        Image = postUpdated.Image,
        PostId = postUpdated.PostId,
        UserId = postUpdated.UserId

      };

      return response;
    }

    private async Task<PostDto> UpdatePost(Post post, int id)
    {
      var existingPost = await _unitOfWork.PostRepository.GetByIdAsync(id);

      if (existingPost == null) throw new Exception("Post doesn't exist");

      existingPost.Image = post.Image;
      existingPost.Description = post.Description;

      var postUpdated = _unitOfWork.PostRepository.Update(existingPost);

      await _unitOfWork.SaveChangesAsync();

      var postUpdatedMaped = _mapper.Map<PostDto>(postUpdated);

      return postUpdatedMaped;
    }
  }
}
