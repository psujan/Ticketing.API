using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticketing.API.Model.Domain;
using Ticketing.API.Model;
using Ticketing.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Ticketing.API.Model.Dto;
using System.Net;
using Ticketing.API.Repositories;
using Ticketing.API.Validations;

namespace Ticketing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolutionGuideController : ControllerBase
    {
        private readonly ISolutionGuideRepository solutionGuideRepository;

        public SolutionGuideController(ISolutionGuideRepository solutionGuideRepository)
        {
            this.solutionGuideRepository = solutionGuideRepository;
        }

        [Authorize]
        [HttpGet]
        [Route("paginated")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1 , int  pageSize = 10)
        {
            var data = await solutionGuideRepository.GetPaginatedData(pageNumber, pageSize);
            return Ok(new ApiResponse<PaginatedModel<SolutionGuide>>()
            {
                Success = true,
                Message = "Data Fetched Successfully",
                Data = data
            });
        }

        [Authorize]
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create(SolutionGuideRequestDto request)
        {
            var data = await solutionGuideRepository.Create(request);
            return Ok(new ApiResponse<SolutionGuide>()
            {
                Success = true,
                Message = "Solution Guide Created Successfully",
                Data = data
            });
            /*try
            {
                var data = await solutionGuideRepository.Create(request);
                return Ok(new ApiResponse<SolutionGuide>()
                {
                    Success = true,
                    Message = "Solution Guide Created Successfully",
                    Data = data
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
            }*/
        }



    }
}
