using Microsoft.AspNetCore.Mvc;

namespace NiceNumber.Web
{
    public class ApiRouteAttribute:RouteAttribute
    {
        public ApiRouteAttribute(string template) : base("Api/" + template)
        {
        }
    }
}