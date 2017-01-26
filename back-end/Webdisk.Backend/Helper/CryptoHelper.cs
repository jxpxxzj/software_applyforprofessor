using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Webdisk.Backend.Helpers
{
    /// <summary>
    /// 加密辅助类
    /// </summary>
    public static class CryptoHelper
    {
        private static MD5 md5Hasher = MD5.Create();
        private static UTF8Encoding utf8Encoding = new UTF8Encoding();
        private static NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

        /// <summary>
        /// 获得 MD5 Hash 后的字符串
        /// </summary>
        /// <param name="instr">原始字符串</param>
        /// <returns>Hash</returns>
        public static string GetMd5String(string instr)
        {
            return GetMd5String(utf8Encoding.GetBytes(instr));
        }

        /// <summary>
        /// 获得 MD5 Hash 后的字节数组
        /// </summary>
        /// <param name="data">原始字节数组</param>
        /// <returns>Hash</returns>
        public static string GetMd5String(byte[] data)
        {
            lock (md5Hasher)
            {
                try
                {
                    // Convert the input string to a byte array and compute the hash.
                    data = md5Hasher.ComputeHash(data);
                }
                catch
                {
                    return "fail";
                }
            }

            char[] str = new char[data.Length * 2];
            for (int i = 0; i < data.Length; i++)
                data[i].ToString("x2", nfi).CopyTo(0, str, i * 2, 2);

            return new string(str);
        }
    }
}