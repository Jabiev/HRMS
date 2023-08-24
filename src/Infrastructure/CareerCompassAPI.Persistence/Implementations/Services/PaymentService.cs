using CareerCompassAPI.Application.Abstraction.Repositories.IPaymentRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Payment_DTOs;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Domain.Identity;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class PaymentService : IPaymentsService
    {
        private readonly IPaymentWriteRepository _paymentWriteRepository;
        private readonly CareerCompassDbContext _context;
        public PaymentService(IPaymentWriteRepository paymentWriteRepository,
                              CareerCompassDbContext context)
        {
            _paymentWriteRepository = paymentWriteRepository;
            _context = context;
        }

        public async Task CreateAsync(PaymentCreateDto paymentCreateDto)
        {
            if (paymentCreateDto is null)
            {
                throw new ArgumentNullException();
            }
            AppUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == paymentCreateDto.appUserId);
            Payments newPayment = new()
            {
                AppUser = user,
                Amount = paymentCreateDto.amount,
                Type = paymentCreateDto.type
            };
            await _paymentWriteRepository.AddAsync(newPayment);
            await _paymentWriteRepository.SaveChangesAsync();
        }
    }
}
