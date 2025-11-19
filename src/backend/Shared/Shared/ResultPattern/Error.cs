namespace Shared.ResultPattern;

public sealed record Error
{
    public Error(
        string reason,
        string action,
        string code)
    {
        Reason = reason; 
        Action = action;
        Code = code;
    }

    public string Reason { get; set; }
    public string Action { get; set; }
    public string Code { get; set; }
}
