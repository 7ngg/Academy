using AutoMapper;
using DataLayer.Models;
using System.Text.Json;
using UserService.Data;
using UserService.Repositories;
using UserService.Validators;

namespace UserService.Endpoint
{
    public static class UserEndpoints
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("users", GetAllUsers)
                .WithName(nameof(GetAllUsers))
                .WithDescription("Return a list of users")
                .WithOpenApi();

            app.MapGet("users/{id}", GetUserById)
                .WithName(nameof(GetUserById))
                .WithDescription("Return a specified user by id")
                .WithOpenApi();

            app.MapPost("users", CreateUser)
                .WithName(nameof(CreateUser))
                .WithDescription("Creates a new user")
                .WithOpenApi();

            app.MapDelete("users/{id}", DeleteUser)
                .WithName(nameof(DeleteUser))
                .WithDescription("Deletes a specified user by id")
                .WithOpenApi();

            return app;
        }

        private static async Task<IResult> GetAllUsers(
            IUserRepository userRepository,
            IMapper mapper)
        {
            var users = await userRepository.GetAll();

            return Results.Ok(mapper.Map<IEnumerable<UserDto>>(users));
        }

        private static async Task<IResult> GetUserById(
            Guid id,
            IUserRepository userRepository,
            IMapper mapper)
        {
            var user = await userRepository.GetById(id);

            if (user is null)
            {
                return Results.NotFound("User does not exist");
            }

            return Results.Ok(mapper.Map<UserDto>(user));
        }

        private static async Task<IResult> CreateUser(
            UserCreateDto request,
            IUserRepository userRepository,
            IMapper mapper)
        {
            var validator = new UserCreateValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                var errors = JsonSerializer.Serialize(validationResult.Errors);
                return Results.Problem(errors);
            }

            var user = mapper.Map<User>(request);

            await userRepository.Create(user);
            await userRepository.Save();

            var userReadDto = mapper.Map<UserDto>(user);

            return Results.CreatedAtRoute(nameof(CreateUser), new { user.Id }, userReadDto);
        }

        private static async Task<IResult> DeleteUser(
            Guid id,
            IUserRepository userRepository)
        {
            var user = await userRepository.GetById(id);

            if (user is null)
            {
                return Results.NotFound("User does not exist");
            }

            userRepository.Delete(user);
            await userRepository.Save();

            return Results.Accepted();
        }
    }
}
