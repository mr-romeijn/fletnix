using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace fletnix.Attributes
{
    public class ValidateAjaxAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var isAjax = filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (!isAjax) return;

            var modelState = ((Controller)filterContext.Controller).ModelState;

            if (!modelState.IsValid) {

                var errorModel =
                    from x in modelState.Keys
                    where modelState[x].Errors.Count > 0
                    select new
                    {
                        key = x,
                        errors = modelState[x].Errors.
                            Select(y => y.ErrorMessage).
                            ToArray()
                    };

                filterContext.Result = new JsonResult(new {Data = errorModel});

                filterContext.HttpContext.Response.StatusCode =
                    (int) HttpStatusCode.BadRequest;
            }
        }
    }
}