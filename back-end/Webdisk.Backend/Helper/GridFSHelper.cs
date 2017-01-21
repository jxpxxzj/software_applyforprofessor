using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Webdisk.Backend.Helpers
{
    /// <summary>
    /// GridFS 辅助类
    /// </summary>
    public static class GridFSHelper
    {
        /// <summary>
        /// 将文件上传至 GridFS.
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="data">数据</param>
        /// <returns>文件 ID</returns>
        /// <seealso cref="https://mongodb.github.io/mongo-csharp-driver/2.2/reference/gridfs/uploadingfiles/"/>
        public async static Task<ObjectId> Upload(string filename, byte[] data)
        {
            var id = await MongoInstance.Bucket.UploadFromBytesAsync(filename, data);
            return id;
        }
        /// <summary>
        /// 将文件上传至 GridFS.
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <returns>文件 ID</returns>
        /// <seealso cref="https://mongodb.github.io/mongo-csharp-driver/2.2/reference/gridfs/uploadingfiles/"/>
        public async static Task<ObjectId> Upload(FileStream stream)
        {
            var filename = Path.GetFileName(stream.Name);
            var id = await MongoInstance.Bucket.UploadFromStreamAsync(filename, stream);
            return id;
        }
        /// <summary>
        /// 将文件上传至 GridFS.
        /// </summary>
        /// <param name="path">文件在本机上的路径</param>
        /// <param name="delete">上传后是否删除源文件</param>
        /// <returns>文件 ID</returns>
        /// <seealso cref="https://mongodb.github.io/mongo-csharp-driver/2.2/reference/gridfs/uploadingfiles/"/>
        public async static Task<ObjectId> Upload(string path, bool delete = false)
        {
            var fs = File.Open(path, FileMode.Open);
            var id = await Upload(fs);
            fs.Close();
            fs.Dispose();
            if (delete)
                File.Delete(path);
            return id;
        }

        /// <summary>
        /// 获得下载文件流
        /// </summary>
        /// <param name="id">文件 ID</param>
        /// <returns>文件流</returns>
        /// <seealso cref="https://mongodb.github.io/mongo-csharp-driver/2.2/reference/gridfs/downloadingfiles/"/>
        public static GridFSDownloadStream Download(ObjectId id)
        {
            var stream = MongoInstance.Bucket.OpenDownloadStream(id);
            return stream;
        }

        /// <summary>
        /// 获得文件信息
        /// </summary>
        /// <param name="id">文件 ID</param>
        /// <returns>GridFS 文件信息</returns>
        /// <seealso cref="https://mongodb.github.io/mongo-csharp-driver/2.2/reference/gridfs/findingfiles/"/>
        public static GridFSFileInfo GetFile(ObjectId id)
        {
            var filter = Builders<GridFSFileInfo>.Filter.And(
                Builders<GridFSFileInfo>.Filter.Eq("_id", id));
            var cursor = MongoInstance.Bucket.Find(filter);
            var result = (cursor.ToList()).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="objectId">文件 ID</param>
        /// <param name="newFilename">新文件名</param>
        /// <seealso cref="https://mongodb.github.io/mongo-csharp-driver/2.2/reference/gridfs/deletingandrenamingfiles/"/>
        public static void Rename(ObjectId objectId, string newFilename)
        {
            MongoInstance.Bucket.Rename(objectId, newFilename);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="objectId">文件 ID</param>
        /// <seealso cref="https://mongodb.github.io/mongo-csharp-driver/2.2/reference/gridfs/deletingandrenamingfiles/"/>
        public static void Delete(ObjectId objectId)
        {
            MongoInstance.Bucket.Delete(objectId);
        }
    }
}