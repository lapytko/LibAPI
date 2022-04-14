using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using LibAPI.Utils.DataFilterUtils.Enums;
using LibAPI.Utils.DataFilterUtils.Models;

namespace LibAPI.Utils.DataFilterUtils
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ToFilterView<T>(
            this IQueryable<T> query, FilterModel filter)
        {
            return query.ToFilterView(filter, out _);
        }

        public static IQueryable<T> ToFilterView<T>(
            this IQueryable<T> query, FilterModel filter, out int count)
        {
            query = Filter(query, filter.Filter);
            count = query.Count();
            query = Sort(query, filter.Sort);
            query = Limit(query, filter.Limit, filter.Offset);

            return query;
        }

        private static IQueryable<T> Filter<T>(
            IQueryable<T> queryable, Filter filter)
        {
            if (filter?.Logic == null) return queryable;
            var filters = GetAllFilters(filter);
            var values = filters.Select(f => f.Value).ToArray();
            var where = Transform(filter, filters);
            queryable = queryable.Where(where, values);

            return queryable;
        }

        private static IQueryable<T> Sort<T>(
            IQueryable<T> queryable, IEnumerable<Sort> sort)
        {
            if (sort == null)
                return queryable;
            var enumerable = sort as Sort[] ?? sort.ToArray();
            if (!enumerable.Any()) return queryable;
            var ordering = string.Join(",",
                enumerable.Select(s => $"{s.Field} {s.Dir}"));
            return queryable.OrderBy(ordering);
        }

        private static IQueryable<T> Limit<T>(
            IQueryable<T> queryable, int? limit, int? offset)
        {
            var query = queryable;
            if (offset != null)
                query = query.Skip((int) offset);
            if (limit != null)
                query = query.Take((int) limit);
            return query;
        }

        private static readonly IDictionary<string, string>
            Operators = new Dictionary<string, string>
            {
                {"=", "="},
                {"!=", "!="},
                {"<", "<"},
                {"<=", "<="},
                {">", ">"},
                {">=", ">="},
                {"startswith", "StartsWith"},
                {"endswith", "EndsWith"},
                {"contains", "Contains"},
                {"doesnotcontain", "Contains"},
            };

        public static IList<Filter> GetAllFilters(Filter filter)
        {
            var filters = new List<Filter>();
            GetFilters(filter, filters);
            return filters;
        }

        private static void GetFilters(Filter filter, IList<Filter> filters)
        {
            if (filter.Filters != null && filter.Filters.Any())
            {
                foreach (var item in filter.Filters)
                {
                    GetFilters(item, filters);
                }
            }
            else
            {
                filters.Add(filter);
            }
        }

        public static string Transform(Filter filter, IList<Filter> filters)
        {
            if (filter.Filters != null && filter.Filters.Any())
            {
                return "(" + string.Join(" " + filter.Logic + " ",
                    filter.Filters.Select(f => Transform(f, filters)).ToArray()) + ")";
            }

            var index = filters.IndexOf(filter);
            var comparison = Operators[filter.Operator];
            if (filter.Operator == "doesnotcontain")
            {
                return string.Format("({0} != null && !{0}.ToString().{1}(@{2}))",
                    filter.Field, comparison, index);
            }

            return string.Format(
                comparison is "StartsWith" or "EndsWith" or "Contains"
                    ? filter.Case == MatchCaseEnum.IgnoreCase
                        ? "({0} != null && {0}.ToLower().{1}(@{2}.ToString().ToLower()))"
                        : "({0} != null && {0}.{1}(@{2}))"
                    : "{0} {1} @{2}", filter.Field, comparison, index);
        }
    }
}