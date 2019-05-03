using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Framework.Entities;

namespace Core.Framework.Repository
{
    public abstract class BaseSpecification<T> : ISpecification<T>
        where T : class, IDbObject
    {
        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; } =
            new List<Expression<Func<T, object>>>();

        public List<string> IncludeStrings { get; } =
            new List<string>();

        protected BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
        }

        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }
    }
}
