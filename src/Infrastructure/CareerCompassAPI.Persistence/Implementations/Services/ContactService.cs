using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Contact_DTOs;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class ContactService : IContactService
    {
        private readonly IMailService _mailService;
        private readonly CareerCompassDbContext _context;

        public ContactService(IMailService mailService,
                              CareerCompassDbContext context)
        {
            _mailService = mailService;
            _context = context;
        }
        public string GetSetting(string settingName)
        {
            var setting = _context.Settings.FirstOrDefault(s => s.SettingName == settingName);
            return setting?.SettingValue;
        }

        public async Task SendContactEmail(ContactCreateDto contactCreateDto)
        {
            string adminEmail = GetSetting("ContactMainEmail");

            var toEmails = new List<string> { adminEmail };

            var subject = "New Contact Submission";
            var content = $@"<html>
                      <body>
                        <h1>New Contact Message Received</h1>
                        <p><strong>Name: </strong>{contactCreateDto.name}</p>
                        <p><strong>Surname: </strong>{contactCreateDto.surname}</p>
                        <p><strong>Email: </strong>{contactCreateDto.email}</p>
                        <p><strong>Message: </strong>{contactCreateDto.message}</p>
                      </body>
                    </html>";

            var message = new Message(toEmails, subject, content);

            await _mailService.SendEmailAsync(message);
        }
    }
}
