using Microsoft.AspNetCore.Http;

namespace Megatron.Services
{
    public interface IUnitOfWork
    {
        void UploadImage(IFormFile file);
    }
}