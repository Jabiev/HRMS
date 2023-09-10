using CareerCompassAPI.Application.Abstraction.Repositories.IMessageRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Message_DTOs;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageWriteRepository _messageWriteRepository;
        private readonly IMessageReadRepository _messageReadRepository;
        private readonly CareerCompassDbContext _context;

        public MessageService(IMessageWriteRepository messageWriteRepository,
                              CareerCompassDbContext context,
                              IMessageReadRepository messageReadRepository)
        {
            _messageWriteRepository = messageWriteRepository;
            _context = context;
            _messageReadRepository = messageReadRepository;
        }
        public async Task CreateAsync(MessageCreateDto messageCreateDto)
        {
            var senderUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == messageCreateDto.senderId);
            var receiverUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == messageCreateDto.receiverId);

            if (senderUser == null || receiverUser == null)
            {
                throw new ArgumentException("Invalid sender or receiver ID");
            }

            var newMessage = new Domain.Entities.Message
            {
                Sender = senderUser,
                Receiver = receiverUser,
                Content = messageCreateDto.content,
                IsRead = false,
                MessageType = messageCreateDto.messageType,
            };
            await _messageWriteRepository.AddAsync(newMessage);
            await _messageWriteRepository.SaveChangesAsync();
        }

        public async Task<List<GetUnreadMessagesDto>> GetUnreadMessagesAsync(string senderId, string receiverId)
        {
            var unreadMessages = await _messageReadRepository.GetAllByExpression(
    m => ((m.Sender.Id == senderId && m.Receiver.Id == receiverId) ||
          (m.Sender.Id == receiverId && m.Receiver.Id == senderId)) &&
         !m.IsRead,
    int.MaxValue,
    0)
    .Select(m => new GetUnreadMessagesDto(m.Sender.Id, m.Receiver.Id, m.Content, m.MessageType))
    .ToListAsync();

            return unreadMessages;
        }

    }
}
