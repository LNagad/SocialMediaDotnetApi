using Swashbuckle.AspNetCore.Annotations;

namespace SocialMedia.Core.Aplication.Features.Posts.Queries.GetAllPosts
{
    public class GetAllPostParameters
    {
        /// <example>
        /// 2
        /// </example>
        [SwaggerParameter("Filter the Post by user", Required = false)]
        public int? UserId { get; set; }

        /// <example>
        /// 12-05-2002
        /// </example>
        [SwaggerParameter("Filter the Post by date", Required = false)]
        public DateTime? Date { get; set; }

        [SwaggerParameter("Filter the Post by description", Required = false)]
        public string? Description { get; set; }
      
        /// <example>
        /// 5
        /// </example>
        [SwaggerParameter("Filter the amount of posts by setting the posts result number", Required = false)]
        public int PageSize { get; set; }

        /// <example>
        /// 1
        /// </example>
        [SwaggerParameter("Filter the page number of the posts", Required = false)]
        public int PageNumber { get; set; }
    }
}
