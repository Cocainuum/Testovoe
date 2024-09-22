namespace WebService.Exceptions;

public class CommonError(int code, string text) : Exception
{
    public int Code { get; set; } = code;
    public string Text { get; set; } = text;
}