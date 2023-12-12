using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TodoList.API.Building.Todos.Domains;
using TodoList.API.Todos.Features.GetTodo.Endpoint;
using TodoList.Common.EFCore;
using TodoList.Common.Web;

namespace TodoList.API.Todos.Features.CreateTodo.Endpoint;

internal class CreateTodoEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.CreateEndpoint<Todo>(MinimalApiExtensions.EndpointType.Post, "todos", CreateTodo);
        return builder;
    }


    public async Task<IResult> CreateTodo(CreateTodoRequest request, IDbContext dbContext, CancellationToken cancellationToken)
    {
        var todo = Todo.Create(request.Name);
        await dbContext.Set<Todo>().AddAsync(todo, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Results.CreatedAtRoute(nameof(GetTodoEndpoint.GetTodo), new { id = todo.Id }, todo);
    }

    public record CreateTodoRequest(string Name);
}
