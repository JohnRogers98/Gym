using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static ObjectResult InternalErrorProblem(this ControllerBase controller, String detail) 
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

        public static ObjectResult BadRequestProblem(this ControllerBase controller, String detail)
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

        public static ObjectResult ConflictProblem(this ControllerBase controller, String detail)
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
