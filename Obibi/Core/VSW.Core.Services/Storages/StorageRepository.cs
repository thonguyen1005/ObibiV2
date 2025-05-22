using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;

namespace VNI.Core.Services
{
    public class StorageRepository
    {
        public const string META_CREATED_USER = "CreatedUser";
        public const string META_FILE_NAME = "FileName";
        public const string META_STATUS = "Status";

        public const int DefaultChunkSizeBytes = 1048576;

        protected StoragesSetting Settings { get; set; }

        protected ConnectionItem Datasource { get; set; }

        public StorageRepository()
        {
            Settings = CoreService.GetConfigWithSection<StoragesSetting>();
            if (Settings.ChunkSizeBytes == 0)
            {
                Settings.ChunkSizeBytes = DefaultChunkSizeBytes;
            }

            var dsSetting = DatasourceExtensions.GetMongoDbSetting();
            Datasource = dsSetting.Item2.FirstOrDefault();
        }


        private Dictionary<string, GridFSBucket> _dicBucket = new Dictionary<string, GridFSBucket>();
        private GridFSBucket GetBucket(string category)
        {
            if (_dicBucket.ContainsKey(category))
            {
                return _dicBucket[category];
            }

            var _bucket = MongoDbHelper.GetGridFSBucket(Settings.DatabaseName, category, Settings.ChunkSizeBytes, Datasource.Value);
            _dicBucket.Add(category, _bucket);
            return _bucket;
        }

        public Result<string> Upload(StorageUploadBinaryItem item)
        {
            if (item.Id.IsEmpty())
            {
                item.Id = StringExtensions.NewId();
            }

            if (item.Category.IsEmpty())
            {
                throw new Exception("Category of StorageUploadItem cann't empty");
            }

            var _bucket = GetBucket(item.Category);
            var meta = ToMetadata(item);
            var opt = new GridFSUploadOptions
            {
                Metadata = meta == null ? null : new BsonDocument(meta)
            };

            var id = _bucket.UploadFromBytes(item.Id, item.Content, opt);
            return id != null ? Result.Ok(item.Id) : Result.ErrorWithData<string>("Error when upload file to Mongodb", null);
        }

        public Result<string> Upload(StorageUploadStreamItem item)
        {
            if (item.Id.IsEmpty())
            {
                item.Id = StringExtensions.NewId();
            }

            if (item.Category.IsEmpty())
            {
                throw new Exception("Category of StorageUploadItem cann't empty");
            }

            var _bucket = GetBucket(item.Category);
            var meta = ToMetadata(item);
            var opt = new GridFSUploadOptions
            {
                Metadata = meta == null ? null : new BsonDocument(meta)
            };

            var id = _bucket.UploadFromStream(item.Id, item.Content, opt);
            return id != null ? Result.Ok(item.Id) : Result.ErrorWithData<string>("Error when upload file to Mongodb", null);
        }

        public Result<Stream> CreateUploadStream(StorageUploadItem item)
        {
            if (item.Id.IsEmpty())
            {
                item.Id = StringExtensions.NewId();
            }

            if (item.Category.IsEmpty())
            {
                throw new Exception("Category of StorageUploadItem cann't empty");
            }

            var _bucket = GetBucket(item.Category);
            var meta = ToMetadata(item);
            var opt = new GridFSUploadOptions
            {
                Metadata = meta == null ? null : new BsonDocument(meta)
            };

            var stream = _bucket.OpenUploadStream(item.Id, opt) as Stream;
            return stream != null ? Result.Ok(stream) : Result.ErrorWithData<Stream>("Error when create upload stream file to Mongodb", null);
        }

        public Result UpdateStatus(string id, string category, StorageFileStatus status)
        {
            var filter = Builders<GridFSFileInfo>.Filter.And(
                    Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, id)
                );

            var update = Builders<GridFSFileInfo>.Update.Set(
                    x => x.Metadata[META_STATUS], status
                );


            var collection = MongoDbHelper.GetCollection(Settings.DatabaseName, category, Datasource.Value);
            var r = collection.UpdateOne(filter, update);
            if (!r.IsAcknowledged || !r.IsModifiedCountAvailable)
            {
                return Result.Error("Error when update status of file storage");
            }

            return Result.Ok();
        }

        public Result<StorageDownloadItem> Download(string id, string category)
        {
            try
            {
                var _bucket = GetBucket(category);
                var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, id);
                var sortDef = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);
                var optFind = new GridFSFindOptions
                {
                    Limit = 1,
                    Sort = sortDef
                };

                GridFSFileInfo fileInfo = null;
                using (var cursor = _bucket.Find(filter, optFind))
                {
                    fileInfo = cursor.FirstOrDefault();
                }

                if (fileInfo == null)
                {
                    return Result.Ok<StorageDownloadItem>(null);
                }

                var meta = fileInfo.Metadata == null ? new Dictionary<string, object>() : fileInfo.Metadata.ToDictionary();

