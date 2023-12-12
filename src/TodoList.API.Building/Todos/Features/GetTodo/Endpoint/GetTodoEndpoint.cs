using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using TodoList.API.Building.Persistence.Contexts;
using TodoList.API.Building.Todos.Domains;
using TodoList.Common.Web;

namespace TodoList.API.Todos.Features.GetTodo.Endpoint;

internal class GetTodoEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.CreateEndpoint<Todo>(MinimalApiExtensions.EndpointType.Get, "todos/{id:guid}", GetTodo)
            .ProducesProblem(404);

        return builder;
    }



    public async Task<IResult> GetTodo([FromRoute] Guid id, AppDbContext dbContext)
    {
        var todo = await dbContext.Set<Todo>()
                        .Include(x => x.Items)
                        .FirstOrDefaultAsync(x => x.Id == id);

        if (todo is null)
            return Results.NotFound();

        return Results.Ok(todo);
    }
}
