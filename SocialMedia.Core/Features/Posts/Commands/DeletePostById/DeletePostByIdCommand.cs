using MediatR;
using SocialMedia.Core.Aplication.Exceptions;
using SocialMedia.Core.Aplication.Wrappers;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Interfaces;
using System.Net;

namespace SocialMedia.Core.Aplication.Features.Posts.Commands.DeletePostById
{
  public class DeletePostByIdCommand : IRequest<Response<PostDto>>
  {
    public int Id { get; set; }
  }

  public class DeletePostByIdCommandHandler : IRequestHandler<DeletePostByIdCommand, Response<PostDto>>
  {
    private readonly IUnitOfWork _unitOfWork;

    public DeletePostByIdCommandHandler(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }


    public async Task<Response<PostDto>> Handle(DeletePostByIdCommand request, CancellationToken cancellationToken)
    {
      var postId = await DeletePost(request.Id);
      var postDto = new PostDto
      {
        PostId = postId
      };
      return new Response<PostDto>(){ Data = postDto };
    }

    private async Task<int> DeletePost(int id)
    {
      var existingPost = await _unitOfWork.PostRepository.GetByIdAsync(id);

      if (existingPost == null) throw new ApiException("Post doesn't exist", (int)HttpStatusCode.NotFound);
       
      _unitOfWork.PostRepository.Delete(existingPost);

      await _unitOfWork.SaveChangesAsync();

      return existingPost.Id;
    }

  }
}
