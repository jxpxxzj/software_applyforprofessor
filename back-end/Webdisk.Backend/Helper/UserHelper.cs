using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Text;
using Webdisk.Backend.Models;

namespace Webdisk.Backend.Helpers
{
    /// <summary>
    /// 用户辅助类
    /// </summary>
    public static class UserHelper
    {
        /// <summary>
        /// 用户信息集合
        /// </summary>
        public static IMongoCollection<User> UserCollection => MongoInstance.BasicDatabase.GetCollection<User>("user");

        /// <summary>
        /// 用户信息 LINQ 对象
        /// </summary>
        public static IMongoQueryable<User> UserQueryable => UserCollection.AsQueryable();

        /// <summary>
        /// 从 HTTP Basic Authorization 字符串获取用户
        /// </summary>
        /// <param name="auth">HTTP Basic Authorization 字符串</param>
        /// <returns>用户信息</returns>
        public static User GetByAuth(string auth)
        {
            var strlist = Encoding.Default.GetString(Convert.FromBase64String(auth)).Split(':');
            var name = strlist[0];
            var pass = strlist[1];

            return GetByNameAndPassword(name, pass);
        }

        /// <summary>
        /// 从用户名密码获取信息
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">原始密码</param>
        /// <returns>用户信息</returns>
        public static User GetByNameAndPassword(string username, string password)
        {
            var query = from p in UserQueryable
                        where p.Username == username
                        where p.Password == CryptoHelper.GetMd5String(password)
                        select p;
            var result = query.ToList();
            return result.FirstOrDefault();
        }

        /// <summary>
        /// 查找用户是否存在
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>是否存在</returns>
        public static bool isExisted(string username)
        {
            var query = from p in UserQueryable
                        where p.Username == username
                        select p;
            return query.ToList().Count != 0;
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">原始密码</param>
        /// <returns>用户信息</returns>
        public static User Register(string username, string password)
        {
            if (isExisted(username)) throw new ApplicationException("User is existed.");
            UserCollection.InsertOne(new User()
            {
                Username = username,
                Password = CryptoHelper.GetMd5String(password),
                Usage = 0,
                Files = new FileInfo()
                {
                    Id = new ObjectId(Guid.NewGuid().ToString("n")),
                    IsFolder = true,
                    Metadata = new FileMetadata()
                    {
                        Name = "root",
                        UploadTime = DateTime.Now
                    }
                }
            });

            return GetByNameAndPassword(username, password);
        }
    }
}