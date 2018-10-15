using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Web.Core.Routing.Constraints
{
    public class SlugConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext,
            IRouter route,
            string routeKey,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            var pattern = @"[a-z0-9\-]?";
            var regex = new Regex(pattern);
            var result = regex.IsMatch(values[routeKey]?.ToString());
            return result;
        }
    }
}
