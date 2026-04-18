using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAdvisor.Application.DTOs.Common;
using MyAdvisor.Application.Interfaces.Services.App;

namespace MyAdvisor.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AiTransactionController : BaseController
    {
        private readonly IAiTransactionImportService _aiImportService;

        public AiTransactionController(IAiTransactionImportService aiImportService)
        {
            _aiImportService = aiImportService;
        }

        [HttpPost("import")]
        [RequestSizeLimit(20 * 1024 * 1024)]
        public async Task<IActionResult> ImportFromImage([FromQuery] int diaryId, IFormFile image)
        {
            var userId = ResolveUserId();
            if (userId is null) return Unauthorized();

            if (image is null || image.Length == 0)
                return BadRequest(new ErrorResponse("No image file was provided."));

            var allowed = new[] { "image/jpeg", "image/png", "image/webp", "image/gif" };
            if (!allowed.Contains(image.ContentType.ToLowerInvariant()))
                return BadRequest(new ErrorResponse("Unsupported image type. Please upload a JPEG, PNG, WebP, or GIF."));

            try
            {
                using var ms = new MemoryStream();
                await image.CopyToAsync(ms);

                var result = await _aiImportService.ImportFromImageAsync(
                    diaryId: diaryId,
                    userId: userId.Value,
                    imageData: ms.ToArray(),
                    mimeType: image.ContentType);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse(ex.Message));
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status502BadGateway, new ErrorResponse($"AI service error: {ex.Message}"));
            }
        }
    }
}
