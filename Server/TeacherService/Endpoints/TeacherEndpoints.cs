using AutoMapper;
using DataLayer.Models;
using TeacherService.Data;
using TeacherService.Repositories.Interfaces;

namespace TeacherService.Endpoints
{
    public static class TeacherEndpoints
    {
        public static IEndpointRouteBuilder MapTeacherEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("teachers", GetAllTeachers)
                .WithOpenApi()
                .WithSummary(nameof(GetAllTeachers))
                .WithDescription("Returns a list of teachers");

            app.MapGet("teachers/{id}", GetTeacherById)
                .WithSummary(nameof(GetTeacherById))
                .WithDescription("Returns a spicified teacher by id")
                .WithOpenApi();

            app.MapPost("teachers", CreateTeacher)
                .WithSummary(nameof(CreateTeacher))
                .WithDescription("Creates a new teacher")
                .WithOpenApi();

            app.MapDelete("teachers/{id}", DeleteTeacher)
                .WithSummary(nameof(DeleteTeacher))
                .WithDescription("Removes a specified teacher")
                .WithOpenApi();

            app.MapPatch("teachers/{id}", EditTeacher)
                .WithSummary(nameof(EditTeacher))
                .WithDescription("Edits teacher's data")
                .WithOpenApi();

            return app;
        }

        private static async Task<IResult> GetAllTeachers(
            ITeacherRepository teacherRepository,
            IMapper mapper)
        {
            var teachers = await teacherRepository.GetAllAsync();
            var dtos = mapper.Map<IEnumerable<TeacherDto>>(teachers);

            return Results.Ok(dtos);
        }

        private static async Task<IResult> GetTeacherById(
            Guid id,
            ITeacherRepository teacherRepository,
            IMapper mapper)
        {
            var teacher = await teacherRepository.GetByIdAsync(id);
            var dto = mapper.Map<TeacherDto>(teacher);

            return Results.Ok(dto);
        }

        private static async Task<IResult> CreateTeacher(
            TeacherCreateDto request,
            ITeacherRepository teacherRepository,
            IMapper mapper)
        {
            if (request is null)
            {
                return Results.BadRequest("Request is empty");
            }

            var teacher = mapper.Map<Teacher>(request);

            await teacherRepository.AddAsync(teacher);
            await teacherRepository.SaveAsync();

            var dto = mapper.Map<TeacherDto>(teacher);

            return Results.CreatedAtRoute(nameof(GetTeacherById), new { teacher.Id }, dto);
        }

        private static async Task<IResult> DeleteTeacher(
            Guid id,
            ITeacherRepository teacherRepository)
        {
            var teacher = await teacherRepository.GetByIdAsync(id);

            if (teacher is null)
            {
                return Results.NotFound("Required teacher does not exist");
            }

            teacherRepository.Delete(teacher);
            await teacherRepository.SaveAsync();

            return Results.NoContent();
        }

        private static async Task<IResult> EditTeacher(
            Guid id,
            TeacherEditDto request,
            ITeacherRepository teacherRepository,
            IMapper mapper)
        {
            if (request is null)
            {
                return Results.BadRequest("Request is null");
            }

            var teacher = await teacherRepository.GetByIdAsync(id);

            if (teacher is null)
            {
                return Results.NotFound("Required teacher does not exist");
            }

            teacher.DepartmentId = request.DepartmentId;

            await teacherRepository.SaveAsync();

            var dto = mapper.Map<TeacherDto>(teacher);

            return Results.Ok(dto);
        }
    }
}
