namespace Digi.Base.Response;

public partial class ApiResponse : Exception
{
    public string Message { get; set; }
    public bool IsSuccess { get; set; }
    public DateTime ServerDate { get; set; } = DateTime.UtcNow;
    public Guid ReferenceNumber { get; set; } = Guid.NewGuid();
    
    public ApiResponse(string message = null)
    {
        if (string.IsNullOrEmpty(message))
        {
            IsSuccess = true;
        }
        else
        {
            IsSuccess = false;
            Message = message;
        }
    }
}

public partial class ApiResponse<T>
{
    public T Data { get; set; }
    public string Message { get; set; }
    public bool IsSuccess { get; set; }
    public DateTime ServerDate { get; set; } = DateTime.UtcNow;
    public Guid ReferenceNumber { get; set; } = Guid.NewGuid();


    public ApiResponse(T data)
    {
        IsSuccess = true;
        Data = data;
        Message = "Success";
    }
    
    public ApiResponse(string error)
    {
        IsSuccess = true;
        Data = default;
        Message = error;
    }
}