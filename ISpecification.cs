using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Framework.Entities;

namespace Core.Framework.Repository
{
    public interface ISpecification<T>
        where T : class, IAggregationRoot
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
    }
}
