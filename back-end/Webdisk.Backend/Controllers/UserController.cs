using System;
using System.Text;
using System.Web.Http;
using Webdisk.Backend.Helpers;
using Webdisk.Backend.Models;
using Webdisk.Backend.Systems;

namespace Webdisk.Backend.Controllers
{
    public class UserController : ApiController
    {
        [HttpGet]
        [HTTPBasicAuthorize]
        public User Login()
        {
            var r = new User()
            {
                Username = this.CurrentUser().Username,
                Usage = this.CurrentUser().Usage,
                Files = this.CurrentUser().GetFolder(this.CurrentUser().Files.Id)
            };
            return r;
        }

        [HttpGet]
        public User Register()
        {
            if (string.IsNullOrEmpty(Request.Headers.Authorization.Parameter))
            {
                return null;
            }

            var strlist = Encoding.Default.GetString(Convert.FromBase64String(Request.Headers.Authorization.Parameter)).Split(':');
            var name = strlist[0];
            var pass = strlist[1];

            var u = UserHelper.Register(name, pass);
            return Login();
        }
    }
}
