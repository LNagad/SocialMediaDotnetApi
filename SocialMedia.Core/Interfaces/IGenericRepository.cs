﻿using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Interfaces
{
  public interface IGenericRepository<Entity> where Entity : BaseEntity
  {
    IAsyncEnumerable<Entity> GetAll();
    Task<Entity> GetByIdAsync(int id);
    Task AddAsync(Entity entity);
    void Update(Entity entity);
    Task DeleteAsync(int id);
  }
}