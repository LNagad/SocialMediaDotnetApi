using SocialMedia.Core.Aplication.Interfaces;
using SocialMedia.Core.Domain.Entities;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.Core.Aplication.Services
{
    public class SecurityService : ISecurityService
  {
    private readonly IUnitOfWork _unitOfWork;
    public SecurityService(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<Security> GetUser(UserLogin login)
    {
      return await _unitOfWork.SecurityRepository.GetUser(login);
    }

    public async Task RegisterUser(Security security)
    {
      await _unitOfWork.SecurityRepository.AddAsync(security);
      await _unitOfWork.SaveChangesAsync();
    }

  }
}
