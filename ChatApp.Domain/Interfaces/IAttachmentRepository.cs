using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces
{
    public interface IAttachmentRepository : IGenericRepository<Attachment>
    {
        Task SaveMetadataAsync(Attachment attachment);
        Task<Attachment?> GetAttachmentByAttIdAsync(Guid attachmentId);
        Task DeleteFileMetadataAsync(Guid attachmentId);
    }
}
