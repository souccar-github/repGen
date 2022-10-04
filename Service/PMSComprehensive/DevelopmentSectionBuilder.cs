#region

using Model.PMS.Entities;
using Model.PMS.ValueObjects;
using Model.Training.ValueObjects;

#endregion

namespace Service.PMSComprehensive
{
    public class DevelopmentSectionBuilder
    {
        #region Properties

        private readonly EntityService<Appraisal> _service;

        public DevelopmentSectionBuilder()
        {
            _service = new EntityService<Appraisal>();
        }

        #endregion

        public void Create(int appraisalId)
        {
            Appraisal appraisal = _service.LoadById(appraisalId);

            var developmentSection = new DevelopmentSection();

            // TODO Complete objective section scenario as POC + Facts for test

            #region Objective Sections
            //todo refactor here
            //commment after delete phase from appraisal
            //if (appraisal.ObjectiveSections.Count > 0)
            //{
            //    if (appraisal.AppraisalPhase.FullMarkThreshold > 3)
            //    {
            //        developmentSection.AddSkill(new Skill {Name = "Test"});
            //    }
            //}

            #endregion

            appraisal.AddDevelopmentSection(developmentSection);
            _service.Update(appraisal);
        }
    }
}