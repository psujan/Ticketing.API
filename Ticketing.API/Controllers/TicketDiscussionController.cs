using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticketing.API.Model.Domain;
using Ticketing.API.Model;
using Ticketing.API.Repositories.Interfaces;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Ticketing.API.Model.Dto.Requuest;
using Ticketing.API.Model.Dto;

namespace Ticketing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketDiscussionController : ControllerBase
    {
        private readonly ITicketDiscussionRepository discussionRepository;

        public TicketDiscussionController(ITicketDiscussionRepository discussionRepository)
        {
            this.discussionRepository = discussionRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment([FromBody]TicketDiscussionRequestDto request)
        {
           
            try
            {
                var discussion = await discussionRepository.Create(request);
                return Ok(new ApiResponse<TicketDiscussionResponseDto?>()
                {
                    Success = true,
                    Message = "Comment Added Successfully",
                    Data = discussion
                });
            }
            catch (Exception ex) 
            {
                return new ObjectResult(new ApiResponse<string>()
                {
                    Success = false,
                    Message = ex.Message,
                    Data = ""
                })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> GetAll([FromRoute] int id)
        {

            try
            {
                var discussion = await discussionRepository.GetAll(id);
                return Ok(new ApiResponse<IEnumerable<TicketDiscussionResponseDto>>()
                {
                    Success = true,
                    Message = "Comments Fetched Successfully",
                    Data = discussion
                });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ApiResponse<string>()
                {
                    Success = false,
                    Message = ex.Message,
                    Data = ""
                })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

        }
    }
}
