using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Infastructure
{
    class ReplaceVisitor : ExpressionVisitor
    {
        private readonly Expression from, to;
        public ReplaceVisitor(Expression from, Expression to)
        {
            this.from = from;
            this.to = to;
        }
        public override Expression Visit(Expression node)
        {
            return node == from ? to : base.Visit(node);
        }
    }

    public static class ExpressionExtension
    {
        public static Expression<Func<T1, T3>> Combine<T1, T2, T3>(
            this Expression<Func<T1, T2>> first,
            Expression<Func<T2, T3>> second)
        {
            var param = Expression.Parameter(typeof(T1), "param");

            var newFirst = new ReplaceVisitor(first.Parameters.First(), param).Visit(first.Body);
            var newSecond = new ReplaceVisitor(second.Parameters.First(), newFirst).Visit(second.Body);

            return Expression.Lambda<Func<T1, T3>>(newSecond, param);
        }
    }
}
