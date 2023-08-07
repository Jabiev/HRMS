using CareerCompassAPI.Domain.Identity;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddDbContext<CareerCompassDbContext>(options =>
options.UseSqlServer(builder.Services.BuildServiceProvider().GetService<IConfiguration>().GetConnectionString("Default")));

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


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
