﻿using SocialMedia.Core.Aplication.DTOs.CustomEntities;
using SocialMedia.Core.Aplication.QueryFilters;
using SocialMedia.Core.Domain.Entities;
using SocialMedia.Core.DTOs;

namespace SocialMedia.Core.Aplication.Interfaces.Services
{
    public interface IPostService
    {
        (PagedList<Post>, IEnumerable<PostDto>) GetPosts(PostQueryFilter filters);
        Task<PostDto> GetPost(int id);
        Task<PostDto> InsertPost(PostDto postDto);
        Task<bool> UpdatePost(PostDto postDto, int id);
        Task<bool> DeletePost(int id);
    }
}