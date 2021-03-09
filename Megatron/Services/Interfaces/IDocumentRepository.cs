using Microsoft.AspNetCore.Http;

namespace Megatron.Services
{
    public interface IDocumentRepository
    {
        bool UploadDocument(IFormFileCollection files, int articleId);
        void ConvertHtmlToDoc(int facultyId);
        void DownloadZip(int facultyId);

        byte[] FinalResult(int facultyId);
        string FileZipName(int facultyId);

    }
}