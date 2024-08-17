using DataLayer.Contexts;
using DataLayer.Models;
using GroupService.Data;
using GroupService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GroupService.Endpoints
{
    public static class GroupEndpoints
    {
        public static IEndpointRouteBuilder MapGroupEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("groups", GetAllGroups)
                .RequireAuthorization("ADMIN")
                .WithName(nameof(GetAllGroups))
                .WithDescription("Return a list of all groups");

            app.MapGet("groups/{id}", GetGroupById)
                .RequireAuthorization()
                .WithName(nameof(GetGroupById))
                .WithDescription("Return a specified group by id");

            app.MapPost("groups", CreateGroup)
                .RequireAuthorization("ADMIN")
                .WithName(nameof(CreateGroup))
                .WithDescription("Creates new group");

            app.MapPost("groups/{groupId}/students", AddStudentToGroup)
                .RequireAuthorization("ADMIN")
                .WithName(nameof(AddStudentToGroup))
                .WithDescription("Adds a student to the group");

            app.MapDelete("/groups/{id}", RemoveGroup)
                .RequireAuthorization("ADMIN")
                .WithName(nameof(RemoveGroup))
                .WithDescription("Removes groups with a specified id");

            app.MapDelete("groups/{groupId}/students/{studentId}", RemoveStudentFromGroup)
                .RequireAuthorization("ADMIN")
                .WithName(nameof(RemoveStudentFromGroup))
                .WithDescription("Removes a student from group");

            app.MapPut("groups/{groupId}/teacher", ChangeGroupTeacher)
                .RequireAuthorization("ADMIN")
                .WithName(nameof(ChangeGroupTeacher))
                .WithDescription("Changes group's teacher");

            app.MapPut("groups/{id}", RenameGroup)
                .RequireAuthorization("ADMIN")
                .WithName(nameof(RenameGroup))
                .WithDescription("Changes group's name");

            return app;
        }

        private static async Task<IResult> GetAllGroups(AcademyContext context)
        {
            var groups = await context.Groups
                .Include(g => g.Faculty)
                .Include(g => g.Teacher)
                .ThenInclude(t => t.User)
                .Include(g => g.Students)
                .ThenInclude(s => s.User)
                .ToListAsync();

            var groupDtos = groups.Select(g => GroupDtoProvider.Generate(g)).ToList();

            return Results.Ok(groupDtos);
        }

        private static async Task<IResult> GetGroupById(
            Guid id,
            AcademyContext context)
        {
            var group = await context.Groups
                .Include(g => g.Faculty)
                .Include(g => g.Teacher)
                .ThenInclude(t => t.User)
                .Include(g => g.Students)
                .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
            {
                return Results.NotFound("Required group does not exists");
            }

            var groupDto = GroupDtoProvider.Generate(group);

            return Results.Ok(groupDto);
        }

        private static async Task<IResult> CreateGroup(
            AddGroupDTO groupDto,
            AcademyContext context)
        {
            var group = new Group()
            {
                Name = groupDto.Name,
                FacultyId = groupDto.FacultyId,
                TeacherId = groupDto.TeacherId,
            };

            try
            {
                await context.Groups.AddAsync(group);
                await context.SaveChangesAsync();

                return Results.Created(nameof(CreateGroup), group);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

        private static async Task<IResult> RemoveGroup(
            Guid id,
            AcademyContext context)
        {
            var group = await context.Groups.FindAsync(id);

            if (group == null)
            {
                return Results.NotFound("The group does not exist");
            }

            try
            {
                context.Groups.Remove(group);

                await context.SaveChangesAsync();

                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

        private static async Task<IResult> AddStudentToGroup(
            Guid groupId,
            Guid studentId,
            AcademyContext context)
        {
            var student = await context.Students.FindAsync(studentId);

            if (student == null)
            {
                return Results.NotFound("Required student does not exist");
            }

            var group = await context.Groups.FindAsync(groupId);

            if (group == null)
            {
                return Results.NotFound("Required group does not exist");
            }

            group.Students.Add(student);
            await context.SaveChangesAsync();

            return Results.Ok();
        }

        private static async Task<IResult> RemoveStudentFromGroup(
            Guid groupId,
            Guid studentId,
            AcademyContext context)
        {
            var student = await context.Students.FindAsync(studentId);

            if (student == null)
            {
                return Results.NotFound("Required student does not exist");
            }

            var group = await context.Groups.FindAsync(groupId);

            if (group == null)
            {
                return Results.NotFound("Required group does not exist");
            }

            try
            {
                group.Students.Remove(student);

                await context.SaveChangesAsync();

                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }
    
        private static async Task<IResult> ChangeGroupTeacher(
            Guid teacherId,
            Guid groupId,
            AcademyContext context)
        {
            var group = await context.Groups.FindAsync(groupId);

            if (group == null)
            {
                return Results.NotFound("Required group does not exist");
            }

            var teacher = await context.Teachers.FindAsync(teacherId);

            if (teacher == null)
            {
                return Results.NotFound("Required teacher does not exist");
            }

            if (group.TeacherId == teacherId)
            {
                return Results.NoContent();
            }

            group.TeacherId = teacherId;

            await context.SaveChangesAsync();
        
            return Results.Ok();
        }

        private static async Task<IResult> RenameGroup(
            Guid id,
            string newName,
            AcademyContext context)
        {
            if (string.IsNullOrEmpty(newName))
            {
                return Results.BadRequest("Group name cannot be empty");
            }

            var group = await context.Groups.FindAsync(id);

            if (group == null)
            {
                return Results.NotFound("Required group does not exist");
            }

            if (group.Name == newName)
            {
                return Results.NoContent();
            }

            group.Name = newName;

            await context.SaveChangesAsync();

            return Results.Ok();
        }

    }
}
