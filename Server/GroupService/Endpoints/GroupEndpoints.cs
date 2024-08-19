using DataLayer.Contexts;
using DataLayer.Models;
using GroupService.Data;
using GroupService.Infrastructure;
using GroupService.Repositories.Interfaces;
using GroupService.Services;
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

        private static async Task<IResult> GetAllGroups(IGroupRepository groupRepository)
        {
            var groups = await groupRepository.GetAllAsync();

            return Results.Ok(groups);
        }

        private static async Task<IResult> GetGroupById(
            Guid id,
            IGroupRepository groupRepository)
        {
            var group = await groupRepository.GetByIdAsync(id);

            if (group == null)
            {
                return Results.NotFound("Required group does not exists");
            }

            var groupDto = GroupDtoProvider.Generate(group);

            return Results.Ok(groupDto);
        }

        private static async Task<IResult> CreateGroup(
            AddGroupDTO groupDto,
            IGroupRepository groupRepository,
            IGroupService groupService)
        {
            var group = groupService.Create(groupDto);

            try
            {
                await groupRepository.AddAsync(group);

                return Results.Created(nameof(CreateGroup), group);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

        private static async Task<IResult> RemoveGroup(
            Guid id,
            IGroupRepository groupRepository)
        {
            var group = await groupRepository.GetByIdAsync(id);

            if (group == null)
            {
                return Results.NotFound("The group does not exist");
            }

            try
            {
                await groupRepository.RemoveAsync(group);

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
            IGroupService groupService)
        {
            try
            {
                var result = await groupService.AddStudent(studentId, groupId);
                if (!result)
                {
                    return Results.Problem("Error while preccessing");
                }

                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

        private static async Task<IResult> RemoveStudentFromGroup(
            Guid groupId,
            Guid studentId,
            IGroupService groupService)
        {

            try
            {
                var result = await groupService.RemoveStudent(studentId, groupId);

                if (!result)
                {
                    return Results.Problem("Error while precessing");
                }

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
            IGroupService groupService)
        {
            try
            {
                var result = await groupService.ChangeTeacher(teacherId, groupId);

                if (!result)
                {
                    return Results.Problem("Error while precessing");
                }

                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

        private static async Task<IResult> RenameGroup(
            Guid id,
            string newName,
            IGroupService groupService)
        {
            if (string.IsNullOrEmpty(newName))
            {
                return Results.BadRequest("Group name cannot be empty");
            }

            try
            {
                var updatedGroup = await groupService.Rename(id, newName);

                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

    }
}
