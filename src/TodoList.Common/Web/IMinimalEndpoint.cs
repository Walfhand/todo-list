using Microsoft.AspNetCore.Routing;

namespace TodoList.Common.Web;
public interface IMinimalEndpoint
{
    IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder);
}
