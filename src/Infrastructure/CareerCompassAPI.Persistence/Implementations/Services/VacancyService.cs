    using AutoMapper;
using CareerCompassAPI.Application.Abstraction.Repositories.ICompanyRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IRecruiterRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IVacancyRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Vacancy_DTOs;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class VacancyService : IVacancyService
    {
        private readonly CareerCompassDbContext _context;
        private readonly IRecruiterReadRepository _recruiterReadRepository;
        private readonly ICompanyReadRepository _companyReadRepository;
        private readonly IVacancyWriteRepository _vacancyWriteRepository;
        private readonly IVacancyReadRepository _vacancyReadRepository;
        private readonly IRecruiterWriteRepository _recruiterWriteRepository;
        private readonly IMapper _mapper;

        public VacancyService(CareerCompassDbContext context,
                              IRecruiterReadRepository recruiterReadRepository,
                              ICompanyReadRepository companyReadRepository,
                              IVacancyWriteRepository vacancyWriteRepository,
                              IMapper mapper,
                              IVacancyReadRepository vacancyReadRepository,
                              IRecruiterWriteRepository recruiterWriteRepository)
        {
            _context = context;
            _recruiterReadRepository = recruiterReadRepository;
            _companyReadRepository = companyReadRepository;
            _vacancyWriteRepository = vacancyWriteRepository;
            _mapper = mapper;
            _vacancyReadRepository = vacancyReadRepository;
            _recruiterWriteRepository = recruiterWriteRepository;
        }

        public async Task Create(VacancyCreateDto vacancyCreateDto, string userId, Guid companyId)
        {
            if (vacancyCreateDto is null)
            {
                throw new ArgumentNullException();
            }
            Guid recruiterAppUserId = Guid.Parse(userId);
            var recruiter = await _recruiterReadRepository.GetByUserIdAsync(recruiterAppUserId);
            var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.Id == recruiter.Subscription.Id);
            if (recruiter.CurrentPostCount >= subscription.PostLimit)
            {
                throw new InvalidOperationException("Post limit exceeded");
            }

            var company = await _companyReadRepository.GetByIdAsync(companyId);
            var experience = await _context.ExperienceLevels.FirstOrDefaultAsync(e => e.Id == vacancyCreateDto.experienceLevelId);
            var jobLocation = await _context.JobLocations.FirstOrDefaultAsync(l => l.Id == vacancyCreateDto.locationId);

            //var recruiter = await _context.Recruiters.FirstOrDefaultAsync(r => r.AppUserId == userId);
            //var company = await _companyReadRepository.GetByIdAsync(companyId);
            // Handle not found

            List<JobType> jobTypes = await _context.JobTypes.Where(j => vacancyCreateDto.jobTypeIds.Contains(j.Id)).ToListAsync();
            List<ShiftAndSchedule> shifts = await _context.ShiftAndSchedules.Where(s => vacancyCreateDto.shiftIds.Contains(s.Id)).ToListAsync();
            Vacancy newVacancy = new()
            {
                JobTitle = vacancyCreateDto.jobTitle,
                ExperienceLevel = experience,
                Recruiter = recruiter,
                Salary = vacancyCreateDto.salary,
                ApplicationLimit = vacancyCreateDto.applicationLimit,
                JobType = jobTypes,
                JobLocation = jobLocation,
                Description = vacancyCreateDto.description,
                Company = company,
                ShiftAndSchedules = shifts
            };
            newVacancy.Company = company;
            newVacancy.Recruiter = recruiter;
            await _vacancyWriteRepository.AddAsync(newVacancy);
            await _vacancyWriteRepository.SaveChangesAsync();

            recruiter.CurrentPostCount++;
            _recruiterWriteRepository.Update(recruiter);
            await _recruiterWriteRepository.SaveChangesAsync();
        }

        public async Task<List<VacancyGetDto>> GetBySearch(string jobTitle)
        {
            var vacancies = await _context.Vacancy
                .Where(vacancy => vacancy.JobTitle.Contains(jobTitle))
                .ToListAsync();
            return _mapper.Map<List<VacancyGetDto>>(vacancies);
        }

        public async Task<List<VacancyGetDetailsDto>> GetDetailsBySearch(string? jobTitle, Guid? locationId)
        {
            IQueryable<Vacancy> query = _context.Vacancy
                .Include(v => v.Company)
                    .ThenInclude(c => c.Details)
                .Include(v => v.JobLocation)
                .Include(v => v.JobType)
                .Include(v => v.ShiftAndSchedules);

            if (!string.IsNullOrEmpty(jobTitle))
            {
                query = query.Where(v => v.JobTitle.ToLower().Contains(jobTitle.ToLower()));
            }

            if (locationId.HasValue)
            {
                query = query.Where(v => v.JobLocationId == locationId.Value);
            }

            var vacancies = await query.ToListAsync();

            return vacancies.Select(v => new VacancyGetDetailsDto(
                v.Id,
                v.JobTitle,
                v.Company.Name,
                v.JobLocation.Location,
                v.Salary,
                v.JobType.Select(jt => jt.TypeName).ToList(),
                v.ShiftAndSchedules.Select(ss => ss.ShiftName).ToList(),
                v.Description,
                v.Company.Details.Link,
                v.DateCreated,
                v.ApplicationLimit,
                v.CurrentApplicationCount
            )).ToList();
        }
        public async Task<List<VacancyGetByIdDto>> GetVacancyByRecruiterId(Guid id)
        {
            var list = await _context.Vacancy
                            .Include(v => v.Company)
                            .Include(v => v.JobLocation)
                            .Where(v => v.Recruiter.Id == id)
                            .ToListAsync();

            var mappedList = list.Select(vacancy => new VacancyGetByIdDto(
                vacancy.JobTitle,
                vacancy.Company.Name, 
                vacancy.JobLocation.Location 
            )).ToList();

            return mappedList;
        }


    }
}
