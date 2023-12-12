using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Diagnostics.CodeAnalysis;
using TodoList.API.Setup.Web;

namespace TodoList.Common.Web;

public static class MinimalApiExtensions
{
    public static IServiceCollection AddMinimalEndpoints(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.Scan(scan =>
        {
            scan.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(classes => classes.AssignableTo(typeof(IMinimalEndpoint)))
            .UsingRegistrationStrategy(RegistrationStrategy.Append)
            .As<IMinimalEndpoint>()
            .WithLifetime(serviceLifetime);
        });

        return services;
    }

    public static IEndpointRouteBuilder MapMinimalEndpoints(this IEndpointRouteBuilder builder)
    {
        var scope = builder.ServiceProvider.CreateScope();
        var endpoints = scope.ServiceProvider.GetServices<IMinimalEndpoint>();

        foreach (var endpoint in endpoints)
            endpoint.MapEndpoint(builder);

        return builder;
    }

    public static RouteHandlerBuilder CreateEndpoint<TEntity>(this IEndpointRouteBuilder builder,
        EndpointType endpointType, [StringSyntax("Route")] string path, Delegate handler)
         => builder.CreateConvention<TEntity>(endpointType, $"{EndpointPath.BaseApiPath}/{path}", handler)
            .WithTags($"{typeof(TEntity).Name}s");

    private static RouteHandlerBuilder CreateConvention<TEntity>(this IEndpointRouteBuilder builder,
        EndpointType endpointType, string path, Delegate handler)
        => endpointType switch
        {
            EndpointType.Filter => builder.MapGet(path, handler).WithName(handler.Method.Name).Produces(200, typeof(List<TEntity>)),
            EndpointType.Get => builder.MapGet(path, handler).WithName(handler.Method.Name).Produces(200, typeof(TEntity)),
            EndpointType.Post => builder.MapPost(path, handler).WithName(handler.Method.Name).Produces(201, typeof(TEntity)),
            EndpointType.Put => builder.MapPut(path, handler).WithName(handler.Method.Name).Produces(204),
            EndpointType.Patch => builder.MapPatch(path, handler).WithName(handler.Method.Name).Produces(204),
            EndpointType.Delete => builder.MapDelete(path, handler).WithName(handler.Method.Name).Produces(204),
            _ => throw new NotImplementedException()
        };


    public enum EndpointType
    {
        Get,
        Filter,
        Post,
        Put,
        Patch,
        Delete
    }
}
