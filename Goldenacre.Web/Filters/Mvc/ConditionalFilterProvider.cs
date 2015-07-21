using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Goldenacre.Web.Filters.Mvc
{
    /*
            var conditions = new Func<ControllerContext, ActionDescriptor, object>[] {
                // Ensure all POST actions are automatically 
                // decorated with the ValidateAntiForgeryTokenAttribute.
                ( c, a ) => string.Equals( c.HttpContext.Request.HttpMethod, "POST",
                StringComparison.OrdinalIgnoreCase ) ?
                new ValidateAntiForgeryTokenAttribute() : null};

            var provider = new ConditionalFilterProvider(conditions);

            FilterProviders.Providers.Add(provider);
     */
    public class ConditionalFilterProvider : IFilterProvider
    {
        private readonly IEnumerable<Func<ControllerContext, ActionDescriptor, object>> _conditions;

        public ConditionalFilterProvider(IEnumerable<Func<ControllerContext, ActionDescriptor, object>> conditions)
        {
            _conditions = conditions;
        }

        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return from condition in _conditions
                select condition(controllerContext, actionDescriptor) into filter
                where filter != null
                select new Filter(filter, FilterScope.Global, null);
        }
    }
}