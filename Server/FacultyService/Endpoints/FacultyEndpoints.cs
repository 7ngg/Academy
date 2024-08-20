﻿using FacultyService.Repositories.Interfaces;
using FacultyService.Services;

namespace FacultyService.Endpoints
{
    public static class FacultyEndpoints
    {
        public static IEndpointRouteBuilder MapFacultyEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("faculties", GetFaculties)
                .WithName(nameof(GetFaculties))
                .WithDescription("Returns a list of faculties")
                .WithOpenApi();

            app.MapGet("faculties/{id}", GetFacultyById)
                .RequireAuthorization("ADMIN")
                .WithDisplayName(nameof(GetFacultyById))
                .WithDescription("Returns a specifies faculty by id")
                .WithOpenApi();

            app.MapDelete("faculties/{id}", RemoveFaculty)
                .RequireAuthorization("ADMIN")
                .WithDisplayName(nameof(RemoveFaculty))
                .WithDescription("Remove a specified faculty by id")
                .WithOpenApi();

            app.MapPatch("faculties/{id}", RenameFaculty)
                .RequireAuthorization("ADMIN")
                .WithDisplayName(nameof(RenameFaculty))
                .WithDescription("Renames a faculty by id")
                .WithOpenApi();

            return app;
        }

        private static async Task<IResult> GetFaculties(IFacultyRepository facultyRepository)
        {
            var faculties = await facultyRepository.GetAll();
            return Results.Ok(faculties);
        }

        private static async Task<IResult> GetFacultyById(
            Guid id,
            IFacultyRepository facultyRepository)
        {
            try
            {
                var faculty = await facultyRepository.GetById(id);
                return Results.Ok(faculty);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

        private static async Task<IResult> RemoveFaculty(
            Guid id,
            IFacultyRepository facultyRepository)
        {
            try
            {
                await facultyRepository.RemoveAsync(id);

                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

        private static async Task<IResult> RenameFaculty(
            Guid id,
            string newName,
            IFacultyService facultyService)
        {
            try
            {
                await facultyService.Rename(id, newName);

                return Results.Accepted();
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }
    }
}
