using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAdvisor.Application.DTOs.Common;
using MyAdvisor.Application.DTOs.FinancialDiary;
using MyAdvisor.Application.Interfaces.Services.Domain;

namespace MyAdvisor.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class FinancialDiaryController : BaseController
    {
        private readonly IFinancialDiaryService _diaryService;

        public FinancialDiaryController(IFinancialDiaryService diaryService)
        {
            _diaryService = diaryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = ResolveUserId();
            if (userId is null) return Unauthorized();

            var diaries = await _diaryService.GetAllAsync(userId.Value);
            return Ok(diaries);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = ResolveUserId();
            if (userId is null) return Unauthorized();

            var diary = await _diaryService.GetByIdAsync(id);
            if (diary is null) return NotFound(new ErrorResponse($"Diary {id} not found."));
            if (diary.UserId != userId.Value) return Forbid();

            return Ok(diary);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFinancialDiaryRequestDto request)
        {
            var userId = ResolveUserId();
            if (userId is null) return Unauthorized();

            var diary = await _diaryService.CreateAsync(request, userId.Value);
            return StatusCode(StatusCodes.Status201Created, diary);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateFinancialDiaryRequestDto request)
        {
            var userId = ResolveUserId();
            if (userId is null) return Unauthorized();

            var existing = await _diaryService.GetByIdAsync(id);
            if (existing is null) return NotFound(new ErrorResponse($"Diary {id} not found."));
            if (existing.UserId != userId.Value) return Forbid();

            try
            {
                var diary = await _diaryService.UpdateAsync(id, request);
                return Ok(diary);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse(ex.Message));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = ResolveUserId();
            if (userId is null) return Unauthorized();

            var existing = await _diaryService.GetByIdAsync(id);
            if (existing is null) return NotFound(new ErrorResponse($"Diary {id} not found."));
            if (existing.UserId != userId.Value) return Forbid();

            await _diaryService.DeleteAsync(id);
            return NoContent();
        }
    }
}
