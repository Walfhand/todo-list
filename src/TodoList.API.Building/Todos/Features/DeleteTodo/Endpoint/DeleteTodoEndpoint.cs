using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using TodoList.API.Building.Todos.Domains;
using TodoList.Common.EFCore;
using TodoList.Common.Web;

namespace TodoList.API.Todos.Features.DeleteTodo.Endpoint;

internal class DeleteTodoEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.CreateEndpoint<Todo>(MinimalApiExtensions.EndpointType.Delete, "todos/{id:guid}", DeleteTodo);
        return builder;
    }

    public async Task<IResult> DeleteTodo(Guid id, IDbContext dbContext, CancellationToken cancellationToken)
    {
        var todo = await dbContext.Set<Todo>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (todo is null)
            return Results.NotFound();

        dbContext.Set<Todo>().Remove(todo);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Results.NoContent();
    }
}
