using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VNI.Core.Services
{
    public enum StorageFileStatus
    {
        Deleted = -1,
        Pending = 0,
        Active = 1
    }

    public class StorageUploadItem
    {
        public string Id { get; set; }

        public string Category { get; set; }

        public string FileName { get; set; }

        public StorageFileStatus Status { get; set; }

        public string CreatedUser { get; set; }

        public DateTime CreatedTime { get; set; }

        public long Size { get; set; }
    }    

    public class StorageUploadBinaryItem: StorageUploadItem
    {       
        public byte[] Content { get; set; }
    }

    public class StorageUploadStreamItem : StorageUploadItem
    {
        public Stream Content { get; set; }
    }
    public class StorageUploadBinaryData : StorageUploadItem
    {
        public byte[] Content { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
    }
    public class StorageUploadStreamData : StorageUploadItem
    {
        public Stream Content { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
    }
}
