using FacultyService.Repositories.Interfaces;

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
    }
}
