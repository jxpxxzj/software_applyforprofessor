using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Webdisk.Backend.Helpers
{
    /// <summary>
    /// MongoDB 实例
    /// </summary>
    public static class MongoInstance
    {
        static MongoClient _client;
        static IMongoDatabase _gridFSDatabase;
        static IMongoDatabase _basicDatabase;
        static GridFSBucket _bucket;

        /// <summary>
        /// 连接字符串
        /// </summary>
        static readonly string _connectionString = "mongodb://localhost:27017";

        /// <summary>
        /// GridFS 数据库名
        /// </summary>
        static readonly string _gridFSDatabaseName = "gridfs";

        /// <summary>
        /// 基本数据库名
        /// </summary>
        static readonly string _basicDatabaseName = "data";

        /// <summary>
        /// MongoDB Client
        /// </summary>
        /// <seealso cref="https://mongodb.github.io/mongo-csharp-driver/2.2/getting_started/quick_tour/"/>
        public static MongoClient Client
        {
            get
            {
                if (_client == null)
                    _client = new MongoClient(_connectionString);
                return _client;

            }
        }

        /// <summary>
        /// GridFS 所在的的数据库
        /// </summary>
        /// <seealso cref="https://mongodb.github.io/mongo-csharp-driver/2.2/reference/gridfs/gettingstarted/"/>
        public static IMongoDatabase GridFSDatabase
        {
            get
            {
                if (_gridFSDatabase == null)
                    _gridFSDatabase = Client.GetDatabase(_gridFSDatabaseName);
                return _gridFSDatabase;
            }
        }
        /// <summary>
        /// GridFS Bucket
        /// </summary>
        /// <seealso cref="https://mongodb.github.io/mongo-csharp-driver/2.2/reference/gridfs/gettingstarted/"/>
        public static GridFSBucket Bucket
        {
            get
            {
                if (_bucket == null)
                    _bucket = new GridFSBucket(GridFSDatabase);
                return _bucket;
            }
        }

        /// <summary>
        /// 基本数据库
        /// </summary>
        public static IMongoDatabase BasicDatabase
        {
            get
            {
                if (_basicDatabase == null)
                    _basicDatabase = Client.GetDatabase(_basicDatabaseName);
                return _basicDatabase;
            }
        }
    }
}