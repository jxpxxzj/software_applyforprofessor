using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using System.Linq;
using Webdisk.Backend.Models;

namespace Webdisk.Backend.Helpers
{
    /// <summary>
    /// 文件辅助类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="fi">GridFS 文件信息</param>
        /// <returns>文件信息</returns>
        public static FileInfo CreateFile(GridFSFileInfo fi, ObjectId parent)
        {
            return new FileInfo()
            {
                Id = fi.Id,
                Metadata = new FileMetadata()
                {
                    Name = fi.Filename,
                    Size = fi.Length,
                    UploadTime = fi.UploadDateTime
                },
                Parent = parent
            };
        }

        /// <summary>
        /// 以递归方式查找文件夹
        /// </summary>
        /// <param name="parent">母文件夹</param>
        /// <param name="id">文件夹 ID</param>
        /// <returns>文件夹信息</returns>
        public static FileInfo FindFolder(FileInfo parent, ObjectId id)
        {
            if (parent.Id == id) return parent;

            var folders = parent.ChildFiles.Where(f => f.IsFolder);
            foreach (var f in folders)
            {
                var value = FindFolder(f, id);
                if (value != null) return value;
            }

            return null;
        }

        /// <summary>
        /// 以递归方式查找文件 
        /// </summary>
        /// <param name="folder">要查找的文件夹</param>
        /// <param name="id">文件 ID</param>
        /// <returns>文件信息</returns>
        public static FileInfo FindFile(FileInfo folder, ObjectId id)
        {
            var files = folder.ChildFiles.Where(f => !f.IsFolder);
            foreach (var p in files)
            {
                if (p.Id == id)
                    return p;
            }
            var folders = folder.ChildFiles.Where(f => f.IsFolder);
            foreach (var p in folders)
            {
                var result = FindFile(p, id);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}