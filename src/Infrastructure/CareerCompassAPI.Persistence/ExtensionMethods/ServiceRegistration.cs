using CareerCompassAPI.Application.Abstraction.Repositories;
using CareerCompassAPI.Application.Abstraction.Repositories.ICompanyRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IEventRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IFeedbackRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IFileRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IFollowerRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IIndustryRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IJobApplicationRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IJobSeekerRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IMessageRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.INotificationRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IPaymentRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IRecruiterRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IReviewRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.ISubscriptionRepository;
using CareerCompassAPI.Application.Abstraction.Repositories.ITeamRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IVacancyRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Infrastructure.Services;
using CareerCompassAPI.Infrastructure.Services.Token;
using CareerCompassAPI.Persistence.Contexts;
using CareerCompassAPI.Persistence.Implementations.Repositories;
using CareerCompassAPI.Persistence.Implementations.Repositories.CompanyRepositories;
using CareerCompassAPI.Persistence.Implementations.Repositories.EventRepositories;
using CareerCompassAPI.Persistence.Implementations.Repositories.FeedbackRepositories;
using CareerCompassAPI.Persistence.Implementations.Repositories.FileRepositories;
using CareerCompassAPI.Persistence.Implementations.Repositories.FollowerRepositories;
using CareerCompassAPI.Persistence.Implementations.Repositories.IndustryRepositories;
using CareerCompassAPI.Persistence.Implementations.Repositories.JobApplicationRepositories;
using CareerCompassAPI.Persistence.Implementations.Repositories.JobSeekerRepositories;
using CareerCompassAPI.Persistence.Implementations.Repositories.MessageRepositories;
using CareerCompassAPI.Persistence.Implementations.Repositories.NotificationRepositories;
using CareerCompassAPI.Persistence.Implementations.Repositories.PaymentRepositories;
using CareerCompassAPI.Persistence.Implementations.Repositories.RecruiterRepositories;
using CareerCompassAPI.Persistence.Implementations.Repositories.ReviewRepositories;
using CareerCompassAPI.Persistence.Implementations.Repositories.SubscriptionRepositories;
using CareerCompassAPI.Persistence.Implementations.Repositories.TeamRepositories;
using CareerCompassAPI.Persistence.Implementations.Repositories.VacancyRepositories;
using CareerCompassAPI.Persistence.Implementations.Services;
using CareerCompassAPI.Persistence.Interceptors;
using CareerCompassAPI.Persistence.MapperProfiles;
using CareerCompassAPI.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using EventService = CareerCompassAPI.Persistence.Implementations.Services.EventService;
using FileService = CareerCompassAPI.Persistence.Implementations.Services.FileService;

namespace CareerCompassAPI.Persistence.ExtensionMethods;

