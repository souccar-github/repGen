#region

using System.Linq;
using Model.PMS.Entities;

#endregion

namespace Service.PMSComprehensive
{
    public class AppraisalPhaseHelper
    {

        private static readonly EntityService<AppraisalPhase> _service = new EntityService<AppraisalPhase>();


        public static AppraisalPhase GetCurrent()
        {
            return _service.GetAll().OrderByDescending(phase => phase.Id).FirstOrDefault();
        }

        public static void AddAppraisal(Appraisal appraisal)
        {
            //var appraisalPhase = GetCurrent();

            //appraisalPhase.AddAppraisal(appraisal);

            //_service.Update(appraisalPhase);
        }

        public static Appraisal GetAppraisalForEmployee(int employeeId)
        {
            //var query = from appraisalPhase in _service.GetAll()
            //            from appraisal in appraisalPhase.Appraisals
            //            where appraisal.Employee.Id == employeeId
            //            orderby appraisalPhase.Id descending
            //            select appraisal;
            //return query.FirstOrDefault();
            return null;
        }
    }
}