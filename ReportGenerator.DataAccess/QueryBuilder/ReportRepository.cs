using Repository.NHibernate;
using Souccar.ReportGenerator.Domain.QueryBuilder;

namespace Souccar.ReportGenerator.DataAccess.QueryBuilder
{
    public class ReportRepository : Repository<Report>, IReportRepository
    {
    }
}