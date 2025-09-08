using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SFB.Shared.Backend.Helpers
{
    public abstract class ExpressionHelper<T>
    {
        public static IQueryable<T> GenerateSearchQuery(List<string> filterProps, IQueryable<T> query, string? filter)
        {
            if (string.IsNullOrEmpty(filter) || filterProps.Count == 0)
                return query;

            var predicate = GeneratePredicate(filterProps, query, filter);

            return query.Where(predicate);
        }

        public static Expression<Func<T, bool>> GeneratePredicate(List<string> filterProps, IQueryable<T> query, string filter)
        {
            var parameter = Expression.Parameter(typeof(T), "e");
            var members = GenerateFilterableMembers(filterProps, parameter);
            var constant = Expression.Constant(filter.ToLower());
            var toLowerMethod = typeof(string).GetMethod("ToLower", System.Type.EmptyTypes);
            Expression searchExp = null;

            // Dividir el filtro en palabras individuales
            var filterWords = GenerateFilterWords(filter);

            // Crear una lista para almacenar las expresiones generadas para cada palabra
            var wordFilterExpressions = new List<Expression>();

            foreach (var word in filterWords)
            {
                // Crear una lista para almacenar las expresiones generadas para esta palabra
                var wordExpressions = new List<Expression>();

                foreach (var member in members)
                {
                    if (member.Type == typeof(string))
                    {
                        // e => e.Member != null
                        var notNullExp = Expression.NotEqual(member, Expression.Constant(null));
                        // e => e.Member.ToLower() 
                        var toLowerExp = Expression.Call(member, toLowerMethod);
                        // e => e.Member.ToLower().Contains(word)
                        var containsExp = Expression.Call(toLowerExp, "Contains", null, Expression.Constant(word));
                        // e => e.Member != null && e.Member.ToLower().Contains(word)
                        var singleWordFilter = Expression.AndAlso(notNullExp, containsExp);

                        // Agregar la expresión generada para esta columna a la lista
                        wordExpressions.Add(singleWordFilter);
                    }
                    else
                    {
                        // Mantener la lógica existente para las columnas que no son de tipo string
                        if (Regex.IsMatch(filter, @"^\d+$") && IsNumeric(member.Type))
                        {
                            var value = Convert.ChangeType(filter, member.Type);
                            if (value != null)
                            {
                                var filterExpression = Expression.Equal(member, Expression.Constant(value));
                                wordExpressions.Add(filterExpression);
                            }
                        }
                    }
                }

                // Combinar las expresiones generadas para esta palabra con OrElse
                var wordCombinedExpression = wordExpressions.Any() ? wordExpressions.Aggregate(Expression.OrElse) : null;

                // Agregar la expresión combinada de la palabra a la lista
                if (wordCombinedExpression != null)
                {
                    wordFilterExpressions.Add(wordCombinedExpression);
                }
            }

            // Combinar las expresiones generadas para cada palabra con AndAlso
            var combinedSearchExp = wordFilterExpressions.Any() ? wordFilterExpressions.Aggregate(Expression.AndAlso) : null;

            if (combinedSearchExp != null)
            {
                searchExp = combinedSearchExp;
            }

            var predicate = Expression.Lambda<Func<T, bool>>(searchExp ?? Expression.Constant(true), parameter);

            return predicate;
        }
        public static string[] GenerateFilterWords(string filter)
        {
            return filter.ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static bool IsNumeric(Type type)
        {
            return type == typeof(decimal)
                || type == typeof(int)
                || type == typeof(uint)
                || type == typeof(long)
                || type == typeof(ulong)
                || type == typeof(short)
                || type == typeof(ushort)
                || type == typeof(byte)
                || type == typeof(sbyte)
                || type == typeof(float)
                || type == typeof(double);
        }


        public static IQueryable<T> GenerateSoftDeleteQuery(IQueryable<T> query)
        {
            var parameter = Expression.Parameter(typeof(T), "e");
            var member = Expression.Property(parameter, "Activo");
            var constant = Expression.Constant(true);
            // e => e.Activo == true
            var activeExp = Expression.Equal(member, constant);
            var predicate = Expression.Lambda<Func<T, bool>>(activeExp, parameter);

            return query.Where(predicate);
        }

        public static IQueryable<T> GenerateOrderByQuery(IQueryable<T> query, List<string> orderByProperties)
        {
            var type = typeof(T);
            var parameter = Expression.Parameter(type, "p");

            for (var i = 0; i < orderByProperties.Count(); i++)
            {
                var splitedOrder = orderByProperties[i].Split(':');
                var columnName = splitedOrder[0];
                var orderType = splitedOrder.Count() > 1 ? splitedOrder[1].ToLower() : "asc";
                var member = columnName.Split('.')
                    .Aggregate((Expression)parameter, Expression.PropertyOrField);
                var expression = Expression.Lambda(member, parameter);
                var orderMethod = "";

                if (i == 0)
                {
                    // la primera vez es orderBy
                    orderMethod = orderType == "asc" ? "OrderBy" : "OrderByDescending";
                }
                else
                {
                    // luego es ThenBy
                    orderMethod = orderType == "asc" ? "ThenBy" : "ThenByDescending";
                }

                // typeof(T) is the type of the Entity
                // exp.Body.Type is the type of the property. 
                Type[] types = new Type[] { type, expression.ReturnType };

                // Build the call expression
                // It will look something like:
                //     OrderBy*(x => x.Cassette) or Order*(x => x.SlotNumber)
                //     ThenBy*(x => x.Cassette) or ThenBy*(x => x.SlotNumber)
                var callExpression = Expression.Call(typeof(Queryable), orderMethod, types,
                    query.Expression, expression);

                query = query.Provider.CreateQuery<T>(callExpression);
            }

            return query;
        }

        // convierte un string a capitalize
        private static string FirstCharToUpper(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            return char.ToUpper(value[0]) + value.Substring(1);
        }

        private static MemberExpression[] GenerateFilterableMembers(List<string> filterProps, ParameterExpression parameter)
        {
            var members = new MemberExpression[filterProps.Count()];

            for (int i = 0; i < filterProps.Count(); i++)
            {
                if (filterProps[i].Contains('.'))
                {   // el filtro es una propiedad de una entidad anidada
                    // ej. u => u.Rol.Nombre
                    Expression nestedMember = parameter;
                    foreach (var prop in filterProps[i].Split('.'))
                    {
                        nestedMember = Expression.PropertyOrField(nestedMember, prop);
                    }
                    members[i] = (MemberExpression)nestedMember;
                }
                else
                {
                    // el filtro es una propiedad de la entidad
                    // ej. u => u.Username
                    members[i] = Expression.Property(parameter, filterProps[i]);
                }
            }

            return members;
        }

    }
}
