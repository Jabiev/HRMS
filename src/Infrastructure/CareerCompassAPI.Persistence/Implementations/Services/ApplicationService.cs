using CareerCompassAPI.Application.Abstraction.Repositories.IJobApplicationRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IJobSeekerRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IVacancyRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Application_DTOs;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly CareerCompassDbContext _context;
        private readonly IJobSeekerReadRepository _jobSeekerReadRepository;
        private readonly IVacancyReadRepository _vacancyReadRepository;
        private readonly IVacancyWriteRepository _vacancyWriteRepository;
        private readonly IJobApplicationWriteRepository _jobApplicationWriteRepository;

        public ApplicationService(CareerCompassDbContext context,
                                  IJobSeekerReadRepository jobSeekerReadRepository,
                                  IVacancyReadRepository vacancyReadRepository,
                                  IJobApplicationWriteRepository jobApplicationWriteRepository,
                                  IVacancyWriteRepository vacancyWriteRepository)
        {
            _context = context;
            _jobSeekerReadRepository = jobSeekerReadRepository;
            _vacancyReadRepository = vacancyReadRepository;
            _jobApplicationWriteRepository = jobApplicationWriteRepository;
            _vacancyWriteRepository = vacancyWriteRepository;
        }

        public async Task CreateAsync(ApplicationCreateDto applicationCreateDto)
        {
            Vacancy vacancy = await _vacancyReadRepository.GetByIdAsync(applicationCreateDto.vacancyId);
            JobSeeker jobSeeker = await _jobSeekerReadRepository.GetByIdAsync(applicationCreateDto.jobSeekerId);

            if (applicationCreateDto is null)
            {
                throw new ArgumentNullException();
            }

            if (vacancy.CurrentApplicationCount >= vacancy.ApplicationLimit)
            {
                throw new InvalidOperationException("Application limit for this vacancy has been reached.");
            }

            JobApplications newJobApplication = new()
            {
                Vacancy = vacancy,
                JobSeeker = jobSeeker,
                Status = Domain.Enums.ApplicationStatus.Pending
            };

            await _jobApplicationWriteRepository.AddAsync(newJobApplication);

            vacancy.CurrentApplicationCount += 1;
            _vacancyWriteRepository.Update(vacancy);

            await _jobApplicationWriteRepository.SaveChangesAsync();
            await _vacancyWriteRepository.SaveChangesAsync();
        }

    }
}
