namespace src.DTOs;

public class Result 
{
    public string Message { get; set; }
    public object Data { get; set; }
    public bool Success { get; set; }

    public static Result SucessResult(object data, string message = "")
    {
        return new Result { Success = true, Message = message, Data = data };
    }

    public static Result BadResult(string message = "")
    {
        return new Result { Success = false, Message = message };
    }
}

