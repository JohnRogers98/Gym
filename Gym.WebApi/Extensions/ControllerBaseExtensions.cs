namespace Microsoft.AspNetCore.Mvc;

public static class ControllerBaseExtensions
{
    extension(ControllerBase controller)
    {
        public ObjectResult InternalErrorProblem(String detail)
        {
            var problemDetails = new ProblemDetails
            {
                Type = "about:blank",
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal error",
                Detail = detail,
                Instance = controller.HttpContext.Request.Path,
            };

            return controller.StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
        }

        public ObjectResult BadRequestProblem(String detail)
        {
            var problemDetails = new ProblemDetails
            {
                Type = "about:blank",
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation error",
                Detail = detail,
                Instance = controller.HttpContext.Request.Path,
            };

            return controller.BadRequest(problemDetails);
        }

        public ObjectResult ConflictProblem(String detail)
        {
            var problemDetails = new ProblemDetails
            {
                Type = "about:blank",
                Status = StatusCodes.Status409Conflict,
                Title = "Conflict",
                Detail = detail,
                Instance = controller.HttpContext.Request.Path,
            };

            return controller.Conflict(problemDetails);
        }
    }
   
}
