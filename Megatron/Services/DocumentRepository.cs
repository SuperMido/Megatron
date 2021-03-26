using System;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Zip;
using Megatron.Data;
using Megatron.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Spire.Doc;

namespace Megatron.Services
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _dbContext;
        private readonly IArticleRepository _articleRepository;

        public DocumentRepository(IWebHostEnvironment webHostEnvironment, ApplicationDbContext dbContext,
            IArticleRepository articleRepository)
        {
            _webHostEnvironment = webHostEnvironment;
            _dbContext = dbContext;
            _articleRepository = articleRepository;
        }

        public bool UploadDocument(IFormFileCollection files, int articleId)
        {
            var result = true;

            if (files.Count == 0)
            {
                return false;
            }

            foreach (var formFile in files)
            {
                var upFileName = formFile.FileName;
                var fileName = Guid.NewGuid() + Path.GetExtension(upFileName);
                var saveDir = Path.Combine(_webHostEnvironment.WebRootPath, "files");
                var savePath = Path.Combine(saveDir, fileName);
                try
                {
                    if (!Directory.Exists(saveDir))
                    {
                        Directory.CreateDirectory(saveDir);
                    }

                    using var fs = File.Create(savePath);
                    formFile.CopyTo(fs);
                    fs.Flush();
                    var newDocument = new ArticleDocument
                    {
                        ArticleId = articleId,
                        DocumentFile = fileName
                    };
                    _dbContext.ArticleDocuments.Add(newDocument);
                    _dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    result = false;
                }
            }

            return result;
        }

        public void ConvertHtmlToDoc(int facultyId, int semesterId)
        {
            var listArticleApproved =
                _articleRepository.GetListArticlesApprovedAfterFinalDateByFaculty(facultyId, semesterId);
            foreach (var article in listArticleApproved)
            {
                var articleDocInDb = _dbContext.ArticleDocuments.Where(d => article.Id.Equals(d.ArticleId)).ToList();
                if (articleDocInDb.Any()) continue;
                var fileName = Guid.NewGuid() + ".docx";
                var docPath = GetDocPath(fileName);
                ConvertDocument(article.Content, docPath);

                var articleDoc = new ArticleDocument()
                {
                    ArticleId = article.Id,
                    DocumentFile = fileName
                };
                _dbContext.ArticleDocuments.Add(articleDoc);
                _dbContext.SaveChanges();
            }
        }

        private string GetDocPath(string fileName)
        {
            var docDir = Path.Combine(_webHostEnvironment.WebRootPath, "files");
            return Path.Combine(docDir, fileName);
        }

        private static void ConvertDocument(string html, string outPutFile)
        {
            var document = new Document();
            var session = document.AddSection();
            var paragraph = session.AddParagraph();

            var htmlString = html;
            paragraph.AppendHTML(htmlString);
            document.SaveToFile(outPutFile, FileFormat.Docx2013);
        }

        public void DownloadZip(int facultyId, int semesterId)
        {
            var listArticleApproved = _articleRepository
                .GetListArticlesApprovedAfterFinalDateByFaculty(facultyId, semesterId).Select(a => a.Id).ToArray();
            var listDocArticleApproved = _dbContext.ArticleDocuments
                .Where(d => listArticleApproved.Any(a => a.Equals(d.ArticleId))).ToList();
            var zipDir = GetZipDir();
            if (!Directory.Exists(zipDir))
            {
                Directory.CreateDirectory(zipDir);
            }

            var tempOutPut = GetTempOutPut(facultyId, semesterId);
            var zipOutputStream = new ZipOutputStream(File.Create(tempOutPut));
            zipOutputStream.SetLevel(9);

            var buffer = new byte[4096];

            foreach (var doc in listDocArticleApproved)
            {
                var docPath = GetDocPath(doc.DocumentFile);
                var entry = new ZipEntry(Path.GetFileName(docPath)) {DateTime = DateTime.Now, IsUnicodeText = true};
                zipOutputStream.PutNextEntry(entry);

                var fileStream = File.OpenRead(docPath);
                int sourceBytes;
                do
                {
                    sourceBytes = fileStream.Read(buffer, 0, buffer.Length);
                    zipOutputStream.Write(buffer, 0, sourceBytes);
                } while (sourceBytes > 0);
            }

            zipOutputStream.Finish();
            zipOutputStream.Flush();
            zipOutputStream.Close();
        }

        public byte[] FinalResult(int facultyId, int semesterId)
        {
            var tempOutPut = GetTempOutPut(facultyId, semesterId);
            var finalResult = File.ReadAllBytes(tempOutPut);
            if (File.Exists(tempOutPut))
            {
                File.Delete(tempOutPut);
            }

            if (finalResult == null || !finalResult.Any())
            {
                throw new Exception("Nothing Found");
            }

            return finalResult;
        }

        public string FileZipName(int facultyId, int semesterId)
        {
            var name = GetTempOutPut(facultyId, semesterId);
            name += ".zip";

            return name;
        }

        public string GetFilePath(string fileName)
        {
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "files");
            return Path.Combine(path, fileName);
        }

        public string GetOutPutDirectory()
        {
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "Output");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return Path.Combine(path, "output.pdf");
        }

        public bool DeleteDocumentByName(string name)
        {
            var documentInDb = _dbContext.ArticleDocuments.FirstOrDefault(d => d.DocumentFile == name);
            if (documentInDb == null)
            {
                return false;
            }

            _dbContext.ArticleDocuments.Remove(documentInDb);
            _dbContext.SaveChanges();
            return true;
        }

        private string GetZipDir()
        {
            return Path.Combine(_webHostEnvironment.WebRootPath, "zips");
        }

        private string GetTempOutPut(int facultyId, int semesterId)
        {
            string tempFileName;
            var facultyInDb = _dbContext.Faculties.FirstOrDefault(f => f.Id == facultyId);
            var semesterInDb = _dbContext.Semesters.FirstOrDefault(s => s.Id == semesterId);
            var dateTime = GetDateTime("HH-mm MM-dd-yyyy");
            if (facultyInDb == null || semesterInDb == null)
            {
                tempFileName = "Unknown-Faculty" + " " + "Unknown-Semester" + " (" + dateTime + ")";
            }
            else
            {
                tempFileName = facultyInDb.FacultyName + " " + semesterInDb.SemesterName + " (" + dateTime + ")";
            }

            return tempFileName;
        }

        private static string GetDateTime(string format)
        {
            return DateTime.Now.ToString(format);
        }
    }
}