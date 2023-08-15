using CareerCompassAPI.Application.Abstraction.Repositories.ICompanyRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IIndustryRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Company_DTOs;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyWriteRepository _companyWriteRepository;
        private readonly IIndustryReadRepository _industryReadRepository;
        private readonly ICompanyReadRepository _companyReadRepository;
        private readonly CareerCompassDbContext _context;
        public CompanyService(ICompanyWriteRepository companyWriteRepository,
                              IIndustryReadRepository industryReadRepository,
                              ICompanyReadRepository companyReadRepository,
                              CareerCompassDbContext context)
        {
            _companyWriteRepository = companyWriteRepository;
            _industryReadRepository = industryReadRepository;
            _companyReadRepository = companyReadRepository;
            _context = context;
        }
        public async Task CreateAsync(CompanyCreateDto companyCreateDto)
        {
            if (companyCreateDto is null)
            {
                throw new ArgumentNullException();
            }
            var industry = await _industryReadRepository.GetByExpressionAsync(i => i.Id == companyCreateDto.industryId);
            var jobLocation = await _context.JobLocations.FirstOrDefaultAsync(j => j.Id == companyCreateDto.locationId);

            CompanyDetails newCompanyDetails = new()
            {
                Ceo = companyCreateDto.ceoName,
                DateFounded = companyCreateDto.dateFounded,
                Industry = industry,
                CompanySize = companyCreateDto.companySize,
                Location = jobLocation,
                Link = companyCreateDto.websiteLink,
                Description = companyCreateDto.description,
                Address = companyCreateDto.address
            };
            Company newCompany = new()
            {
                Name = companyCreateDto.name,
                Details = newCompanyDetails
            };
            await _companyWriteRepository.AddAsync(newCompany);
            await _companyWriteRepository.SaveChangesAsync();
        }

        public async Task Remove(Guid companyId)
        {
            var company = await _companyReadRepository.GetByIdAsync(companyId);
            if (company is not Company)
            {
                throw new ArgumentNullException();
            }
            _companyWriteRepository.Remove(company);
            await _companyWriteRepository.SaveChangesAsync();
        }
    }
}
