using MediatR;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.Core.Aplication.Features.Posts.Commands.DeletePostById
{
  public class DeletePostByIdCommand : IRequest<bool>
  {
    public int Id { get; set; }
  }

  public class DeletePostByIdCommandHandler : IRequestHandler<DeletePostByIdCommand, bool>
  {
    private readonly IUnitOfWork _unitOfWork;

    public DeletePostByIdCommandHandler(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }


    public async Task<bool> Handle(DeletePostByIdCommand request, CancellationToken cancellationToken)
    {
      return await DeletePost(request.Id);
    }

    private async Task<bool> DeletePost(int id)
    {
      var existingPost = await _unitOfWork.PostRepository.GetByIdAsync(id);

      if (existingPost == null) throw new Exception("Post doesn't exist");
       
      _unitOfWork.PostRepository.Delete(existingPost);

      await _unitOfWork.SaveChangesAsync();

      return true;
    }

  }
}
