using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Ticketing.API.Model;
using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto.Category;
using Ticketing.API.Repositories.Interfaces;
using Ticketing.API.Services;
using Ticketing.API.Validations;

namespace Ticketing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [Authorize]
        [HttpGet]
        [Route("paginated")]
        public async Task<IActionResult> GetPaginatedResult([FromQuery] int pageNumber = 1 , int pageSize = 10)
        {
            var data =  await categoryRepository.GetPaginatedData(pageNumber, pageSize);
            return Ok(new ApiResponse<PaginatedModel<Category>>()
            {
                Success = true,
                Message = "Data Fetched Successfully",
                Data  =  data
            });
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] CategoryRequestDto categoryRequest)
        {
            try
            {
                var category = await categoryRepository.Create(categoryRequest);
                return Ok(new ApiResponse<Category>()
                {
                    Success = true,
                    Message = "Category Created Successfully",
                    Data = category
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

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut]
        [Route("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute]int id , [FromBody] CategoryRequestDto categoryRequest)
        {
            try
            {
                var category = await categoryRepository.Update(id , categoryRequest);
                return Ok(new ApiResponse<Category?>()
                {
                    Success = category != null ? true : false,
                    Message = category != null ? "Category Updated Successfully" :"Category Not Found",
                    Data = category
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

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var category = await categoryRepository.Delete(id);
                return Ok(new ApiResponse<Category?>()
                {
                    Success = category != null ? true : false,
                    Message = category != null ? "Category Deleted Successfully" : "Category Not Found",
                    Data = category
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
