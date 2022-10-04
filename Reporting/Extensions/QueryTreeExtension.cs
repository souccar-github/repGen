using Souccar.ReportGenerator.Domain.QueryBuilder;
using System.Linq;

namespace Reporting.Extensions
{
    public static class QueryTreeExtension
    {
        public static string GetTableName(this QueryTree queryTree)
        {
            return queryTree.FullClassName.Split('.').LastOrDefault();
        }
    }
}
