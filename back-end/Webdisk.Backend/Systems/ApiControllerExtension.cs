using System.Web.Http;
using Webdisk.Backend.Helpers;
using Webdisk.Backend.Models;

namespace Webdisk.Backend.Systems
{
    public static class ApiControllerExtension
    {
        /// <summary>
        /// 当前请求的用户
        /// </summary>
        /// <param name="controller">Controller</param>
        /// <returns>用户信息</returns>
        public static User CurrentUser(this ApiController controller) => UserHelper.GetByAuth(controller.Request.Headers.Authorization.Parameter);
    }
}