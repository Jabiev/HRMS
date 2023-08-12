using CareerCompassAPI.Application.DTOs.Company_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface ICompanyService
    {
        Task CreateAsync(CompanyCreateDto companyCreateDto);
        Task Remove(Guid companyId);
    }
}
