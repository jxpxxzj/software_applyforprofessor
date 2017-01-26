using System.Web.Http;
using Webdisk.Backend.Helpers;
using Webdisk.Backend.Models;

namespace Webdisk.Backend.Controllers
{
#if DEBUG
    public class TestController : ApiController
    {
        [HttpGet]
        public FileInfo RunTest()
        {
            var user = UserHelper.Register("Parry", "123456");
            user.CreateFolder("FolderA");
            user.CreateFolder("FolderB");
            return user.GetFolder(user.Files.Id);
        }
    }
#endif
}
