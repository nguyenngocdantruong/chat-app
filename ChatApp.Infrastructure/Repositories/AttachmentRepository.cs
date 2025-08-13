using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;

namespace ChatApp.Infrastructure.Repositories
{
    public class AttachmentRepository : GenericRepository<Attachment>, IAttachmentRepository
    {
        public AttachmentRepository(AppDbContext context) : base(context)
        {
        }

        public Task SaveMetadataAsync(Attachment attachment)
        {
            throw new NotImplementedException();
        }

        public Task<Attachment?> GetAttachmentByAttIdAsync(Guid attachmentId)
        {
            throw new NotImplementedException();
        }

        public Task<Stream?> GetFileStreamAsync(Guid attachmentId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFileMetadataAsync(Guid attachmentId)
        {
            throw new NotImplementedException();
        }
    }
}
