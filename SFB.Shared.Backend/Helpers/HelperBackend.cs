using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SGD.Shared.Backend.Helpers
{
    public static class HelperBackend
    {
        public static IQueryable<T> ConditionalWhere<T>(this IQueryable<T> source, object? condition, Expression<Func<T, bool>> predicate)
        {
            if (condition == null) return source;
            bool condi = ExistObj(condition);
            return condi ? source.Where(predicate) : source;
        }

        public static bool ExistObj(object condition)
        {
            return condition switch
            {
                bool booleanCondition => booleanCondition,
                _ => true
            };
        }
    }
}
