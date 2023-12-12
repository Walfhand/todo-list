using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoList.API.Building.Todos.Domains;
using TodoList.Common.EFCore;

namespace TodoList.API.Building.Todos.ETCS;

public class TodoEtc : BaseETC<Todo>
{
    public override void Configure(EntityTypeBuilder<Todo> builder)
    {
        base.Configure(builder);
        builder.ToTable(nameof(Todo).ToLower());
        builder.OwnsMany(x => x.Items);
    }
}
