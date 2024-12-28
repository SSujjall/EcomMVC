using Microsoft.AspNetCore.Mvc.Razor;

namespace EcomSiteMVC.Utilities
{
    public class CustomViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            return new[]
            {
                "/Web/Views/{1}/{0}.cshtml", // {1} is controller, {0} is action
                "/Web/Views/Shared/{0}.cshtml"
            }.Concat(viewLocations);
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }
    }
}
