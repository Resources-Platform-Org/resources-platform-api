using System.Linq.Expressions;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Data;

public static class ModelBuilderExtensions
{
    public static void AddGlobalFilter<TInterface>(this ModelBuilder modelBuilder, Expression<Func<TInterface, bool>> filterExpression)
    {
        var entities = modelBuilder.Model.GetEntityTypes()
            .Where(e => typeof(TInterface).IsAssignableFrom(e.ClrType))
            .Select(e => e.ClrType);

        foreach (var entity in entities)
        {
            var parameter = Expression.Parameter(entity);
            var body = ReplacingExpressionVisitor.Replace(filterExpression.Parameters.Single(), parameter, filterExpression.Body);
            var lambda = Expression.Lambda(body, parameter);

            modelBuilder.Entity(entity).HasQueryFilter(lambda);
        }
    }
}
