using Reddit.Models;
using System.Linq.Expressions;

namespace Reddit.Repositories
{
    public class CommunitiesRepository : ICommunitiesRepository
    {
        private readonly ApplicationDbContext _context;

        public CommunitiesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedList<Community>> GetCommunities(int pageNumber, int pageSize, string? search, string? sortKey, bool? isAscending)
        {
            var communities = _context.Communities.AsQueryable();
            if (isAscending == false)
            {
                communities = communities.OrderByDescending(GetSortExpression(sortKey));
            } else
            {
                communities = communities.OrderBy(GetSortExpression(sortKey));
            }

            if (!string.IsNullOrEmpty(search))
            {
                communities = communities.Where(community => community.Name.Contains(search) || community.Description.Contains(search));
            }

            return await PagedList<Community>.CreateAsync(communities, pageNumber, pageSize);
        }

        public Expression<Func<Community, object>> GetSortExpression(string? sortKey)
        {
            sortKey = sortKey?.ToLower();
            return sortKey switch
            {
                "date" => community => (community.CreateAt),
                "postsCount" => community => (community.Posts.Count()),
                "subscribersCount" => community => (community.Subscribers.Count()),
                _ => community => community.Id
            };
        }
    }
}
