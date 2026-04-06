using MyAdvisor.Application.DTOs.FinancialDiary;
using MyAdvisor.Application.Interfaces.Repositories;
using MyAdvisor.Application.Interfaces.Services.Domain;
using MyAdvisor.Application.Mappers;
using MyAdvisor.Domain.Entities;

namespace MyAdvisor.Application.Services
{
    public class FinancialDiaryService : IFinancialDiaryService
    {
        private readonly IFinancialDiaryRepository _diaryRepository;
        private readonly FinancialDiaryMapper _mapper;

        public FinancialDiaryService(
            IFinancialDiaryRepository diaryRepository,
            FinancialDiaryMapper mapper)
        {
            _diaryRepository = diaryRepository;
            _mapper = mapper;
        }

        public async Task<FinancialDiaryDto?> GetByIdAsync(int id)
        {
            var diary = await _diaryRepository.GetByIdWithTransactionsAsync(id);
            return diary is null ? null : _mapper.ToDto(diary);
        }

        public async Task<IReadOnlyList<FinancialDiarySummaryDto>> GetAllAsync(int userId)
        {
            var diaries = await _diaryRepository.GetByUserIdAsync(userId);
            return diaries.Select(_mapper.ToSummaryDto).ToList();
        }

        public async Task<FinancialDiaryDto> CreateAsync(CreateFinancialDiaryRequestDto request, int userId)
        {
            var diary = new FinancialDiary(userId, request.Date, request.Notes);
            await _diaryRepository.AddAsync(diary);
            return _mapper.ToDto(diary);
        }

        public async Task<FinancialDiaryDto> UpdateAsync(int id, UpdateFinancialDiaryRequestDto request)
        {
            var diary = await _diaryRepository.GetByIdWithTransactionsAsync(id)
                ?? throw new KeyNotFoundException($"Diary {id} not found.");

            diary.UpdateNotes(request.Notes);
            await _diaryRepository.UpdateAsync(diary);
            return _mapper.ToDto(diary);
        }

        public async Task UpdateTotalAsync(int id, decimal total)
        {
            var diary = await _diaryRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Diary {id} not found.");

            diary.UpdateTotalAmount(total);
            await _diaryRepository.UpdateAsync(diary);
        }

        public async Task DeleteAsync(int id)
        {
            await _diaryRepository.DeleteAsync(id);
        }
    }
}
