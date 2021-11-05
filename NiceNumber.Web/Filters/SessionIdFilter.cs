using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NiceNumber.Web.Filters
{
    public class SessionIdFilter:ActionFilterAttribute
    {
        public const string CookieSessionForGameKey = "SessionId";
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Cookies.TryGetValue(CookieSessionForGameKey, out var sessionId))
            {
                sessionId = Guid.NewGuid().ToString();
                context.HttpContext.Response.Cookies.Append(CookieSessionForGameKey, sessionId);
            }

            context.HttpContext.Session.SetString(CookieSessionForGameKey, sessionId);
        }
    }
}