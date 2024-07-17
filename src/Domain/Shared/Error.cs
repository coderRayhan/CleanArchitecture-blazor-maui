namespace Domain.Shared;
public record Error(string Code, string Message, ErrorType ErrorType)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null", ErrorType.Failure);

    public static implicit operator string(Error error) => error.Code ?? string.Empty;

    public static Error Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);

    public static Error Validation(string code, string description) =>
        new(code, description, ErrorType.Validation);

    public static Error Unauthorized(string code, string description) =>
        new(code, description, ErrorType.Unauthorized);

    public static Error Forbidden(string code, string description) =>
        new(code, description, ErrorType.Forbidden);

    public static Error NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    public static Error Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);
}

public enum ErrorType
{
    Validation,
    Failure,
    Unauthorized,
    Forbidden,
    NotFound,
    Conflict
}
