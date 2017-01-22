using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using Webdisk.Backend.Helpers;

namespace Webdisk.Backend.Models
{
    /// <summary>
    /// 元信息
    /// </summary>
    public class FileMetadata
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// 文件大小, 单位 Byte
        /// </summary>
        public long Size;

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadTime;
    }
    /// <summary>
    /// 文件
    /// </summary>
    public class FileInfo
    {
        /// <summary>
        /// MongoDB Object ID
        /// </summary>
        public ObjectId Id;

        //for folders
        /// <summary>
        /// 是否为文件夹
        /// </summary>
        public bool IsFolder = false;
        /// <summary>
        /// 文件列表
        /// </summary>
        public List<FileInfo> ChildFiles { get; set; } = new List<FileInfo>();

        /// <summary>
        /// 所在文件夹 ID
        /// </summary>
        public ObjectId Parent;

        /// <summary>
        /// 文件元信息
        /// </summary>
        public FileMetadata Metadata;

        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="fi">GridFS 文件信息</param>
        public void AddFile(GridFSFileInfo fi)
        {
            if (!IsFolder) throw new Exception("This is not a folder.");
            ChildFiles.Add(FileHelper.CreateFile(fi, Id));
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="name">文件夹名</param>
        /// <returns>文件信息</returns>
        public FileInfo CreateFolder(string name)
        {
            var folder = new FileInfo()
            {
                Id = new ObjectId(Guid.NewGuid().ToString("n")),
                IsFolder = true,
                Parent = Id,
                Metadata = new FileMetadata()
                {
                    Name = name,
                    UploadTime = DateTime.Now
                }
            };
            ChildFiles.Add(folder);
            return folder;
        }

        public FileInfo CreateNonChildFileInfo()
        {
            var f = new FileInfo()
            {
                Id = Id,
                IsFolder = IsFolder,
                Metadata = Metadata,
                Parent = Parent
            };
            return f;
        }
    }
}