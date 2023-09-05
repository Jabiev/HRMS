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
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices();
builder.Services.AddSignalRServices();
builder.Services.AddStorage<AzureStorage>(); 

builder.Services.AddIdentity<AppUser, IdentityRole>(identityOption =>
{
    identityOption.User.RequireUniqueEmail = true;
    identityOption.Password.RequiredLength = 8;
    identityOption.Lockout.MaxFailedAccessAttempts = 3;
})
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<CareerCompassDbContext>();

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
   opt.TokenLifespan = TimeSpan.FromHours(2));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:issuer"],
        ValidAudience = builder.Configuration["Jwt:audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"])),
        LifetimeValidator = (_, expire, _, _) => expire > DateTime.UtcNow,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddFluentValidationValidators();
var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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


var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var instance = scope.ServiceProvider.GetRequiredService<CareerCompassDbContextInitialiser>();
    await instance.InitialiseAsync();
    await instance.EducationLevelsSeed();
    await instance.RoleSeedAsync();
    await instance.SubscriptionsSeedAsync();
    await instance.IndustrySeed();
    await instance.LocationsSeed();
    await instance.JobTypeSeed();
    await instance.ExperienceLevelSeed();
    await instance.ShiftAndScheduleSeed();
    await instance.UserSeedAsync();
}
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
app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<PaymentHub>("/payments");
    endpoints.MapHub<ApplicationHub>("/application");
    endpoints.MapHub<ChatHub>("/chat");
    endpoints.MapHub<VideoHub>("/video").RequireAuthorization();
});
app.MapHangfireDashboard("/hangfire", new DashboardOptions());
RecurringJob.AddOrUpdate("check-subscription", () => app.Services.CreateScope().ServiceProvider.GetRequiredService<IHangFireService>().CheckSubscriptions(), Cron.Hourly);
app.Run();
