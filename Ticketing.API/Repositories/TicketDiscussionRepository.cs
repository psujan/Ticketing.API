using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Ticketing.API.Data;
using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto;
using Ticketing.API.Model.Dto.Requuest;
using Ticketing.API.Repositories.Interfaces;
using Ticketing.API.Repositories.Interfaces.Auth;

namespace Ticketing.API.Repositories
{
    public class TicketDiscussionRepository : ITicketDiscussionRepository
    {
        private readonly TicketingDbContext dbContext;

        private readonly ITicketRepository ticketRepository;
        private readonly IUserManagerRepository uRep;
        private readonly IMapper mapper;

        public TicketDiscussionRepository(TicketingDbContext dbContext , 
            ITicketRepository ticketRepository , 
            IUserManagerRepository uRep,
            IMapper mapper

        )
        {
            this.dbContext = dbContext;
            this.ticketRepository = ticketRepository;
            this.uRep = uRep;
            this.mapper = mapper;
        }

        public async Task<bool> TicketExist(int ticketId)
        {
            var ticket = await ticketRepository.GetById(ticketId);
            if (ticket == null)
            {
                throw new Exception("Ticket doesnot exist");
            }
            return true;
        }

        public async Task<User> UserExist(string userName)
        {
            var user = await uRep.GetUserByUserName(userName);
            if(user == null)
            {
                throw new Exception("User nnot found");
            }
            return user;
        }


        public async Task<TicketDiscussionResponseDto> Create(TicketDiscussionRequestDto request)
        {
            await TicketExist(request.TicketId);
            var user = await UserExist(request.UserName);
            TicketDiscussion discussion = new TicketDiscussion()
            {
                Comment = request.Comment,
                DeletedAt = request.DeletedAt,
                TicketId = request.TicketId,
                UserId = user.Id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await dbContext.AddAsync(discussion);
            await dbContext.SaveChangesAsync();
            return mapper.Map<TicketDiscussionResponseDto>(discussion);
        }

        public async Task<IEnumerable<TicketDiscussionResponseDto>> GetAll(int ticketId)
        {
            var status = await TicketExist(ticketId);
            var discussions = await dbContext.Set<TicketDiscussion>().Where(x => x.TicketId == ticketId).ToListAsync();
            return mapper.Map<IEnumerable<TicketDiscussionResponseDto>>(discussions);
        }
    }
}
