using DataLayer.Contexts;
using DataLayer.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StudentService.Data;
using System.Net.Http.Headers;

namespace StudentService.Endpoints
{
    public static class StudentEndpoints
    {
        public static IEndpointRouteBuilder MapStudentEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("students", GetAllStudents)
                .RequireAuthorization("ADMIN")
                .WithName("GetAllStudents")
                .WithDescription("Returns a list of all students");

            app.MapGet("students/{id}", GetStudentById)
                .RequireAuthorization("ADMIN")
                .WithName(nameof(GetStudentById))
                .WithDescription("Returns a specified student by id");

            app.MapGet("students/self", GetSelf)
                .RequireAuthorization("STUDENT")
                .WithName("GetSelfData")
                .WithDescription("Return students self data");

            app.MapPost("students", AddStudent)
                .RequireAuthorization("ADMIN")
                .WithName("AddStudent")
                .WithDescription("Adds new student to academy");

            app.MapDelete("students/{id}", RemoveStudent)
                .RequireAuthorization("ADMIN")
                .WithName("RemoveStudentById")
                .WithDescription("Removes student by a specified id");

            app.MapPut("students/{id}", EditStudent)
                .RequireAuthorization("ADMIN")
                .WithName("RemoveStudent")
                .WithDescription("Removes student with specified id");

            return app;
        }

        private static async Task<IResult> GetAllStudents(AcademyContext context)
        {
            var students = await context.Students.ToListAsync();
            return Results.Ok(students);
        }

        private static async Task<IResult> GetStudentById(Guid id, AcademyContext context)
        {
            var student = await context.Students.FindAsync(id);
            
            if (student == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(student);
        }

        private static async Task<IResult> GetSelf(Guid id, AcademyContext context)
        {
            var student = await context.Students
                .Include(s => s.User)
                .Include(s => s.Group)
                .FirstOrDefaultAsync(s => s.Id == id);

            return Results.Ok(student);
        }

        private static async Task<IResult> AddStudent(
            StudentDTO studentDTO, 
            HttpClient httpClient, 
            HttpContext httpContext,
            AcademyContext context)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5094/api/users")
            {
                Content = JsonContent.Create(new
                {
                    Name = studentDTO.FirstName,
                    Surname = studentDTO.LastName,
                    Username = studentDTO.Username,
                    Password = studentDTO.Password,
                    Email = studentDTO.Email
                }),
            };

            var token = httpContext.Request.Cookies["tasty-cookies"];

            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return Results.Problem(response.StatusCode.ToString());
            }

            var content = await response.Content.ReadFromJsonAsync<User>() ?? throw new Exception();
            var student = new Student()
            {
                UserId = content.Id,
                GroupId = studentDTO.GroupId
            };

            await context.Students.AddAsync(student);
            await context.SaveChangesAsync();

            return Results.Created(nameof(AddStudent), student);
        }
    
        private static async Task<IResult> RemoveStudent(Guid id, AcademyContext context)
        {
            var student = await context.Students.FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return Results.NotFound();
            }

            context.Students.Remove(student);
            await context.SaveChangesAsync();

            return Results.Ok();
        }

        public static async Task<IResult> EditStudent(
            Guid id,
            EditStudentDTO data, 
            AcademyContext context)
        {
            var student = await context.Students
                .Include(s => s.Group)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return Results.NotFound();
            }

            if (!string.IsNullOrEmpty(data.FirstName)) 
                student.User.FirstName = data.FirstName;
            if (!string.IsNullOrEmpty(data.LastName))
                student.User.LastName = data.LastName;
            if (data.GroupId != null)
                student.GroupId = data.GroupId;
            if (!string.IsNullOrEmpty(data.Email)) 
                student.User.Email = data.Email;
            if (!string.IsNullOrEmpty(data.Password))
                student.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(data.Password);

            await context.SaveChangesAsync();

            return Results.Ok();
        }
    }
}
