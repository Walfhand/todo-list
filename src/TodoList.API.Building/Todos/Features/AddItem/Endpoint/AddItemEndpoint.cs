using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using TodoList.API.Building.Todos.Domains;
using TodoList.Common.EFCore;
using TodoList.Common.Web;

namespace TodoList.API.Building.Todos.Features.AddItem.Endpoint;

internal class AddItemEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.CreateEndpoint<Todo>(MinimalApiExtensions.EndpointType.Post, "todos/{id:guid}/items", AddItem);
        return builder;
    }

    public async Task<IResult> AddItem(Guid id, CreateItemRequest request, IDbContext dbContext, CancellationToken cancellationToken)
    {
        var todo = await dbContext.Set<Todo>()
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (todo is null)
            return Results.NotFound();

        var item = todo.AddItem(request.Name);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Results.Created("", item);
    }

    public record CreateItemRequest(string Name);
}
