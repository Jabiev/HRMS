using AutoMapper;
using CareerCompassAPI.Application.Abstraction.Repositories.ICompanyRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IIndustryRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IRecruiterRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Company_DTOs;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Domain.Enums;
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
        private readonly IRecruiterWriteRepository _recruiterWriteRepository;
        private readonly IMapper _mapper;
        public CompanyService(ICompanyWriteRepository companyWriteRepository,
                              IIndustryReadRepository industryReadRepository,
                              ICompanyReadRepository companyReadRepository,
                              CareerCompassDbContext context,
                              IRecruiterWriteRepository recruiterWriteRepository,
                              IMapper mapper)
        {
            _companyWriteRepository = companyWriteRepository;
            _industryReadRepository = industryReadRepository;
            _companyReadRepository = companyReadRepository;
            _context = context;
            _mapper = mapper;
            _recruiterWriteRepository = recruiterWriteRepository;
        }
        public async Task CreateAsync(CompanyCreateDto companyCreateDto, string userId)
        {
            if (companyCreateDto is null)
            {
                throw new ArgumentNullException();
            }
            var industry = await _industryReadRepository.GetByExpressionAsync(i => i.Id == companyCreateDto.industryId);
            var jobLocation = await _context.JobLocations.FirstOrDefaultAsync(j => j.Id == companyCreateDto.locationId);
            var recruiter = await _context.Recruiters.FirstOrDefaultAsync(r => r.AppUserId == userId);

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

            recruiter.CompanyId = newCompany.Id;
            await _recruiterWriteRepository.SaveChangesAsync();
        }

        public async Task<List<CompanyDetailsGetDto>> GetCompanyBySearchAsync(string companyName)
        {
            IQueryable<Company> query = _context.Companies
                .Include(c => c.Details)
                    .ThenInclude(d => d.Location)
                .Include(c => c.Details)
                    .ThenInclude(d => d.Industry)
                .Include(c => c.Reviews)
                .Where(c => c.IsDeleted == false);


            if (!string.IsNullOrEmpty(companyName))
            {
                query = query.Where(c => c.Name.ToLower().Contains(companyName.ToLower()));
            }

            var companies = await query.ToListAsync();

            return companies.Select(c => new CompanyDetailsGetDto(
                 c.Name,
                 c.Details.Description,
                 c.Details.Ceo,
                 c.Details.DateFounded,
                 Enum.GetName(typeof(CompanySizeEnum), c.Details.CompanySize),
                 c.Details.Industry.Name,
                 c.Details.Link,
                 c.Details.Location.Location
                 )).ToList();
        }


        public async Task<CompanyGetDto> GetCompanyDetailsById(Guid companyId)
        {
            var company = await _context.Companies
                 .Include(c => c.Details)
                 .ThenInclude(d => d.Industry)
                 .FirstOrDefaultAsync(c => c.Id == companyId);
            if (company is not Company)
            {
                throw new ArgumentNullException();
            }
            var details = company.Details;
            var industryName = details.Industry.Name;
            CompanyGetDto companyGetDto = new(
                company.Id,
                company.Name, details.Ceo, details.DateFounded, details.CompanySize, industryName, details.Link, details.Description, details.Address,company.LogoUrl
                );
            return companyGetDto;
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

        public async Task UploadLogoAsync(Guid companyId, CompanyLogoUploadDto logoUploadDto)
        {
            if (logoUploadDto is null)
            {
                throw new ArgumentNullException();
            }
            Company company = await _companyReadRepository.GetByIdAsync(companyId);
            // company not found Exception
            company.LogoUrl = logoUploadDto.url;
            _companyWriteRepository.Update(company);
            await _companyWriteRepository.SaveChangesAsync();
        }
    }
}
