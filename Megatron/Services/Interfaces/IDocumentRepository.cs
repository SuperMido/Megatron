using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Megatron.Services
{
    public interface IDocumentRepository
    {
        bool UploadDocument(IFormFileCollection files, int articleId);
    }
}