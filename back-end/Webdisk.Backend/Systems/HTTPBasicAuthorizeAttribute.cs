using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Webdisk.Backend.Helpers;

namespace Webdisk.Backend.Systems
{
    public class HTTPBasicAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization != null)
            {
                if (UserHelper.GetByAuth(actionContext.Request.Headers.Authorization.Parameter) != null)
                {
                    IsAuthorized(actionContext);
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
            else
            {
                HandleUnauthorizedRequest(actionContext);
            }
        }
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            var challengeMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            challengeMessage.Headers.Add("WWW-Authenticate", "Basic");
            throw new HttpResponseException(challengeMessage);
        }
    }
}