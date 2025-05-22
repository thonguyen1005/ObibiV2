using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Text;

namespace VNI.Core.Services
{
    public static class MongoDbHelper
    {
        internal static GridFSBucket GetGridFSBucket(string dbName, string bucketName, int chuckSize, string connString)
        {
            var opt = new GridFSBucketOptions
            {
                BucketName = bucketName,
                ChunkSizeBytes = chuckSize,
                WriteConcern = WriteConcern.WMajority,
                ReadPreference = ReadPreference.Secondary
            };
            var database = GetDatabase(dbName, connString);
            var _bucket = new GridFSBucket(database, opt);
            return _bucket;
        }

        internal static IMongoCollection<GridFSFileInfo> GetCollection(string dbName, string bucketName, string connString)
        {
            var database = GetDatabase(dbName, connString);
            var collection = database.GetCollection<GridFSFileInfo>(bucketName + ".files");
            return collection;
        }

        internal static IMongoDatabase GetDatabase(string dbName, string connString)
        {
            var _client = GetClient(connString);
            var db = _client.GetDatabase(dbName);
            return db;
        }

        internal static MongoClient GetClient(string connString)
        {
            return new MongoClient(connString);
        }
    }
}
