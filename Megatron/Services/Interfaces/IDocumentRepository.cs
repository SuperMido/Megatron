using Microsoft.AspNetCore.Http;

namespace Megatron.Services
{
    public interface IDocumentRepository
    {
        bool UploadDocument(IFormFileCollection files, int articleId);
        void ConvertHtmlToDoc(int facultyId, int semesterId);
        void DownloadZip(int facultyId, int semesterId);

        byte[] FinalResult(int facultyId, int semesterId);
        string FileZipName(int facultyId, int semesterId);
        string GetFilePath(string fileName);
        string GetOutPutDirectory();

        bool DeleteDocumentByName(string name);

        byte[] GetDocumentByName(string name);
    }
}