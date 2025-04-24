using System;

namespace api.Common.Models;

public class ApiResponse
{
	public bool Success { get; set; } = true;
	public string Message { get; set; } = string.Empty;
	public object? Data { get; set; } = null;
	public int StatusCode { get; set; } = 200;


	public static ApiResponse SuccessResponse(
	  object? data = null,
	  string message = "Success",
	  int statusCode = 200)
	{
		return new ApiResponse
		{
			Success = true,
			Message = message,
			Data = data,
			StatusCode = statusCode
		};
	}

	public static ApiResponse ErrorResponse(
	  string message = "An error occurred",
	  object? data = null,
	  int statusCode = 500)
	{
		return new ApiResponse
		{
			Success = false,
			Message = message,
			Data = data,
			StatusCode = statusCode
		};
	}
}
