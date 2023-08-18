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

        public VacancyService(CareerCompassDbContext context,
                              IRecruiterReadRepository recruiterReadRepository,
                              ICompanyReadRepository companyReadRepository,
                              IVacancyWriteRepository vacancyWriteRepository)
        {
            _context = context;
            _recruiterReadRepository = recruiterReadRepository;
            _companyReadRepository = companyReadRepository;
            _vacancyWriteRepository = vacancyWriteRepository;
        }

        public async Task Create(VacancyCreateDto vacancyCreateDto, string userId, Guid companyId)
        {
            if (vacancyCreateDto is null)
            {
                throw new ArgumentNullException();
            }
            ExperienceLevel experience = await _context.ExperienceLevels.FirstOrDefaultAsync(e => e.Id == vacancyCreateDto.experienceLevelId);
            JobType jobType = await _context.JobTypes.FirstOrDefaultAsync(j => j.Id == vacancyCreateDto.jobTypeId);
            JobLocation jobLocation = await _context.JobLocations.FirstOrDefaultAsync(l => l.Id == vacancyCreateDto.jobLocationId);
            Recruiter recruiter = await _context.Recruiters.FirstOrDefaultAsync(r => r.AppUserId == userId);
            Company company = await _companyReadRepository.GetByIdAsync(companyId);
            // Handle not found

            Vacancy newVacancy = new()
            {
                JobTitle = vacancyCreateDto.jobTitle,
                ExperienceLevel = experience,
                Recruiter = recruiter,
                Salary = vacancyCreateDto.salary,
                JobType = jobType,
                JobLocation = jobLocation,
                Description = vacancyCreateDto.description,
                Company = company,
                ShiftAndSchedules = vacancyCreateDto.shifts.ToList(),
            };
            await _vacancyWriteRepository.AddAsync(newVacancy);
            await _vacancyWriteRepository.SaveChangesAsync();
        }
    }
}