                var rs = new StorageDownloadItem
                {
                    Id = id,
                    Category = category,
                    CreatedTime = fileInfo.UploadDateTime,
                    CreatedUser = meta.ContainsKey(META_CREATED_USER) ? (meta[META_CREATED_USER] as string) : "",
                    FileName = meta.ContainsKey(META_FILE_NAME) ? (meta[META_FILE_NAME] as string) : "",
                    Status = meta.ContainsKey(META_STATUS) ? (StorageFileStatus)meta[META_STATUS] : StorageFileStatus.Pending,
                };

                var opt = new GridFSDownloadByNameOptions
                {
                    Revision = -1 //Newest Version Of File
                };
                var r = _bucket.DownloadAsBytesByName(id, opt);
                rs.Content = r;
                return Result.Ok(rs);
            }
            catch (GridFSFileNotFoundException)
            {
                return null;
            }
        }

        public Result<string> Upload(StorageUploadBinaryData item)
        {
            if (item.Id.IsEmpty())
            {
                item.Id = StringExtensions.NewId();
            }

            if (item.Category.IsEmpty())
            {
                throw new Exception("Category of StorageUploadItem cann't empty");
            }

            var _bucket = GetBucket(item.Category);
            var meta = item.Metadata;
            meta.Add(META_STATUS, item.Status);
            var opt = new GridFSUploadOptions
            {
                Metadata = meta == null ? null : new BsonDocument(meta)
            };

            var id = _bucket.UploadFromBytes(item.Id, item.Content, opt);
            return id != null ? Result.Ok(item.Id) : Result.ErrorWithData<string>("Error when upload file to Mongodb", null);
        }

        public Result<string> Upload(StorageUploadStreamData item)
        {
            if (item.Id.IsEmpty())
            {
                item.Id = StringExtensions.NewId();
            }

            if (item.Category.IsEmpty())
            {
                throw new Exception("Category of StorageUploadItem cann't empty");
            }

            var _bucket = GetBucket(item.Category);
            var meta = item.Metadata;
            meta.Add(META_STATUS, item.Status);
            var opt = new GridFSUploadOptions
            {
                Metadata = meta == null ? null : new BsonDocument(meta)
            };

            var id = _bucket.UploadFromStream(item.Id, item.Content, opt);
            return id != null ? Result.Ok(item.Id) : Result.ErrorWithData<string>("Error when upload file to Mongodb", null);
        }

        public Result UpdateMetaData(string id, string category, UpdateDefinition<GridFSFileInfo> update)
        {
            var filter = Builders<GridFSFileInfo>.Filter.And(
                    Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, id)
                );

            var collection = MongoDbHelper.GetCollection(Settings.DatabaseName, category, Datasource.Value);
            var r = collection.UpdateOne(filter, update);
            if (!r.IsAcknowledged || !r.IsModifiedCountAvailable)
            {
                return Result.Error("Error when update status of file storage");
            }

            return Result.Ok();
        }

        public Result<List<StorageDownloadData>> DownloadAllData(string category,string column, string value, Dictionary<string, object> valuePairs = null)
        {
            try
            {
                var _bucket = GetBucket(category);
                var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Metadata[column], value);
                if(valuePairs != null)
                {
                    foreach (var item in valuePairs)
                    {
                        filter = filter & (Builders<GridFSFileInfo>.Filter.Eq(x => x.Metadata[item.Key], item.Value));
                    }
                }
                filter = filter & (Builders<GridFSFileInfo>.Filter.Eq(x => x.Metadata[META_STATUS], StorageFileStatus.Active));
                var sortDef = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);
                var optFind = new GridFSFindOptions
                {
                    Sort = sortDef
                };

                List<GridFSFileInfo> listFileInfo = null;
                using (var cursor = _bucket.Find(filter, optFind))
                {
                    listFileInfo = cursor.ToList();
                }

                if (listFileInfo == null)
                {
                    return Result.Ok<List<StorageDownloadData>>(null);
                }
                List<StorageDownloadData> list = new List<StorageDownloadData>();
                for (int i = 0; i < listFileInfo.Count; i++)
                {
                    var meta = listFileInfo[i].Metadata == null ? new Dictionary<string, object>() : listFileInfo[i].Metadata.ToDictionary();
                    var item = new StorageDownloadData
                    {
                        Id = listFileInfo[i].Filename,
                        Category = category,
                        CreatedTime = listFileInfo[i].UploadDateTime,
                        Metadata = meta
                    };
                    var opt = new GridFSDownloadByNameOptions
                    {
                        Revision = -1 //Newest Version Of File
                    };
                    var r = _bucket.DownloadAsBytesByName(item.Id, opt);
                    item.Content = r;
                    list.Add(item);
                }
                return Result.Ok<List<StorageDownloadData>>(list);
            }
            catch (GridFSFileNotFoundException)
            {
                return null;
            }
        }

        private Dictionary<string, object> ToMetadata(StorageUploadItem item)
        {
            var rs = new Dictionary<string, object>();
            rs.Add(META_CREATED_USER, item.CreatedUser);
            rs.Add(META_FILE_NAME, item.FileName);
            rs.Add(META_STATUS, item.Status);
            return rs;
        }
    }
}
