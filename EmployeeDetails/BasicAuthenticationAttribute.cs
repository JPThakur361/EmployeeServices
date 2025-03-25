using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EmployeeDetails
{
	public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
	{
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if(actionContext.Request.Headers.Authorization ==null)
            {
                 actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                string authentication = actionContext.Request.Headers.Authorization.Parameter;
                 string decodeAuthenticationTocken =  Encoding.UTF8.GetString( Convert.FromBase64String(authentication));
                 string [] userPassArray  = decodeAuthenticationTocken.Split(':');
                  string username = userPassArray[0];
                string password = userPassArray[1];

                if (EmployeeSecuriy.Login(username, password))
                {
                     Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username),null);
                }
                else {

                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);               }
            }
        }

    }
}