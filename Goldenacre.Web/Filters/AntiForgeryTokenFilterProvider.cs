using System.Collections.Generic;
using System.Web.Mvc;
using Goldenacre.Extensions;

namespace Goldenacre.Web.Filters
{
    public class AntiForgeryTokenFilterProvider : IFilterProvider
    {
        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var incomingVerb = controllerContext.HttpContext.Request.HttpMethod.ToUpperInvariant();

            var filters = new List<Filter>();
            if (incomingVerb == HttpVerbs.Post.ToString().ToString().ToUpperInvariant())
            {
                filters.Add(new Filter(new ValidateAntiForgeryTokenAttribute(), FilterScope.Global, null));
            }

            return filters;
        }
    }
}