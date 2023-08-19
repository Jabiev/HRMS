using CareerCompassAPI.Domain.Concretes;
using CareerCompassAPI.Domain.Identity;
using CareerCompassAPI.Infrastructure.Services;
using CareerCompassAPI.Infrastructure.Services.Azure;
using CareerCompassAPI.Persistence.Contexts;
using CareerCompassAPI.Persistence.ExtensionMethods;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices();
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
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var instance = scope.ServiceProvider.GetRequiredService<CareerCompassDbContextInitialiser>();
    await instance.InitialiseAsync();
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
app.UseCors("AllowAllOrigins");


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();
app.MapHangfireDashboard("/hangfire", new DashboardOptions());
app.Run();
