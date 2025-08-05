using ChatApp.Application.DTOs.Response;
using ChatApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChatApp.Api.Response
{
    /// <summary>
    /// Lớp helper tĩnh để tạo các response JSON nhất quán cho API.
    /// </summary>
    ///

    public class ResponseDto<T>(int code, string message, bool success, T data) where T: class
    {
        public int Code { get; set; } = code;
        public T Data { get; set; } = data;
        public string Message { get; set; } = message;
        public bool IsSuccess { get; set; } = success;
        public Dictionary<string, List<string>> Errors { get; set; } = [];

    }
    public class ResponseJson
    {
        /// <summary>
        /// Tạo một response dựa trên HTTP status code.
        /// </summary>
        /// <param name="code">Mã trạng thái HTTP.</param>
        /// <param name="data">Dữ liệu tùy chọn để trả về.</param>
        /// <param name="message">Thông điệp tùy chọn.</param>
        /// <returns>Một IActionResult tương ứng với mã trạng thái.</returns>
        public static JsonResult GetByCode(int code, object? data = null, string? message = null)
        {
            switch (code)
            {
                case 200:
                    return Ok(data, message);
                case 201:
                    return Created(data, message);
                case 204:
                    return NoContent();
                case 207:
                    return MultiStatus(data, message);
                case 400:
                    return BadRequest(data, message);
                case 401:
                    return Unauthorized(data, message);
                case 403:
                    return Forbidden(data, message);
                case 404:
                    return NotFound(data, message);
                case 409:
                    return Conflict(data, message);
                case 500:
                    return InternalServerError(data, message);

                default:
                    bool isSuccess = code is >= 200 and < 300;
                    string defaultMessage = message ?? (isSuccess ? "Success" : "An error occurred");
                    var response = new ResponseDto<object>(code, defaultMessage, isSuccess, data);
                    return new JsonResult(response) { StatusCode = code };
            }
        }

        // --- 2xx Success ---

        /// <summary>
        /// Trả về response 200 OK.
        /// </summary>
        public static JsonResult Ok(object? data = null, string? message = null, bool? isSuccess = null)
        {
            string msg = message ?? "OK";
            // Mặc định isSuccess là true cho các response 2xx
            var response = new ResponseDto<object>(200, msg, isSuccess ?? true, data);
            return new JsonResult(response) { StatusCode = 200 };
        }

        /// <summary>
        /// Trả về response 201 Created.
        /// </summary>
        public static JsonResult Created(object? data = null, string? message = null, bool? isSuccess = null)
        {
            string msg = message ?? "Resource created successfully";
            var response = new ResponseDto<object>(201, msg, isSuccess ?? true, data);
            return new JsonResult(response) { StatusCode = 201 };
        }

        /// <summary>
        /// Trả về response 204 No Content. (Không có body)
        /// </summary>
        public static JsonResult NoContent()
        {
            string msg = "No content";
            var response = new ResponseDto<object>(204, msg, true, null);
            return new JsonResult(response) { StatusCode = 204 };
        }

        /// <summary>
        /// Trả về response 207 Multi-Status.
        /// </summary>
        public static JsonResult MultiStatus(object? data = null, string? message = null, bool? isSuccess = null)
        {
            string msg = message ?? "Multi-status response";
            var response = new ResponseDto<object>(207, msg, isSuccess ?? true, data);
            return new JsonResult(response) { StatusCode = 207 };
        }
        // --- 4xx Client Errors ---

        /// <summary>
        /// Trả về response 400 Bad Request.
        /// </summary>
        public static JsonResult BadRequest(object? data = null, string? message = null, bool? isSuccess = null)
        {
            string msg = message ?? "Bad request";
            // Mặc định isSuccess là false cho các response 4xx/5xx
            var response = new ResponseDto<object>(400, msg, isSuccess ?? false, data);
            return new JsonResult(response) { StatusCode = 400 };
        }

        /// <summary>
        /// Trả về response 401 Unauthorized.
        /// </summary>
        public static JsonResult Unauthorized(object? data = null, string? message = null, bool? isSuccess = null)
        {
            string msg = message ?? "Unauthorized";
            var response = new ResponseDto<object>(401, msg, isSuccess ?? false, data);
            return new JsonResult(response) { StatusCode = 401 };
        }

        /// <summary>
        /// Trả về response 403 Forbidden.
        /// </summary>
        public static JsonResult Forbidden(object? data = null, string? message = null, bool? isSuccess = null)
        {
            string msg = message ?? "Forbidden";
            var response = new ResponseDto<object>(403, msg, isSuccess ?? false, data);
            return new JsonResult(response) { StatusCode = 403 };
        }

        /// <summary>
        /// Trả về response 404 Not Found.
        /// </summary>
        public static JsonResult NotFound(object? data = null, string? message = null, bool? isSuccess = null)
        {
            string msg = message ?? "Resource not found";
            var response = new ResponseDto<object>(404, msg, isSuccess ?? false, data);
            return new JsonResult(response) { StatusCode = 404 };
        }

        /// <summary>
        /// Trả về response 409 Conflict.
        /// </summary>
        public static JsonResult Conflict(object? data = null, string? message = null, bool? isSuccess = null)
        {
            string msg = message ?? "A conflict occurred with the resource";
            var response = new ResponseDto<object>(409, msg, isSuccess ?? false, data);
            return new JsonResult(response) { StatusCode = 409 };
        }


        // --- 5xx Server Errors ---

        /// <summary>
        /// Trả về response 500 Internal Server Error.
        /// </summary>
        public static JsonResult InternalServerError(object? data = null, string? message = null, bool? isSuccess = null)
        {
            string msg = message ?? "An unexpected error occurred on the server";
            var response = new ResponseDto<object>(500, msg, isSuccess ?? false, data);
            return new JsonResult(response) { StatusCode = 500 };
        }
    }
}