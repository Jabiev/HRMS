using CareerCompassAPI.Application.Abstraction.Repositories.IJobApplicationRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IJobSeekerRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IVacancyRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Application_DTOs;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly CareerCompassDbContext _context;
        private readonly IJobSeekerReadRepository _jobSeekerReadRepository;
        private readonly IVacancyReadRepository _vacancyReadRepository;
        private readonly IJobApplicationWriteRepository _jobApplicationWriteRepository;

        public ApplicationService(CareerCompassDbContext context,
                                  IJobSeekerReadRepository jobSeekerReadRepository,
                                  IVacancyReadRepository vacancyReadRepository,
                                  IJobApplicationWriteRepository jobApplicationWriteRepository)
        {
            _context = context;
            _jobSeekerReadRepository = jobSeekerReadRepository;
            _vacancyReadRepository = vacancyReadRepository;
            _jobApplicationWriteRepository = jobApplicationWriteRepository;
        }

        public async Task CreateAsync(ApplicationCreateDto applicationCreateDto)
        {
            Vacancy vacancy = await _vacancyReadRepository.GetByIdAsync(applicationCreateDto.vacancyId);
            JobSeeker jobSeeker = await _jobSeekerReadRepository.GetByIdAsync(applicationCreateDto.jobSeekerId);
            if (applicationCreateDto is null)
            {
                throw new ArgumentNullException();
            }
            JobApplications newJobApplication = new()
            {
                Vacancy = vacancy,
                JobSeeker = jobSeeker,
                Status = Domain.Enums.ApplicationStatus.Pending
            };
            await _jobApplicationWriteRepository.AddAsync(newJobApplication);
            await _jobApplicationWriteRepository.SaveChangesAsync();
        }
    }
}
