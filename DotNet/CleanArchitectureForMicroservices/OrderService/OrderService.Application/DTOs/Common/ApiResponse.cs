namespace OrderService.Application.DTOs.Common;

/// <summary>
/// Generic API response wrapper.
/// </summary>
public class ApiResponse<T>
{
  /// <summary>Indicates if the request was successful.</summary>
  public bool Success { get; set; }

  /// <summary>Response message.</summary>
  public string Message { get; set; } = null!;

  /// <summary>Response data.</summary>
  public T? Data { get; set; }

  /// <summary>Error code (if applicable).</summary>
  public string? ErrorCode { get; set; }

  /// <summary>Timestamp of the response.</summary>
  public DateTime Timestamp { get; set; }

  /// <summary>Creates a successful response with data.</summary>
  public static ApiResponse<T> SuccessResponse(T data, string message = "Operation successful")
  {
    return new ApiResponse<T>
    {
      Success = true,
      Message = message,
      Data = data,
      Timestamp = DateTime.UtcNow
    };
  }

  /// <summary>Creates a failed response with error details.</summary>
  public static ApiResponse<T> ErrorResponse(string message, string errorCode = "ERROR", T? data = default)
  {
    return new ApiResponse<T>
    {
      Success = false,
      Message = message,
      ErrorCode = errorCode,
      Data = data,
      Timestamp = DateTime.UtcNow
    };
  }
}

/// <summary>
/// Generic API response wrapper for non-generic operations.
/// </summary>
public class ApiResponse
{
  /// <summary>Indicates if the request was successful.</summary>
  public bool Success { get; set; }

  /// <summary>Response message.</summary>
  public string Message { get; set; } = null!;

  /// <summary>Error code (if applicable).</summary>
  public string? ErrorCode { get; set; }

  /// <summary>Timestamp of the response.</summary>
  public DateTime Timestamp { get; set; }

  /// <summary>Creates a successful response.</summary>
  public static ApiResponse SuccessResponse(string message = "Operation successful")
  {
    return new ApiResponse
    {
      Success = true,
      Message = message,
      Timestamp = DateTime.UtcNow
    };
  }

  /// <summary>Creates a failed response.</summary>
  public static ApiResponse ErrorResponse(string message, string errorCode = "ERROR")
  {
    return new ApiResponse
    {
      Success = false,
      Message = message,
      ErrorCode = errorCode,
      Timestamp = DateTime.UtcNow
    };
  }
}
