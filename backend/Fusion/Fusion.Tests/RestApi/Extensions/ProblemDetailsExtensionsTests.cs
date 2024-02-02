using ErrorOr;
using FluentAssertions;
using Fusion.RestApi.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Fusion.Tests.RestApi.Extensions;

public class ProblemDetailsExtensionsTests
{
    [Theory]
    [MemberData(nameof(ErrorsForStatusCode500))]
    public void Problem_ErrorsMappedToCorrectStatusCode_ProblemDetailsResponse(Error error, int expectedStatusCode)
    {
        var httpContext = new DefaultHttpContext();
        var errors = new List<Error> { error };

        var res = errors.Problem(httpContext);

        res.Should().NotBeNull();
        res.Should().BeOfType<ProblemHttpResult>();

        var resTyped = (res as ProblemHttpResult)!;
        resTyped.StatusCode.Should().Be(expectedStatusCode);
    }


    public static IEnumerable<object[]> ErrorsForStatusCode500()
    {
        yield return [
            Error.Failure("Test", "This error is for test purposes only"),
            StatusCodes.Status500InternalServerError
        ];

        yield return [
            Error.Unexpected("Test", "This error is for test purposes only"),
            StatusCodes.Status500InternalServerError
        ];

        yield return [
            Error.Validation("Test", "This error is for test purposes only"),
            StatusCodes.Status400BadRequest
        ];

        yield return [
            Error.NotFound("Test", "This error is for test purposes only"),
            StatusCodes.Status404NotFound
        ];

        yield return [
            Error.Conflict("Test", "This error is for test purposes only"),
            StatusCodes.Status409Conflict
        ];

        yield return [
            Error.Unauthorized("Test", "This error is for test purposes only"),
            StatusCodes.Status401Unauthorized
        ];
    }
}