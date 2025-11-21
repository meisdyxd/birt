namespace Shared.Extensions;

public static class ValidationExtensions
{
    public static T ThrowIfNull<T>(this T value, string paramName)
        where T : class
        => value ?? throw new ArgumentNullException(paramName);

    public static T ThrowIfDefault<T>(this T value, string paramName)
        where T : struct, IEquatable<T>
        => value.Equals(default)
            ? throw new ArgumentException($"{paramName} cannot be default value", paramName)
            : value;

    public static string ThrowIfNullOrWhiteSpace(this string? value, string paramName)
        => string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException($"{paramName} cannot be null or whitespace", paramName)
            : value;

    public static string ThrowIfNullOrEmpty(this string? value, string paramName)
        => string.IsNullOrEmpty(value)
            ? throw new ArgumentException($"{paramName} cannot be null or empty", paramName)
            : value;
}
