using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Exceptions.Database;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ArgumentNullException = ChatApp.Domain.Exceptions.Runtime.ArgumentNullException;

namespace ChatApp.Infrastructure.Repositories
{
    public class AttachmentRepository(AppDbContext context)
        : GenericRepository<Attachment>(context), IAttachmentRepository
    {
        public async Task SaveMetadataAsync(Attachment attachment)
        {
            var existingAttachment = await DbSet.FirstOrDefaultAsync(a => a.Guid == attachment.Guid);
            if(existingAttachment != null)
            {
                // Update existing attachment metadata
                existingAttachment.FileType = attachment.FileType;
                existingAttachment.FileUrl = attachment.FileUrl;
                existingAttachment.AttType = attachment.AttType;
                existingAttachment.FileSize = attachment.FileSize;
                existingAttachment.UploadedAt = attachment.UploadedAt ?? DateTime.UtcNow;
                // Update the UpdatedAt timestamp
                existingAttachment.UpdatedAt = DateTime.UtcNow;
                DbSet.Update(existingAttachment);
            }
            else
            {
                // Add new attachment metadata
                await DbSet.AddAsync(attachment);
            }
        }

        public Task<Attachment?> GetAttachmentByAttIdAsync(Guid attachmentId)
        {
            return GetByIdAsync(attachmentId);
        }

        public async Task DeleteFileMetadataAsync(Guid attachmentId)
        {
            var deletingEntity = await GetByIdAsync(attachmentId);
            if(deletingEntity == null)
            {
                throw new ArgumentNullException($"Attachment with ID {attachmentId} does not exist.", nameof(attachmentId));
            }
            DbSet.Remove(deletingEntity);
        }
    }
}
