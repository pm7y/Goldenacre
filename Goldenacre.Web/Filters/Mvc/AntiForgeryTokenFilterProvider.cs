using System.Collections.Generic;
using System.Web.Mvc;

namespace Goldenacre.Web.Filters.Mvc
{
    public class AntiForgeryTokenFilterProvider : IFilterProvider
    {
        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var incomingVerb = controllerContext.HttpContext.Request.HttpMethod.ToUpperInvariant();

            var filters = new List<Filter>();
            if (incomingVerb == HttpVerbs.Post.ToString().ToUpperInvariant())
            {
                filters.Add(new Filter(new ValidateAntiForgeryTokenAttribute(), FilterScope.Global, null));
            }

            return filters;
        }
    }
}