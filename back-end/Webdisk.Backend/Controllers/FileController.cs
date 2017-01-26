using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Webdisk.Backend.Helpers;
using Webdisk.Backend.Systems;

namespace Webdisk.Backend.Controllers
{
    public class FileController : ApiController
    {
        [HTTPBasicAuthorize]
        public async Task<HttpResponseMessage> Upload()
        {
            // Check whether the POST operation is MultiPart?  
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            // Prepare CustomMultipartFormDataStreamProvider in which our multipart form  
            // data will be loaded.  
            var fileSaveLocation = @"E:\MongoTest\uploadCache";
            var provider = new FileMultipartFormDataStreamProvider(fileSaveLocation);
            var ids = new List<ObjectId>();

            try
            {
                // Read all contents of multipart message into CustomMultipartFormDataStreamProvider.  
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (MultipartFileData file in provider.FileData)
                {
                    //move to GridFS
                    var id = await GridFSHelper.Upload(Path.Combine(fileSaveLocation, file.LocalFileName), true);
                    ids.Add(id);
                    var fileinfo = GridFSHelper.GetFile(id);

                    this.CurrentUser().AddFile(new ObjectId(provider.FormData["folder"]), fileinfo);
                }

                // Send OK Response along with saved file names to the client.  
                return Request.CreateResponse(HttpStatusCode.OK, this.CurrentUser().GetFolder(new ObjectId(provider.FormData["folder"])));
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HTTPBasicAuthorize]
        [HttpGet]
        public HttpResponseMessage Download(string objectId)
        {
            try
            {
                var file = this.CurrentUser().GetFile(new ObjectId(objectId));
                var stream = GridFSHelper.Download(file.Id);
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentLength = stream.FileInfo.Length;
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = stream.FileInfo.Filename,
                    Size = stream.FileInfo.Length
                };
                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
        }

        [HttpGet]
        [HTTPBasicAuthorize]
        public Models.FileInfo GetFolder(string objectId = "")
        {
            try
            {
                return this.CurrentUser().GetFolder(new ObjectId(objectId));
            }
            catch
            {
                return this.CurrentUser().GetFolder(this.CurrentUser().Files.Id);
            }
        }

        [HttpGet]
        [HTTPBasicAuthorize]
        public Models.FileInfo Rename(string objectId, string newFilename)
        {
            this.CurrentUser().Rename(new ObjectId(objectId), newFilename);
            return this.CurrentUser().GetParentFolder(new ObjectId(objectId));
        }

        [HttpGet]
        [HTTPBasicAuthorize]
        public Models.FileInfo Delete(string objectId)
        {
            var folder = this.CurrentUser().GetParentFolder(new ObjectId(objectId));
            this.CurrentUser().Delete(new ObjectId(objectId));
            folder = this.CurrentUser().GetFolder(folder.Id);
            return folder;
        }

        [HttpGet]
        [HTTPBasicAuthorize]
        public Models.FileInfo CreateFolder(string objectId, string name)
        {
            Models.FileInfo tryf = null;
            try
            {
                tryf = this.CurrentUser().GetFolder(new ObjectId(objectId));
            }
            catch
            {

            }
            if (tryf == null)
            {
                this.CurrentUser().CreateFolder(name);
                return this.CurrentUser().GetFolder(this.CurrentUser().Files.Id);
            }

            else
            {
                var parentId = new ObjectId(objectId);
                this.CurrentUser().CreateFolder(parentId, name);
                return this.CurrentUser().GetFolder(parentId);
            }
        }

        [HttpGet]
        [HTTPBasicAuthorize]
        public List<Models.FileInfo> Search(string keywords = "",int limit = 100, int skip = 0)
        {
            if (keywords == null) keywords = string.Empty;
            return this.CurrentUser().Search(keywords, limit, skip);
        }

    }
}
