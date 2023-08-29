using CareerCompassAPI.Application.Abstraction.Repositories.IJobApplicationRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IJobSeekerRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IVacancyRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Application_DTOs;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly CareerCompassDbContext _context;
        private readonly IJobSeekerReadRepository _jobSeekerReadRepository;
        private readonly IVacancyReadRepository _vacancyReadRepository;
        private readonly IVacancyWriteRepository _vacancyWriteRepository;
        private readonly IJobApplicationWriteRepository _jobApplicationWriteRepository;
        private readonly IJobApplicationReadRepository _jobApplicationReadRepository;
        private readonly ILogger<ApplicationService> _logger;


        public ApplicationService(CareerCompassDbContext context,
                                  IJobSeekerReadRepository jobSeekerReadRepository,
                                  IVacancyReadRepository vacancyReadRepository,
                                  IJobApplicationWriteRepository jobApplicationWriteRepository,
                                  IVacancyWriteRepository vacancyWriteRepository,
                                  IJobApplicationReadRepository jobApplicationReadRepository,
                                  ILogger<ApplicationService> logger)
        {
            _context = context;
            _jobSeekerReadRepository = jobSeekerReadRepository;
            _vacancyReadRepository = vacancyReadRepository;
            _jobApplicationWriteRepository = jobApplicationWriteRepository;
            _vacancyWriteRepository = vacancyWriteRepository;
            _jobApplicationReadRepository = jobApplicationReadRepository;
            _logger = logger;
        }

        public async Task<int> CreateAsync(ApplicationCreateDto applicationCreateDto)
        {
            if (applicationCreateDto is null)
            {
                throw new ArgumentNullException(nameof(applicationCreateDto), "The application cannot be null.");
            }

            Vacancy vacancy = await _vacancyReadRepository.GetByIdAsync(applicationCreateDto.vacancyId);
            if (vacancy == null)
            {
                throw new InvalidOperationException("The specified vacancy does not exist.");
            }

            if (vacancy.CurrentApplicationCount >= vacancy.ApplicationLimit)
            {
                throw new InvalidOperationException("Application limit for this vacancy has been reached.");
            }

            JobSeeker jobSeeker = await _jobSeekerReadRepository.GetByIdAsync(applicationCreateDto.jobSeekerId);
            if (jobSeeker == null)
            {
                throw new InvalidOperationException("The specified job seeker does not exist.");
            }

            JobApplications newJobApplication = new()
            {
                Vacancy = vacancy,
                JobSeeker = jobSeeker,
                Status = Domain.Enums.ApplicationStatus.Pending
            };

            await _jobApplicationWriteRepository.AddAsync(newJobApplication);

            vacancy.CurrentApplicationCount++;
            _vacancyWriteRepository.Update(vacancy);

            await _jobApplicationWriteRepository.SaveChangesAsync();
            await _vacancyWriteRepository.SaveChangesAsync();

            return vacancy.CurrentApplicationCount;
        }

        public async Task<List<ApplicantsGetDto>> GetApplicationsByAppUserId(string appUserId)
        {
            var recruiter = await _context.Recruiters.FirstOrDefaultAsync(r => r.AppUserId == appUserId);

            var applications = await _jobApplicationReadRepository
                .GetAllByExpression(
                    ja => ja.Vacancy.Recruiter.Id == recruiter.Id,
                    50,
                    0
                )
                .Include(ja => ja.JobSeeker)
                .Include(ja => ja.Vacancy)
                .ToListAsync();

            var dtos = new List<ApplicantsGetDto>();

            foreach (var application in applications)
            {
                var jobSeekerAppUserId = application.JobSeeker.AppUserId;
                var file = await _context.Files.FirstOrDefaultAsync(f => f.User.Id == jobSeekerAppUserId);

                dtos.Add(new ApplicantsGetDto(
                    application.JobSeeker.FirstName,
                    application.JobSeeker.LastName,
                    application.Vacancy.JobTitle,
                    file?.BlobPath ?? "NoFile"
                ));
            }
            return dtos;
        }
    }
}
