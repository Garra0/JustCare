using JustCare_MB.Dtos;
using JustCare_MB.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Services
{
    public class MedicalHistoryStatusService : IMedicalHistoryStatusService
    {
        public Task<bool> CreateMedicalHistoriesStatusAsync(MedicalHistoryStatusDto medicalHistoryStatus)
        {
            throw new NotImplementedException();
        }

        public Task<MedicalHistoryStatusDto> GetMedicalHistoriesStatusAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateMedicalHistoryStatusAsync(MedicalHistoryStatusDto medicalHistoryStatus)
        {
            throw new NotImplementedException();
        }
    }
}
