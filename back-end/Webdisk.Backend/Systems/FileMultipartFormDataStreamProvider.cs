using System.Net.Http;
using System.Net.Http.Headers;

namespace Webdisk.Backend.Systems
{
    /// <seealso cref="http://blog.csdn.net/greystar/article/details/44562715"/>
    public class FileMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public FileMultipartFormDataStreamProvider(string path) : base(path) { }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            return headers.ContentDisposition.FileName.Replace("\"", string.Empty);
        }
    }
}