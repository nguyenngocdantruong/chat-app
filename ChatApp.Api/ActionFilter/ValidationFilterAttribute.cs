using ChatApp.Api.Response;
using ChatApp.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ChatApp.Api.ActionFilter
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Nếu model state không hợp lệ
            if (!context.ModelState.IsValid)
            {
                // Lấy lỗi và tạo response DTO
                var errorsInModelState = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList());

                var response = new
                {
                    CreatedAt = DateTime.UtcNow,
                    Code = 400,
                    Message = "Validation failed",
                    Success = false,
                    Errors = errorsInModelState
                };

                // Gán kết quả là BadRequest, action sẽ không được thực thi
                context.Result = new BadRequestObjectResult(response);
            }
        }

        // Không cần làm gì ở đây
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
