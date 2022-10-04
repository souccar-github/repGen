using Repository.NHibernate;
using Souccar.ReportGenerator.Domain.Classification;

namespace Souccar.ReportGenerator.DataAccess.Classification
{
    public class ReportTemplateRepository : Repository<ReportTemplate>, IReportTemplateRepository
    {
    }
}