using AutoMapper;
using DataLayer.Models;
using GroupService.Data.Dtos;
using GroupService.Repositories;
using GroupService.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GroupService.Endpoints
{
    public static class GroupEndpoints
    {
        public static IEndpointRouteBuilder MapGroupEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("groups", GetAllGroups)
                .WithSummary(nameof(GetAllGroups))
                .WithDescription("Return a list of all groups");

            app.MapGet("groups/{id}", GetGroupById)
                .WithSummary(nameof(GetGroupById))
                .WithDescription("Return a specified group by id");

            app.MapPost("groups", CreateGroup)
                .WithSummary(nameof(CreateGroup))
                .WithDescription("Creates new group");

            app.MapPost("groups/{groupId}/students", AddStudentToGroup)
                .WithSummary(nameof(AddStudentToGroup))
                .WithDescription("Adds a student to the group");

            app.MapDelete("/groups/{id}", DeleteGroup)
                .WithSummary(nameof(DeleteGroup))
                .WithDescription("Removes groups with a specified id");

            app.MapDelete("groups/{groupId}/students/{studentId}", RemoveStudentFromGroup)
                .WithSummary(nameof(RemoveStudentFromGroup))
                .WithDescription("Removes a student from group");

            app.MapPatch("groups/{groupId}/teacher", ChangeGroupTeacher)
                .WithSummary(nameof(ChangeGroupTeacher))
                .WithDescription("Changes group's teacher");

            app.MapPatch("groups/{id}", RenameGroup)
                .RequireAuthorization("ADMIN")
                .WithSummary(nameof(RenameGroup))
                .WithDescription("Changes group's name");

            return app;
        }

        private static async Task<IResult> GetAllGroups(
            IGroupRepository groupRepository,
            IMapper mapper)
        {
            var groups = await groupRepository.GetAllAsync();
            var dtos = mapper.Map<IEnumerable<GroupDTO>>(groups);

            return Results.Ok(dtos);
        }

        private static async Task<IResult> GetGroupById(
            Guid id,
            IGroupRepository groupRepository,
            IMapper mapper)
        {
            var group = await groupRepository.GetByIdAsync(id);

            if (group is null)
            {
                return Results.NotFound("Required group does not exists");
            }

            var dto = mapper.Map<GroupDTO>(group);

            return Results.Ok(dto);
        }

        private static async Task<IResult> CreateGroup(
            GroupCreateDto request,
            IGroupRepository groupRepository,
            IMapper mapper)
        {
            var group = mapper.Map<Group>(request);

            try
            {
                await groupRepository.AddAsync(group);

                var dto = mapper.Map<GroupDTO>(group);

                return Results.CreatedAtRoute(nameof(GetGroupById), new { group.Id }, dto);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

        private static async Task<IResult> DeleteGroup(
            Guid id,
            IGroupRepository groupRepository)
        {
            var group = await groupRepository.GetByIdAsync(id);

            if (group is null)
            {
                return Results.NotFound("The group does not exist");
            }

            try
            {
                await groupRepository.DeleteAsync(group);

                return Results.NoContent();
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

        private static async Task<IResult> AddStudentToGroup(
            Guid groupId,
            Guid studentId,
            IGroupRepository groupRepository,
            IStudentRepository studentRepository,
            IMapper mapper)
        {
            var group = await groupRepository.GetByIdAsync(groupId);

            if (group is null)
            {
                return Results.NotFound("Group does not exist");
            }

            var student = await studentRepository.GetByIdAsync(studentId);

            if (student is null)
            {
                return Results.NotFound("Required student does not exist");
            }

            if (group.Students.Contains(student))
            {
                return Results.Problem("The student is already part of the group");
            }

            try
            {
                group.Students.Add(student);
                await groupRepository.SaveAsync();

                var dto = mapper.Map<GroupDTO>(group);

                return Results.Ok(dto);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

        private static async Task<IResult> RemoveStudentFromGroup(
            Guid groupId,
            Guid studentId,
            IGroupRepository groupRepository,
            IStudentRepository studentRepository)
        {
            var group = await groupRepository.GetByIdAsync(groupId);

            if (group is null)
            {
                return Results.NotFound("Group does not exist");
            }

            var student = await studentRepository.GetByIdAsync(studentId);

            if (student is null)
            {
                return Results.NotFound("Required student does not exist");
            }

            if (!group.Students.Contains(student))
            {
                return Results.NotFound("The student is not a part of the group");
            }

            try
            {
                group.Students.Remove(student);

                await groupRepository.SaveAsync();

                return Results.NoContent();
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

        private static async Task<IResult> ChangeGroupTeacher(
            Guid groupId,
            Guid teacherId,
            IGroupRepository groupRepository,
            IMapper mapper)
        {
            var group = await groupRepository.GetByIdAsync(groupId);

            if (group is null)
            {
                return Results.NotFound("Required group does not exist");
            }

            try
            {
                if (group.TeacherId != teacherId)
                {
                    group.TeacherId = teacherId;
                    await groupRepository.SaveAsync();
                }

                var dto = mapper.Map<GroupDTO>(group);

                return Results.Ok(dto);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

        private static async Task<IResult> RenameGroup(
            Guid id,
            GroupEditDto request,
            IGroupRepository groupRepository,
            IMapper mapper)
        {
            if (request is null)
            {
                return Results.BadRequest();
            }

            var group = await groupRepository.GetByIdAsync(id);

            if (group is null)
            {
                return Results.NotFound("Required group does not exist");
            }

            group.Name = request.Name;

            await groupRepository.SaveAsync();

            var dto = mapper.Map<GroupDTO>(group);

            return Results.Ok(dto);
        }

    }
}
