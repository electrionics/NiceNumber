using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NiceNumber.Web.Filters
{
    public class SessionHoldingFilter:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Session.SetString("Something", "Something");
        }
    }
}