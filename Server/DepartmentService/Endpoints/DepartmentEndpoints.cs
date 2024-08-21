using AutoMapper;
using DepartmentService.Data;
using DepartmentService.Repositories.Interfaces;
using DepartmentService.Services;

namespace DepartmentService.Endpoints
{
    public static class DepartmentEndpoints
    {
        public static IEndpointRouteBuilder MapDepartmentEnpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("departments", GetAllDepartments)
                .WithName(nameof(GetAllDepartments))
                .WithDescription("Return a list of all departments")
                .WithOpenApi();

            app.MapGet("departments/{id}", GetDepartmnetById)
                .WithName(nameof(GetDepartmnetById))
                .WithDescription("Returns a specifies department by id")
                .WithOpenApi();

            app.MapPost("departments", CreateDepartment)
                .WithName(nameof(CreateDepartment))
                .WithDescription("Creates a new department")
                .WithOpenApi();

            app.MapPost("departments/{departmentId}/teachers", AddTeacherToDepartment)
                .WithName(nameof(AddTeacherToDepartment))
                .WithDescription("Adds a teacher to department")
                .WithOpenApi();

            app.MapDelete("departments/{id}", RemoveDepartment)
                .WithName(nameof(RemoveDepartment))
                .WithDescription("Deletes a specified department by id")
                .WithOpenApi();

            app.MapDelete("departments/{departmentId}/teachers/{teacherId}", RemoveTeacherFromDepartment)
                .WithName(nameof(RemoveTeacherFromDepartment))
                .WithDescription("Removes teacher from a departments")
                .WithOpenApi();

            app.MapPatch("departments/{id}", Edit)
                .WithName(nameof(Edit))
                .WithDescription("Edits a specified department")
                .WithOpenApi();

            return app;
        }

        private static async Task<IResult> GetAllDepartments(
            IDepartmentRepository departmentRepository,
            IMapper mapper)
        {
            var departments = await departmentRepository.GetAll();

            return Results.Ok(mapper.Map<IEnumerable<DepartmentDto>>(departments));
        }

        private static async Task<IResult> GetDepartmnetById(
            Guid id,
            IDepartmentRepository departmentRepository,
            IMapper mapper)
        {
            var department = await departmentRepository.GetById(id);

            return Results.Ok(mapper.Map<DepartmentDto>(department));
        }

        private static async Task<IResult> CreateDepartment(
            DepartmentCreateDto request,
            IDepartmentService departmentService,
            IMapper mapper)
        {
            try
            {
                var department = await departmentService.Create(request);
                var dto = mapper.Map<DepartmentDto>(department);

                return Results.CreatedAtRoute(nameof(CreateDepartment), new { department.Id }, dto);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

        private static async Task<IResult> RemoveDepartment(
            Guid id,
            IDepartmentService departmentService)
        {
            try
            {
                await departmentService.Remove(id);

                return Results.Accepted();
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

        private static async Task<IResult> Edit(
            Guid id,
            DepartmentEditDto request,
            IDepartmentService departmentService,
            IMapper mapper)
        {
            try
            {
                var updatedData = await departmentService.Edit(id, request);

                if (updatedData is null)
                {
                    return Results.NoContent();
                }

                var dto = mapper.Map<DepartmentDto>(updatedData);

                return Results.Ok(dto);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

        private static async Task<IResult> AddTeacherToDepartment(
            Guid departmentId,
            Guid teacherId,
            IDepartmentService departmentService,
            IMapper mapper)
        {
            try
            {
                var updatedData = await departmentService.AddTeacher(departmentId, teacherId);
                var dto = mapper.Map<DepartmentDto>(updatedData);

                return Results.Ok(dto);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

        private static async Task<IResult> RemoveTeacherFromDepartment(
            Guid departmentId,
            Guid teacherId,
            IDepartmentService departmentService)
        {
            try
            {
                await departmentService.RemoveTeacher(departmentId, teacherId);

                return Results.NoContent();
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }
    }
}
