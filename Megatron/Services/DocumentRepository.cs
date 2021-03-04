using System;
using System.Collections.Generic;
using System.IO;
using Megatron.Data;
using Megatron.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Megatron.Services
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _dbContext;

        public DocumentRepository(IWebHostEnvironment webHostEnvironment, ApplicationDbContext dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _dbContext = dbContext;
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

                    using var fs = System.IO.File.Create(savePath);
                    formFile.CopyTo(fs);
                    fs.Flush();
                    var newDocument = new ArticleDocument{
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
    }
}