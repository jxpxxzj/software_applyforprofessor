using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Webdisk.Backend.Helper;
using Webdisk.Backend.Helpers;

namespace Webdisk.Backend.Models
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        /// <summary>
        /// MongoDB Object ID
        /// </summary>
        [JsonIgnore]
        public ObjectId _id;

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username;

        /// <summary>
        /// 密码
        /// </summary>
        [JsonIgnore]
        public string Password;

        /// <summary>
        /// 已用空间
        /// </summary>
        public long Usage;

        /// <summary>
        /// 文件列表, 根目录
        /// </summary>
        public FileInfo Files;

        /// <summary>
        /// 在根目录下创建文件夹
        /// </summary>
        /// <param name="name">文件夹名</param>
        /// <returns>根目录信息</returns>
        public FileInfo CreateFolder(string name)
        {
            var result = Files.CreateFolder(name);
            saveUserData();
            return result;
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="parent">母文件夹</param>
        /// <param name="name">文件夹名</param>
        /// <returns></returns>
        public FileInfo CreateFolder(ObjectId parent, string name)
        {
            var folder = FileHelper.FindFolder(Files,parent);
            folder.CreateFolder(name);
            saveUserData();
            return folder;
        }

        /// <summary>
        /// 获得该文件夹下的文件和文件夹列表, 此方法创建了一个深拷贝, 请勿在返回对象上进行操作.
        /// </summary>
        /// <param name="folderId">文件夹 ID</param>
        /// <returns>文件夹信息</returns>
        public FileInfo GetFolder(ObjectId folderId)
        {
            var f = FileHelper.FindFolder(Files, folderId);
            if (f == null) throw new GridFSFileNotFoundException(folderId);
            var fi = new FileInfo()
            {
                IsFolder = true,
                Id = f.Id,
                Metadata = f.Metadata,
                Parent = f.Parent
            };
            foreach (var p in f.ChildFiles)
                fi.ChildFiles.Add(new FileInfo()
                {
                    Id = p.Id,
                    IsFolder = p.IsFolder,
                    Metadata = p.Metadata,
                    Parent = p.Parent
                });

            return fi;
        }

        /// <summary>
        /// 获得文件
        /// </summary>
        /// <param name="objectId">文件 ID</param>
        /// <returns></returns>
        public FileInfo GetFile(ObjectId objectId)
        {
            var f = FileHelper.FindFile(Files, objectId);
            if (f == null) throw new GridFSFileNotFoundException(objectId);
            return f;
        }

        /// <summary>
        /// 获得母文件夹
        /// </summary>
        /// <param name="objectId">文件 ID / 文件夹 ID</param>
        /// <returns></returns>
        public FileInfo GetParentFolder(ObjectId objectId)
        {
            var f = FileHelper.FindFile(Files, objectId) ?? FileHelper.FindFolder(Files, objectId);
            return FileHelper.FindFolder(Files, f.Parent);
        }

        /// <summary>
        /// 向该文件夹添加文件
        /// </summary>
        /// <param name="folderId">文件夹 ID</param>
        /// <param name="fi">GridFS 文件信息</param>
        public void AddFile(ObjectId folderId, GridFSFileInfo fi)
        {
            var folder = FileHelper.FindFolder(Files, folderId);
            if (folder == null) throw new Exception("This folder is not existed");
            folder.AddFile(fi);
            Usage += fi.Length;
            saveUserData();
        }

        /// <summary>
        /// 重命名文件或文件夹
        /// </summary>
        /// <param name="objectId">文件 ID / 文件夹 ID</param>
        /// <param name="newFilename">新文件名</param>
        public void Rename(ObjectId objectId, string newFilename)
        {
            var f = GridFSHelper.GetFile(objectId);
            if (f != null) // 是文件
            {
                GridFSHelper.Rename(objectId, newFilename);
                var f2 = GetFile(objectId);
                f2.Metadata.Name = newFilename;
                saveUserData();
                return;
            }

            var folder = GetFolder(objectId);
            if (folder != null)
            {
                folder.Metadata.Name = newFilename;
                saveUserData();
            }
        }

        /// <summary>
        /// 删除文件或文件夹
        /// </summary>
        /// <param name="objectId">文件 ID / 文件夹 ID</param>
        public void Delete(ObjectId objectId)
        {
            var f = GridFSHelper.GetFile(objectId);
            if (f != null)
            {
                GridFSHelper.Delete(objectId);
                var parent = GetParentFolder(f.Id);
                parent.ChildFiles.Remove(parent.ChildFiles.Where(item => item.Id == f.Id).FirstOrDefault());
                saveUserData();
                return;
            }

            var folder = GetFolder(objectId);
            if (folder != null)
            {
                if (folder.Id == Files.Id)
                    return;

                foreach (var fi in folder.ChildFiles)
                {
                    GridFSHelper.Delete(fi.Id);
                }
                var parent = FileHelper.FindFolder(Files, folder.Parent);
                parent.ChildFiles.Remove(parent.ChildFiles.Where(item => item.Id == folder.Id).FirstOrDefault());
                saveUserData();
            }
        }

        /// <summary>
        /// 搜索文件
        /// </summary>
        /// <param name="keywords">关键词</param>
        /// <param name="limit">结果限制</param>
        /// <param name="skip">跳过结果数</param>
        /// <returns>符合条件的文件列表</returns>
        public List<FileInfo> Search(string keywords, int limit, int skip)
        {
            var result = FileHelper.FindFile(Files, f => TextHelper.IsMatch(f.Metadata.Name,keywords));
            return result.GetRange(skip, limit > result.Count ? result.Count : limit);
        }

        /// <summary>
        /// 保存用户数据
        /// </summary>
        private void saveUserData()
        {
            var filter = Builders<User>.Filter.Eq("Username", Username);
            var update = Builders<User>.Update.Set("Files", Files).Set("Usage", Usage).Set("Password", Password);
            UserHelper.UserCollection.UpdateOne(filter, update);
        }
    }
}