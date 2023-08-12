using CareerCompassAPI.Application.Abstraction.Repositories.ICompanyRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Company_DTOs;
using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyWriteRepository _companyWriteRepository;
        public CompanyService(ICompanyWriteRepository companyWriteRepository)
        {
            _companyWriteRepository = companyWriteRepository;
        }
        public async Task CreateAsync(CompanyCreateDto companyCreateDto)
        {
            if (companyCreateDto is null)
            {
                throw new ArgumentNullException();
            }
            Company newCompany = new()
            {
                Name = companyCreateDto.name
            };
            await _companyWriteRepository.AddAsync(newCompany);
            await _companyWriteRepository.SaveChangesAsync();   
        }
    }
}
