using CareerCompassAPI.Domain.Identity;
using CareerCompassAPI.Persistence.Contexts;
using CareerCompassAPI.Persistence.ExtensionMethods;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddFluentValidationValidators();
builder.Services.AddIdentity<AppUser, IdentityRole>(identityOption =>
{
    identityOption.User.RequireUniqueEmail = true;
    identityOption.Password.RequiredLength = 8;
    identityOption.Lockout.MaxFailedAccessAttempts = 3;
})
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<CareerCompassDbContext>();


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
    await instance.UserSeedAsync();
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
