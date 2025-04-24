using System;

namespace api.Common.Models;

public class ApiResponse
{
	public bool Success { get; set; } = true;
	public string Message { get; set; } = string.Empty;
	public object? Data { get; set; } = null;


	public static ApiResponse SuccessResponse(
	  object? data = null, string message = "Success")
	{
		return new ApiResponse
		{
			Success = true,
			Message = message,
			Data = data
		};
	}

	public static ApiResponse ErrorResponse(
	  string message = "An error occurred", object? data = null)
	{
		return new ApiResponse
		{
			Success = false,
			Message = message,
			Data = data
		};
	}
}
