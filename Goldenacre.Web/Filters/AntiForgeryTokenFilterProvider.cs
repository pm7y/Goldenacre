using System.Collections.Generic;
using System.Web.Mvc;
using Goldenacre.Extensions;

namespace Goldenacre.Web.Filters
{
    public class AntiForgeryTokenFilterProvider : IFilterProvider
    {
        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var incomingVerb = controllerContext.HttpContext.Request.HttpMethod;

            return new List<Filter>().AddElementIf(incomingVerb.EqualsCI(HttpVerbs.Post.ToString()),
                new Filter(new ValidateAntiForgeryTokenAttribute(), FilterScope.Global, null));
        }
    }
}