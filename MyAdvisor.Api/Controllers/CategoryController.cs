using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAdvisor.Application.Interfaces.Services.Domain;

namespace MyAdvisor.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }
    }
}
