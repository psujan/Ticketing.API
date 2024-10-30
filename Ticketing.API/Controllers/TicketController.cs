using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticketing.API.Model.Domain;
using Ticketing.API.Model;
using Ticketing.API.Model.Dto;
using Ticketing.API.Repositories;
using Ticketing.API.Repositories.Interfaces;
using System.Net;
using Ticketing.API.Validations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ticketing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository ticketRepository;

        public TicketController(ITicketRepository ticketRepository)
        {
            this.ticketRepository = ticketRepository;
        }

        [Authorize]
        [HttpGet]
        [Route("paginated")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, int pageSize = 10)
        {
            var data = await ticketRepository.GetPaginatedData(pageNumber , pageSize);
            return Ok(new ApiResponse<PaginatedModel<Ticket>>()
            {
                Success = true,
                Message = "Data Fetched Successfully",
                Data = data
            });
        }

       

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var data = await ticketRepository.GetById(id);
            return Ok(new ApiResponse<Ticket?>()
            {
                Success = true,
                Message = "Data Fetched Successfully",
                Data = data
            });
        }

        [HttpPost]
        [Authorize]
        [ValidateModel]
        [Consumes("multipart/form-data")]
        public async  Task<IActionResult> Create([FromForm]TicketRequestDto ticketRequest)
        {
            try
            {
                var ticket = await ticketRepository.Create(ticketRequest);
                return Ok(new ApiResponse<Ticket>()
                {
                    Success = true,
                    Message = "Ticket Created Successfully",
                    Data = ticket
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

        [HttpPut]
        [Authorize]
        [ValidateModel]
        [Route("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromRoute]int id , [FromForm] TicketRequestDto ticketRequest)
        {
            try
            {
                var ticket = await ticketRepository.Update(id, ticketRequest);
                return Ok(new ApiResponse<Ticket>()
                {
                    Success = ticket != null ? true : false,
                    Message = ticket != null ? "Ticket Updated Successfully" : "Ticket Not Found",
                    Data = ticket
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

        [HttpDelete]
        [Authorize(Roles = "SuperAdmin")]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var ticket = await ticketRepository.Delete(id);
                return Ok(new ApiResponse<Ticket>()
                {
                    Success = ticket != null ? true : false,
                    Message = ticket != null ? "Ticket Deleted Successfully" : "Ticket Not Found",
                    Data = ticket
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
