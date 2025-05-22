using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VNI.Core.Services
{
    public class StorageService : BaseService
    {
        private StorageRepository _repo;
        public StorageService(IWorkingContext context) : base(context)
        {
            _repo = new StorageRepository();
        }

        public Result<Stream> CreateUploadStream(StorageUploadItem item)
        {
            return _repo.CreateUploadStream(item);
        }

        public Result<StorageDownloadItem> Download(string id, string category)
        {
            return _repo.Download(id, category);
        }
    }
}
