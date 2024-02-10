using Asp.Versioning.Builder;
using System.Diagnostics.CodeAnalysis;
using Fusion.Core.Assignment;
using Fusion.RestApi.Assignment.Models;
using Fusion.Core.Profile;
using Fusion.RestApi.Extensions;

namespace Fusion.RestApi.Assignment
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class AssignmentModule
    {
        internal static void AddAssignmentEndpoints(this IVersionedEndpointRouteBuilder app)
        {
            app.WithTags("Assignment")
               .HasApiVersion(1)
               .RequireAuthorization("teacher", "student")
               .MapGet("/api/assigments/{id:long}", async (
                long id,
                HttpContext httpContext,
                IAssignmentService assignmentService,
                CancellationToken _) =>
               {
                   var result = await assignmentService.GetAsync(id);

                   return result.Match(Results.Ok, e => e.Problem(context: httpContext));
               });

            var teacherProfile = app.MapGroup("/api/assignments/")
                .WithTags("Assignment")
                .HasApiVersion(1)
                .RequireAuthorization("teacher");

            teacherProfile.MapPost("", async (
                HttpContext httpContext,
                CreateAssignmentViewModel req,
                IAssignmentService assignmentService,
                CancellationToken _) =>
            {
                var createAssignmentDto = new CreateAssignmentDto
                {
                    Title = req.Title,
                    Description = req.Description,
                    Deadline = req.Deadline,
                    MaxGrade = req.MaxGrade,
                    StartDate = req.StartDate,
                    Type = req.Type,
                };

                var result = await assignmentService.CreateAsync(createAssignmentDto);
                return result.Match(Results.Ok, e => e.Problem(context: httpContext));
            });

            teacherProfile.MapPatch("{id:long}", async (
                long id,
                HttpContext httpContext,
                UpdateAssignmentViewModel req,
                IAssignmentService assignmentService,
                CancellationToken _) =>
            {
                var updateAssignmentDto = new UpdateAssignmentDto
                {
                    Id = id,
                    Title = req.Title,
                    Description = req.Description,
                    Deadline = req.Deadline,
                    MaxGrade = req.MaxGrade,
                    StartDate = req.StartDate,
                    Type = req.Type,
                };

                var result = await assignmentService.UpdateAsync(updateAssignmentDto);
                return result.Match(Results.Ok, e => e.Problem(context: httpContext));
            });

            teacherProfile.MapDelete("{id:long}", async (
                long id,
                IAssignmentService assignmentService,
                CancellationToken _) =>
            {
                await assignmentService.DeleteAsync(id);
                return Results.Ok();
            });
        }
    }
}
