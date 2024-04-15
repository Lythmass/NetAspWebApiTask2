using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Reddit.Filters
{
    public class PaginationSizeActionFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Request.Query.TryGetValue("pageSize", out var pageSizeValue);
            if (pageSizeValue == false)
            {
                context.Result = new BadRequestObjectResult("pageSize parameter is missing");
                return;
            }

            if (!int.TryParse(pageSizeValue, out var pageSize))
            {
                context.Result = new BadRequestObjectResult("pageSize must be an integer");
                return;
            }

            if (pageSize > 50)
            {
                context.Result = new BadRequestObjectResult("pageSize must be less than 50");
                return;
            }
        }
    }
}
