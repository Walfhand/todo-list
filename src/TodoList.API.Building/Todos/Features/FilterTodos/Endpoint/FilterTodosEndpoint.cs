using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using TodoList.API.Building.Todos.Domains;
using TodoList.Common.EFCore;
using TodoList.Common.Web;

namespace TodoList.API.Todos.Features.FilterTodos.Endpoint;

internal class FilterTodosEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.CreateEndpoint<Todo>(MinimalApiExtensions.EndpointType.Filter, "todos", FilterTodos);
        return builder;
    }

    public async Task<IResult> FilterTodos([AsParameters] TodoFilter filter, IDbContext dbContext, CancellationToken cancellationToken)
    {
        var query = dbContext.Set<Todo>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name == filter.Name);

        return Results.Ok(await query.ToListAsync(cancellationToken));
    }

    public record TodoFilter(string? Name);
}
