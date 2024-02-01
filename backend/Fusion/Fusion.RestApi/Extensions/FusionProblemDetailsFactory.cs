using System.Diagnostics;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace Fusion.RestApi.Extensions;

/// <summary>
/// 
/// </summary>
public sealed class FusionProblemDetailsFactory : ProblemDetailsFactory
{
    private const string Errors = "Errors";

    private readonly Action<ProblemDetailsContext>? _configure;
    private readonly ApiBehaviorOptions _options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <param name="problemDetailsOptions"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public FusionProblemDetailsFactory(
        IOptions<ApiBehaviorOptions>? options,
        IOptions<ProblemDetailsOptions>? problemDetailsOptions = null)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _configure = problemDetailsOptions?.Value.CustomizeProblemDetails;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="statusCode"></param>
    /// <param name="title"></param>
    /// <param name="type"></param>
    /// <param name="detail"></param>
    /// <param name="instance"></param>
    /// <returns></returns>
    public override ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        statusCode ??= 500;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = type,
            Detail = detail,
            Instance = instance
        };

        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

        return problemDetails;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="modelStateDictionary"></param>
    /// <param name="statusCode"></param>
    /// <param name="title"></param>
    /// <param name="type"></param>
    /// <param name="detail"></param>
    /// <param name="instance"></param>
    /// <returns></returns>
    public override ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext httpContext,
        ModelStateDictionary modelStateDictionary,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        ArgumentNullException.ThrowIfNull(modelStateDictionary);

        statusCode ??= 400;

        var problemDetails = new ValidationProblemDetails(modelStateDictionary)
        {
            Status = statusCode,
            Type = type,
            Detail = detail,
            Instance = instance
        };

        if (title != null)
            // For validation problem details, don't overwrite the default title with null.
            problemDetails.Title = title;

        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

        return problemDetails;
    }

    private void ApplyProblemDetailsDefaults(HttpContext? httpContext, ProblemDetails problemDetails, int statusCode)
    {
        problemDetails.Status ??= statusCode;

        if (_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
        {
            problemDetails.Title ??= clientErrorData.Title;
            problemDetails.Type ??= clientErrorData.Link;
        }

        var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
        if (traceId != null) problemDetails.Extensions["traceId"] = traceId;

        if (httpContext?.Items[Errors] is List<Error> errors)
            problemDetails.Extensions.Add("errorCodes", errors.Select(e => e.Code));

        _configure?.Invoke(new ProblemDetailsContext { HttpContext = httpContext!, ProblemDetails = problemDetails });
    }
}