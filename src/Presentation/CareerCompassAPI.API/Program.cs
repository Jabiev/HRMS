using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Domain.Concretes;
using CareerCompassAPI.Domain.Identity;
using CareerCompassAPI.Infrastructure.Services;
using CareerCompassAPI.Infrastructure.Services.Azure;
using CareerCompassAPI.Persistence.Contexts;
using CareerCompassAPI.Persistence.ExtensionMethods;
using CareerCompassAPI.SignalR.Hubs;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices();
builder.Services.AddSignalRServices();
builder.Services.AddStorage<AzureStorage>();

#region Add Identity & JWT Authentication & Authorization

builder.Services.AddIdentity<AppUser, IdentityRole>(identityOption =>
{
    identityOption.User.RequireUniqueEmail = true;
    identityOption.Password.RequiredLength = 8;
    identityOption.Lockout.MaxFailedAccessAttempts = 3;
    identityOption.SignIn.RequireConfirmedEmail = true;
})
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<CareerCompassDbContext>();

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
   opt.TokenLifespan = TimeSpan.FromHours(2));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        LifetimeValidator = (_, expire, _, _) => expire > DateTime.UtcNow,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecurityKey"])),
        ClockSkew = TimeSpan.Zero,

        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireMaster", policy => policy.RequireRole("Master"));
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireRecruiter", policy => policy.RequireRole("Recruiter"));
    options.AddPolicy("RequireJobSeeker", policy => policy.RequireRole("JobSeeker"));

    options.AddPolicy("RequireAdminOrRecruiter", policy => policy.RequireRole("Admin", "Recruiter"));
    options.AddPolicy("RequireMasterOrAdmin", policy => policy.RequireRole("Master", "Admin"));
    options.AddPolicy("RequireMasterAdminRecruiter", policy => policy.RequireRole("Master", "Admin", "Recruiter"));
});

#endregion

#region Validation Services & Configuration Bindings
builder.Services.AddFluentValidationValidators();
var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});
#endregion

var app = builder.Build();

#region Seed Data
//using (var scope = app.Services.CreateScope())
//{
//    var instance = scope.ServiceProvider.GetRequiredService<CareerCompassDbContextInitialiser>();
//    await instance.InitialiseAsync();
//    await instance.SettingsSeed();
//    await instance.EducationLevelsSeed();
//    await instance.RoleSeedAsync();
//    await instance.SubscriptionsSeedAsync();
//    await instance.IndustrySeed();
//    await instance.LocationsSeed();
//    await instance.JobTypeSeed();
//    await instance.ExperienceLevelSeed();
//    await instance.ShiftAndScheduleSeed();
//    await instance.CompanySeedAsync();
//    await instance.SeedJobSeekersAsync();
//    await instance.SeedRecruitersAsync();
//    await instance.SeedReviewsAsync();
//    await instance.SeedVacanciesAsync();
//    await instance.UserSeedAsync();
//    //await instance.PaymentsSeedAsync();
//}
#endregion

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
//app.UseCustomExceptionHandler();
app.MapControllers();

#region SignalR Hubs
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<PaymentHub>("/payments");
    endpoints.MapHub<ApplicationHub>("/application");
    endpoints.MapHub<ChatHub>("/chat");
    endpoints.MapHub<VideoHub>("/video").RequireAuthorization();
    endpoints.MapHub<VideoHub>("/review");
});
#endregion

#region Hangfire Recurring Jobs
app.MapHangfireDashboard("/hangfire", new DashboardOptions());
RecurringJob.AddOrUpdate("check-subscription", () => app.Services.CreateScope().ServiceProvider.GetRequiredService<IHangFireService>().CheckSubscriptions(), Cron.Hourly);
RecurringJob.AddOrUpdate("delete-old-messages", () => app.Services.CreateScope().ServiceProvider.GetRequiredService<IHangFireService>().DeleteOldMessages(), "0 0 * * *");
RecurringJob.AddOrUpdate("delete-old-notifications", () => app.Services.CreateScope().ServiceProvider.GetRequiredService<IHangFireService>().DeleteOldNotifications(), "0 0 * * *");
RecurringJob.AddOrUpdate("delete-declined-applications", () => app.Services.CreateScope().ServiceProvider.GetRequiredService<IHangFireService>().DeleteDeclinedApplications(), "0 0 * * *");
RecurringJob.AddOrUpdate("delete-declined-reviews", () => app.Services.CreateScope().ServiceProvider.GetRequiredService<IHangFireService>().DeleteDeclinedReviews(), "0 0 * * *");
RecurringJob.AddOrUpdate("delete-full-vacancies", () => app.Services.CreateScope().ServiceProvider.GetRequiredService<IHangFireService>().DeleteFullVacancies(), "0 0 * * *");
RecurringJob.AddOrUpdate("delete-old-vacancies", () => app.Services.CreateScope().ServiceProvider.GetRequiredService<IHangFireService>().DeleteOldVacancies(), "0 0 * * *");
#endregion


app.Run();
