using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAdvisor.Application.DTOs.AI;
using MyAdvisor.Application.DTOs.Common;
using MyAdvisor.Application.Interfaces.Services.AI;
using MyAdvisor.Application.Interfaces.Services.App;

namespace MyAdvisor.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ChatController : BaseController
    {
        private readonly IFinancialChatService _chatService;
        private readonly IGeminiService _gemini;

        public ChatController(IFinancialChatService chatService, IGeminiService gemini)
        {
            _chatService = chatService;
            _gemini = gemini;
        }

        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] ChatRequestDto request)
        {
            var userId = ResolveUserId();
            if (userId is null) return Unauthorized();

            try
            {
                var response = await _chatService.ChatAsync(userId.Value, request);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(502, new ErrorResponse($"AI service error: {ex.Message}"));
            }
        }

        [HttpPost("summarize-image")]
        [RequestSizeLimit(20 * 1024 * 1024)]
        public async Task<IActionResult> SummarizeImage(IFormFile image)
        {
            var userId = ResolveUserId();
            if (userId is null) return Unauthorized();

            if (image is null || image.Length == 0)
                return BadRequest(new ErrorResponse("No image provided."));

            try
            {
                using var ms = new MemoryStream();
                await image.CopyToAsync(ms);

                var prompt = """
                    You are a financial analyst reviewing a receipt, bank statement, or financial document.
                    Provide a clear, structured summary covering:
                    1. What type of document this is
                    2. Key amounts and what they represent
                    3. Merchant/vendor details if visible
                    4. Date and payment method if visible
                    5. Any notable observations or insights about the spending

                    Be concise and practical. Format your response clearly.
                    """;

                var reply = await _gemini.AnalyzeImageAsync(ms.ToArray(), image.ContentType, prompt);
                return Ok(new ChatResponseDto(reply));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(502, new ErrorResponse($"AI service error: {ex.Message}"));
            }
        }
    }
}
