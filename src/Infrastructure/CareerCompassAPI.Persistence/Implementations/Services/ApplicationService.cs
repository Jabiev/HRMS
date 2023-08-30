using CareerCompassAPI.Application.Abstraction.Repositories.IJobApplicationRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IJobSeekerRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IVacancyRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Application_DTOs;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Domain.Enums;
using CareerCompassAPI.Persistence.Contexts;
using Hangfire;
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
                    application.Id,
                    application.JobSeeker.FirstName,
                    application.JobSeeker.LastName,
                    application.Vacancy.JobTitle,
                    file?.BlobPath ?? "NoFile",
                    application.Status
                ));
            }
            return dtos;
        }

        public async Task<List<ApprovedApplicantGetDto>> GetApprovedApplicantsByAppUserId(string appUserId)
        {
            var recruiter = await _context.Recruiters.FirstOrDefaultAsync(r => r.AppUserId == appUserId);

            if (recruiter == null)
            {
                return new List<ApprovedApplicantGetDto>();
            }

            var approvedApplications = await _context.Applications
                .Where(ja => ja.Vacancy.Recruiter.Id == recruiter.Id && ja.Status == ApplicationStatus.Approved)
                .Include(ja => ja.JobSeeker)
                .Include(ja => ja.Vacancy)
                .ToListAsync();

            var approvedApplicantDtos = new List<ApprovedApplicantGetDto>();
            foreach (var application in approvedApplications)
            {
                approvedApplicantDtos.Add(new ApprovedApplicantGetDto(
                    application.Id,
                    application.JobSeeker.FirstName,
                    application.JobSeeker.LastName,
                    application.Vacancy.JobTitle
                ));
            }
            return approvedApplicantDtos;
        }


        public async Task UpdateAsync(ApplicationStatusUpdateDto applicationStatusUpdateDto)
        {
            JobApplications application = await _jobApplicationReadRepository.GetByIdAsync(applicationStatusUpdateDto.applicationId);

            if (application == null)
            {
                throw new InvalidOperationException("The specified job application does not exist.");
            }
            var jobseekerId = application.JobSeekerId;
            var userId = await _jobSeekerReadRepository.GetByIdAsync(jobseekerId);
            Guid parsedId = Guid.Parse(userId.AppUserId);
            application.Status = applicationStatusUpdateDto.newStatus;
            await _jobApplicationWriteRepository.SaveChangesAsync();
            string title = "Career Compass - Application Status Changed!";
            string message = $"Your application  has been {application.Status}.";

            BackgroundJob.Schedule<INotificationService>(
                x => x.CreateAsync(parsedId, title, message),
                TimeSpan.FromSeconds(10)
            );
        }
    }
}
