using JustCare_MB.Dtos;

namespace JustCare_MB.Services.IServices
{
    public interface IMedicalHistoryStatusService
    {
        Task<MedicalHistoryStatusDto> GetMedicalHistoriesStatusAsync();
        Task<bool> CreateMedicalHistoriesStatusAsync(MedicalHistoryStatusDto medicalHistoryStatus);
        Task<bool> UpdateMedicalHistoryStatusAsync(MedicalHistoryStatusDto medicalHistoryStatus);
    }
}
