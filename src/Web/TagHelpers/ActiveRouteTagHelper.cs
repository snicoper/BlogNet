using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;

/**
 * https://gist.github.com/oguzhaneren/2953c9d81e296269fa3fce287fe542a6"
 * https://gist.github.com/enohka/0d198ed758cd5b048d9d48dd61830f3c"
 */
namespace Web.TagHelpers
{
    [HtmlTargetElement(Attributes = ControllersAttributeName)]
    [HtmlTargetElement(Attributes = ActionsAttributeName)]
    [HtmlTargetElement(Attributes = RouteAttributeName)]
    [HtmlTargetElement(Attributes = ClassAttributeName)]
    public class ActiveRouteTagHelper : TagHelper
    {
        private const string ActionsAttributeName = "asp-active-actions";
        private const string ControllersAttributeName = "asp-active-controllers";
        private const string ClassAttributeName = "asp-active-class";
        private const string RouteAttributeName = "asp-active-route";

        [HtmlAttributeName(ControllersAttributeName)]
        public string Controllers { get; set; }

        [HtmlAttributeName(ActionsAttributeName)]
        public string Actions { get; set; }

        [HtmlAttributeName(RouteAttributeName)]
        public string Route { get; set; }

        [HtmlAttributeName(ClassAttributeName)]
        public string Class { get; set; } = "active";

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var routeValues = ViewContext.RouteData.Values;
            var currentAction = routeValues["action"].ToString();
            var currentController = routeValues["controller"].ToString();

            if (string.IsNullOrEmpty(Actions))
            {
                Actions = currentAction;
            }

            if (string.IsNullOrEmpty(Controllers))
            {
                Controllers = currentController;
            }

            var acceptedActions = Actions.Trim().Split(',').Distinct().ToArray();
            var acceptedControllers = Controllers.Trim().Split(',').Distinct().ToArray();

            var lowerCaseComparer = new LowerCaseComparer();

            if (acceptedActions.Contains(currentAction, lowerCaseComparer)
                && acceptedControllers.Contains(currentController, lowerCaseComparer))
            {
                _setAttribute(output, "class", Class);
            }

            base.Process(context, output);
        }

        private void _setAttribute(TagHelperOutput output, string attributeName, string value, bool merge = true)
        {
            var val = value;
            if (output.Attributes.TryGetAttribute(attributeName, out TagHelperAttribute attribute))
            {
                if (merge)
                {
                    val = $"{attribute.Value} {value}";
                }
            }
            output.Attributes.SetAttribute(attributeName, val);
        }
    }

    public class LowerCaseComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return x.ToLowerInvariant().Equals(y.ToLowerInvariant());
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}
