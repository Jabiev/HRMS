using CareerCompassAPI.Application.Abstraction.Repositories.ICompanyRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IIndustryRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Company_DTOs;
using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyWriteRepository _companyWriteRepository;
        private readonly IIndustryReadRepository _industryReadRepository;
        public CompanyService(ICompanyWriteRepository companyWriteRepository,
                              IIndustryReadRepository industryReadRepository)
        {
            _companyWriteRepository = companyWriteRepository;
            _industryReadRepository = industryReadRepository;
        }
        public async Task CreateAsync(CompanyCreateDto companyCreateDto)
        {
            if (companyCreateDto is null)
            {
                throw new ArgumentNullException();
            }
            var industry = await _industryReadRepository.GetByExpressionAsync(i => i.Id == companyCreateDto.industryId);
            CompanyDetails newCompanyDetails = new()
            {
                Ceo = companyCreateDto.ceoName,
                DateFounded = companyCreateDto.dateFounded,
                Industry = industry,
                CompanySize = companyCreateDto.companySize,
                Link = companyCreateDto.websiteLink,
                Description = companyCreateDto.description
            };
            Company newCompany = new()
            {
                Name = companyCreateDto.name,
                Details = newCompanyDetails
            };
            await _companyWriteRepository.AddAsync(newCompany);
            await _companyWriteRepository.SaveChangesAsync();
        }
    }
}
