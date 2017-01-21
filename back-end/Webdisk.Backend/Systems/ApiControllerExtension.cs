using System.Web.Http;
using Webdisk.Backend.Helpers;
using Webdisk.Backend.Models;

namespace Webdisk.Backend.Systems
{
    public static class ApiControllerExtension
    {
        public static User CurrentUser(this ApiController controller) => UserHelper.GetByAuth(controller.Request.Headers.Authorization.Parameter);
    }
}