public static class ServiceRegistration
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddScoped<CareerCompassDbContextInitialiser>();
        AddReadRepositories(services);
        AddWriteRepositories(services);
        services.AddScoped<ISubscriptionService, Implementations.Services.SubscriptionService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IIndustryService, IndustryService>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IRecruiterService, RecruiterService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<IExperienceLevelService, ExperienceLevelService>();
        services.AddScoped<IJobTypeService, JobTypeService>();
        services.AddScoped<IShiftScheduleService, ShiftScheduleService>();
        services.AddScoped<IVacancyService, VacancyService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IStripeAppService, StripeAppService>();
        services.AddScoped<IHangFireService, HangFireService>();
        services.AddScoped<IEducationLevelService, EducationLevelService>();
        services.AddScoped<IJobSeekerService, JobSeekerService>();
        services.AddScoped<IPaymentsService, PaymentService>();
        services.AddScoped<IAppSettingService, AppSettingService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IApplicationService, ApplicationService>();
        services.AddScoped<ICareerCompassDbContext, CareerCompassDbContext>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IFollowerService, FollowerService>();
        services.AddScoped<ITeamMemberService, TeamMemberService>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IFeedbackService, FeedbackService>();
        services.AddScoped<IResumeService, ResumeService>();
        services.AddScoped<IReviewService, Implementations.Services.ReviewService>();
        services.AddTransient<Stripe.Checkout.SessionService>();
        StripeConfiguration.ApiKey = configuration.GetValue<string>("StripeSettings:SecretKey");
        services.AddScoped<CustomerService>()
               .AddScoped<ChargeService>()
               .AddScoped<TokenService>();

        services.AddAutoMapper(typeof(SubscriptionProfile).Assembly);
        services.AddDbContext<CareerCompassDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default")));
        services.AddScoped<AuditInterceptor>();
        services.AddHangfire(options =>
        {
            options.UsePostgreSqlStorage(configuration.GetConnectionString("Hangfire"));
        });
        services.AddHangfireServer();
        services.AddHttpContextAccessor();
    }

    private static void AddReadRepositories(IServiceCollection services)
    {
        services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
        services.AddScoped<ISubscriptionReadRepository, SubscriptionReadRepository>();
        services.AddScoped<IJobSeekerReadRepository, JobSeekerReadRepository>();
        services.AddScoped<IRecruiterReadRepository, RecruiterReadRepository>();
        services.AddScoped<ICompanyReadRepository, CompanyReadRepository>();
        services.AddScoped<IIndustryReadRepository, IndustryReadRepository>();
        services.AddScoped<INotificationReadRepository, NotificationReadRepository>();
        services.AddScoped<IRecruiterReadRepository, RecruiterReadRepository>();
        services.AddScoped<IVacancyReadRepository, VacancyReadRepository>();
        services.AddScoped<IPaymentReadRepository, PaymentReadRepository>();
        services.AddScoped<IFeedbackReadRepository, FeedbackReadRepository>();
        services.AddScoped<IFileReadRepository, FileReadRepository>();
        services.AddScoped<ITeamReadRepository, TeamReadRepository>();
        services.AddScoped<IEventReadRepository, EventReadRepository>();
        services.AddScoped<IJobApplicationReadRepository, JobApplicationReadRepository>();
        services.AddScoped<IMessageReadRepository, MessageReadRepository>();
        services.AddScoped<IReviewReadRepository, ReviewReadRepository>();
        services.AddScoped<IFollowerReadRepository, FollowerReadRepository>();
    }

    private static void AddWriteRepositories(IServiceCollection services)
    {
        services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
        services.AddScoped<ISubscriptionWriteRepository, SubscriptionWriteRepository>();
        services.AddScoped<IJobSeekerWriteRepository, JobSeekerWriteRepository>();
        services.AddScoped<IRecruiterWriteRepository, RecruiterWriteRepository>();
        services.AddScoped<ICompanyWriteRepository, CompanyWriteRepository>();
        services.AddScoped<IIndustryWriteRepository, IndustryWriteRepository>();
        services.AddScoped<INotificationWriteRepository, NotificationWriteRepository>();
        services.AddScoped<IRecruiterWriteRepository, RecruiterWriteRepository>();
        services.AddScoped<IVacancyWriteRepository, VacancyWriteRepository>();
        services.AddScoped<IPaymentWriteRepository, PaymentWriteRepository>();
        services.AddScoped<IFeedbackWriteRepository, FeedbackWriteRepository>();
        services.AddScoped<IFileWriteRepository, FileWriteRepository>();
        services.AddScoped<IEventWriteRepository, EventWriteRepository>();
        services.AddScoped<ITeamWriteRepository, TeamWriteRepository>();
        services.AddScoped<IJobApplicationWriteRepository, JobApplicationWriteRepository>();
        services.AddScoped<IMessageWriteRepository, MessageWriteRepository>();
        services.AddScoped<IReviewWriteRepository, ReviewWriteRepository>();
        services.AddScoped<IFollowerWriteRepository, FollowerWriteRepository>();
    }
}
