using Microsoft.AspNetCore.Http;

namespace Megatron.Extensions
{
    public static class ImageValidator
    {
        public static bool IsImage(this IFormFile file)
        {
            try
            {
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}