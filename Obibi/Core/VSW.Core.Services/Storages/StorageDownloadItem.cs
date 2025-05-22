using System;
using System.Collections.Generic;
using System.Text;

namespace VNI.Core.Services
{
    public class StorageDownloadItem
    {
        public string Id { get; set; }

        public string Category { get; set; }

        public byte[] Content { get; set; }

        public string FileName { get; set; }

        public StorageFileStatus Status { get; set; }

        public string CreatedUser { get; set; }

        public DateTime CreatedTime { get; set; }
    }
    public class StorageDownloadData
    {
        public string Id { get; set; }

        public string Category { get; set; }

        public byte[] Content { get; set; }

        public DateTime CreatedTime { get; set; }

        public Dictionary<string, object> Metadata { get; set; }
    }
}
