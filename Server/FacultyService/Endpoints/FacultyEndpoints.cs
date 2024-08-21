using AutoMapper;
using FacultyService.Data;
using FacultyService.Repositories.Interfaces;
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
                .WithName(nameof(GetFacultyById))
                .WithDescription("Returns a specifies faculty by id")
                .WithOpenApi();

            app.MapPost("faculties", CreateFaculty)
                .WithName(nameof(CreateFaculty))
                .WithDescription("Create a new faculty")
                .WithOpenApi();

            app.MapPost("faculties/{facultyId}/groups", AddGroupToFaculty)
                .WithName(nameof(AddGroupToFaculty))
                .WithDescription("Adds a group to a faculty")
                .WithOpenApi();

            app.MapDelete("faculties/{id}", RemoveFaculty)
                .WithName(nameof(RemoveFaculty))
                .WithDescription("Remove a specified faculty by id")
                .WithOpenApi();

            app.MapDelete("faculties/{facultyId}/groups/{groupId}", RemoveGroupFromFaculty)
                .WithName(nameof(RemoveGroupFromFaculty))
                .WithDescription("Removes a group from faculty")
                .WithOpenApi();

            app.MapPatch("faculties/{id}", RenameFaculty)
                .WithName(nameof(RenameFaculty))
                .WithDescription("Renames a faculty by id")
                .WithOpenApi();

            return app;
        }

        private static async Task<IResult> GetFaculties(
            IFacultyRepository facultyRepository,
            IMapper mapper)
        {
            var faculties = await facultyRepository.GetAll();
            return Results.Ok(mapper.Map<IEnumerable<FacultyDto>>(faculties));
        }

        private static async Task<IResult> GetFacultyById(
            Guid id,
            IFacultyRepository facultyRepository,
            IMapper mapper)
        {
            try
            {
                var faculty = await facultyRepository.GetById(id);
                return Results.Ok(mapper.Map<FacultyDto>(faculty));
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

        private static async Task<IResult> CreateFaculty(
            FacultyCreateDto request,
            IFacultyService facultyService,
            IMapper mapper)
        {
            var faculty = await facultyService.CreateFaculty(request);

            var dto = mapper.Map<FacultyDto>(faculty);

            return Results.CreatedAtRoute(nameof(CreateFaculty), new { faculty.Id }, dto);
        }

        private static async Task<IResult> AddGroupToFaculty(
            Guid facultyId,
            Guid groupId,
            IFacultyService facultyService,
            IMapper mapper)
        {
            var faculty = await facultyService.AddGroup(facultyId, groupId);

            var dto = mapper.Map<FacultyDto>(faculty);

            return Results.AcceptedAtRoute(
                nameof(AddGroupToFaculty),
                new { faculty.Id },
                dto);
        }

        private static async Task<IResult> RemoveGroupFromFaculty(
            Guid facultyId,
            Guid groupId,
            IFacultyService facultyService)
        {
            try
            {
                await facultyService.RemoveGroup(facultyId, groupId);

                return Results.Accepted();
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }
    }
}
