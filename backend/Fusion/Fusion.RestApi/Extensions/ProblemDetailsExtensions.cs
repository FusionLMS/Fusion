using ErrorOr;

namespace Fusion.RestApi.Extensions;

/// <summary>
/// 
/// </summary>
public static class ProblemDetailsExtensions
{
    private const string Errors = "Errors";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="errors"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static IResult Problem(this List<Error> errors, HttpContext context)
    {
        context.Items.Add(Errors, errors);

        if (errors.All(error => error.Type is ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        var firstError = errors[0];

        var statusCode = firstError.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        return Results.Problem(statusCode: statusCode, title: firstError.Description);
    }

    private static IResult ValidationProblem(List<Error> errors)
    {
        var modelState = errors.ToDictionary<Error, string, string[]>(
            error => error.Code, error => [error.Description]);

        return Results.ValidationProblem(modelState);
    }
}