using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Goldenacre.Web
{
    public class AntiForgeryTokenFilterProvider : System.Web.Mvc.IFilterProvider
    {
        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            List<Filter> result = new List<Filter>();

            string incomingVerb = controllerContext.HttpContext.Request.HttpMethod;

            if (String.Equals(incomingVerb, "POST", StringComparison.OrdinalIgnoreCase))
            {
                result.Add(new Filter(new ValidateAntiForgeryTokenAttribute(), FilterScope.Global, null));
            }

            return result;
        }
    }
}