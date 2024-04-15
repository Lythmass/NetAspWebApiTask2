﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reddit.Dtos;
using Reddit.Mapper;
using Reddit.Models;
using Reddit.Repositories;
using Reddit.Filters;


namespace Reddit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICommunitiesRepository _communitiesRepository;

        public CommunityController(ApplicationDbContext context, IMapper mapper, ICommunitiesRepository communitiesRepository)
        {
            _context = context;
            _mapper = mapper;
            _communitiesRepository = communitiesRepository;
        }

        [HttpGet]
        [PaginationSizeActionFilter]
        public async Task<ActionResult<PagedList<Community>>> GetCommunities(int pageNumber=1, int pageSize=2, string? search=null, string? sortKey=null, bool? isAscending=true)
        {
            return await _communitiesRepository.GetCommunities(pageNumber, pageSize, search, sortKey, isAscending);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Community>> GetCommunity(int id)
        {
            var community = await _context.Communities.FindAsync(id);

            if(community == null)
            {
                return NotFound();
            }

            return community;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCommunity(CreateCommunityDto communityDto)
        {
                var community = _mapper.toCommunity(communityDto);

                await _context.Communities.AddAsync(community);
                await _context.SaveChangesAsync();
                return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommunity(int id)
        {
            var community = await _context.Communities.FindAsync(id);
            if (community == null)
            {
                return NotFound();
            }

            _context.Communities.Remove(community);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommunity (int id, Community community)
        {
            if (!CommunityExists(id))
            {
                return NotFound();
            }

            _context.Entry(community).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool CommunityExists(int id) => _context.Communities.Any(e => e.Id == id);
    }
}
