using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Resume_DTOs;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;
using CareerCompassAPI.Persistence.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class ResumeService : IResumeService
    {
        private readonly CareerCompassDbContext _context;

        public ResumeService(CareerCompassDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateResume(ResumeCreateDto resumeCreateDto)
        {
            if (resumeCreateDto is null)
            {
                throw new ArgumentNullException("Arguments passed in may not contain null values");
            }
            Resume newResume = new()
            {
                Name = resumeCreateDto.name,
                Price = resumeCreateDto.price,
                Description = resumeCreateDto.description,
                Structure = resumeCreateDto.structure,
            };
            await _context.AddAsync(newResume);
            await _context.SaveChangesAsync();
            return newResume.Id;
        }

        public async Task<List<ResumeGetDto>> GetAllResumes()
        {
            var resumeList = await _context.Resumes.ToListAsync();
            if (resumeList.Count == 0)
            {
                throw new NotFoundException("Resumes do not exist");
            }
            List<ResumeGetDto> resumes = resumeList.Select(resume => new ResumeGetDto(resume.Id, resume.Name, resume.Price, resume.Description, resume.Structure)).ToList();
            return resumes;
        }

        public async Task<ResumeGetDto> GetResumeById(Guid id)
        {
            var resume = await _context.Resumes.FindAsync(id);
            if (resume == null)
            {
                throw new NotFoundException("Resume not found");
            }

            return new ResumeGetDto(resume.Id, resume.Name, resume.Price, resume.Description, resume.Structure);
        }

    }
}
