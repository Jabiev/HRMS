using CareerCompassAPI.Application.DTOs.Company_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface ICompanyService
    {
        Task CreateAsync(CompanyCreateDto companyCreateDto, string userId);
        Task Remove(Guid companyId);
        Task<CompanyGetDto> GetCompanyDetailsById(Guid companyId);
    }
}
