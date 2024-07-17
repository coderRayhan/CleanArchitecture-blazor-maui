namespace Domain.Shared;
public static class ResultExtensions
{
    /// <summary>
    /// Ensures that the specified predicate returns true, otherwise returns a failure result with the specified error.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="result">The result.</param>
    /// <param name="predicate">The predicate.</param>
    /// <param name="error">The error.</param>
    /// <returns>The success result if the predicate returns true and the result is a success result, otherwise a failure result</returns>
    public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Error error)
    {
        if (result.IsFailure)
            return result;
        return result.IsSuccess && predicate(result.Value) ? result : Result.Failure<T>(error);
    }

    /// <summary>
    /// Maps the result value to a new value based on the specified mapping function
    /// </summary>
    /// <typeparam name="TIn">The input result type</typeparam>
    /// <typeparam name="TOut">The output result type</typeparam>
    /// <param name="result">The result</param>
    /// <param name="predicate">The predicate.</param>
    /// <returns>The success result with the mapped value if the current result is a success result, otherwise a failure result</returns>
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> predicate)
        => result.IsSuccess ? predicate(result.Value) : Result.Failure<TOut>(result.Error);

    /// <summary>
    /// Matches the success status of the result to the corresponding functions.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="resultTask">The result task.</param>
    /// <param name="onSuccess">the on-success function.</param>
    /// <param name="onFailure">The on-failure function.</param>
    /// <returns>The result of the onsuccess function if the result is a success result, otherwise the result of the failure result.</returns>
    public static async Task<T> Match<T>(this Task<Result> resultTask, Func<T> onSuccess, Func<T> onFailure)
    {
        Result result = await resultTask;

        return result.IsSuccess ? onSuccess() : onFailure();
    }

    public static T Match<T>(this Result result, Func<T> onSuccess, Func<T> onFailure)
        => result.IsSuccess ? onSuccess() : onFailure();
}