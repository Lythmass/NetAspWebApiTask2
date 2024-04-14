using Reddit.Models;

namespace Reddit.Repositories
{
    public class CommunitiesRepository : ICommunitiesRepository
    {
        private readonly ApplicationDbContext _context;

        public CommunitiesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedList<Community>> GetCommunities(int pageNumber, int pageSize, string? search)
        {
            var communities = _context.Communities.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                communities = communities.Where(community => community.Name.Contains(search) || community.Description.Contains(search));
            }

            return await PagedList<Community>.CreateAsync(communities, pageNumber, pageSize);
        }
    }
}
